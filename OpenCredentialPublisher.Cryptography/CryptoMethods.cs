//using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.WebUtilities;
using Sodium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OpenCredentialPublisher.Cryptography
{
    public static class CryptoMethods
    {
        /// <summary>
        /// Used for encrypting shared secrets/keys
        /// </summary>
        /// <param name="rsaKeyBlob">Expects RSA public key in CSP blob format</param>
        /// <param name="value">Can be a string of any length but recommended length is under 214</param>
        /// <returns>Encrypted byte array</returns>
        public static Byte[] EncryptString(Byte[] rsaKeyBlob, String value)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.ImportCspBlob(rsaKeyBlob);
                return provider.Encrypt(UTF8Encoding.UTF8.GetBytes(value), true);
            }
        }
        /// <summary>
        /// Used for decrypting shared secrets/keys
        /// </summary>
        /// <param name="rsaKeyBlob">Expects RSA private key in CSP blob format</param>
        /// <param name="value"></param>
        /// <returns>Unencrypted string</returns>
        public static String DecryptBytes(Byte[] rsaKeyBlob, Byte[] value)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.ImportCspBlob(rsaKeyBlob);
                return UTF8Encoding.UTF8.GetString(provider.Decrypt(value, true));
            }
        }

        public static string SignString(KeyAlgorithmEnum keyAlgorithm, byte[] keyBlob, String value)
        {
            return keyAlgorithm switch
            {
                KeyAlgorithmEnum.Ed25519 => SignEd25519(keyBlob, value),
                _ => SignRsa(keyBlob, value)
            };
        }

        private static string SignRsa(byte[] keyBlob, String value)
        {
            byte[] signedBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    var encodedBytes = UTF8Encoding.UTF8.GetBytes(value);
                    rsa.ImportCspBlob(keyBlob);
                    signedBytes = rsa.SignData(encodedBytes, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                    return WebEncoders.Base64UrlEncode(signedBytes);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        private static string SignEd25519(byte[] keyBlob, string value)
        {
            try
            {
                var encodedBytes = UTF8Encoding.UTF8.GetBytes(value);
                var signedBytes = Sodium.PublicKeyAuth.SignDetached(encodedBytes, keyBlob);
                return WebEncoders.Base64UrlEncode(signedBytes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool VerifySignature(KeyAlgorithmEnum keyAlgorithm, byte[] publicKeyBlob, String signature, String originalData)
        {
            var signedBytes = WebEncoders.Base64UrlDecode(signature);
            var originalBytes = UTF8Encoding.UTF8.GetBytes(originalData);

            return keyAlgorithm switch
            {
                KeyAlgorithmEnum.Ed25519 => VerifyEd25519(publicKeyBlob, signedBytes, originalBytes),
                _ => VerifyRsa(publicKeyBlob, signedBytes, originalBytes)
            };
        }

        private static bool VerifyRsa(byte[] publicKeyBlob, byte[] signature, byte[] originalData)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(publicKeyBlob);
                return rsa.VerifyData(originalData, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            }
        }

        private static bool VerifyEd25519(byte[] publicKeyBlob, byte[] signature, byte[] originalData)
        {
            return Sodium.PublicKeyAuth.VerifyDetached(signature, originalData, publicKeyBlob);
        }

        public static string GenerateSecretKey(int length, string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_")
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];

                // If chars.Length isn't a power of 2 then there is a bias if we simply use the modulus operator. The first characters of chars will be more probable than the last ones.
                // buffer used if we encounter an unusable random byte. We will regenerate it in this buffer
                byte[] buffer = null;

                // Maximum random number that can be used without introducing a bias
                int maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                crypto.GetBytes(data);

                char[] result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    byte value = data[i];

                    while (value > maxRandom)
                    {
                        if (buffer == null)
                        {
                            buffer = new byte[1];
                        }

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = chars[value % chars.Length];
                }

                return new string(result);
            }
        }
        /// <summary>
        /// Generates RSA Key in CSP Blob format
        /// </summary>
        /// <param name="includePrivateParameters"></param>
        /// <returns></returns>
        public static Byte[] GenerateRsaKey(bool includePrivateParameters = true)
        {

            using (var provider = new RSACryptoServiceProvider(2048))
            {
                return provider.ExportCspBlob(includePrivateParameters);
            }
        }

        public static RSAParameters FromCspBlobToRSAParameters(this byte[] cspBlob, bool includePrivateParameters = true)
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.ImportCspBlob(cspBlob);
                return provider.ExportParameters(includePrivateParameters);
            }
        }

        public static Byte[] GetPublicRsaKey(Byte[] keyBlob)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.ImportCspBlob(keyBlob);
                return provider.ExportCspBlob(false);
            }
        }

        public static Byte[] GetPublicEd25519Key(Byte[] keyBlob)
        {

            if (keyBlob.Length == 96)
            {
                return keyBlob.Skip(64).ToArray();
            }

            if (keyBlob.Length == 92)
            {
                return keyBlob.Skip(48).ToArray();
            }

            return PublicKeyAuth.ExtractEd25519PublicKeyFromEd25519SecretKey(keyBlob);
        }

        public static (byte[] publicKey, byte[] privateKey) GenerateEd25519Keys()
        {
            var keyPair = PublicKeyAuth.GenerateKeyPair();
            return (keyPair.PublicKey, keyPair.PrivateKey);
        }

        public static byte[] GenerateEd25519Key()
        {
            var (publicKey, privateKey) = GenerateEd25519Keys();
            return privateKey.Concat(publicKey).ToArray();
        }

        public static Byte[] GenerateKey(KeyAlgorithmEnum keyType)
        {
            return keyType switch
            {
                KeyAlgorithmEnum.Ed25519 => GenerateEd25519Key(),
                _ => GenerateRsaKey()
            };
        }

        public static Byte[] GetPublicKey(KeyAlgorithmEnum keyType, Byte[] keyBlob)
        {
            return keyType switch
            {
                KeyAlgorithmEnum.RSA => GetPublicRsaKey(keyBlob),
                KeyAlgorithmEnum.Ed25519 => GetPublicEd25519Key(keyBlob),
                _ => keyBlob
            };
        }
    }
}
