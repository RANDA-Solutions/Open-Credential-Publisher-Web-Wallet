using System;
using System.Security.Cryptography;

namespace OpenCredentialPublisher.ClrWallet.Utilities
{
    /// <summary>
    /// Cryptography utilities.
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        /// Create a cryptographically random string.
        /// </summary>
        /// <param name="length">The length of the string to create. Must be greater than 0.</param>
        /// <param name="allowedChars">The allowed characters. Defaults to 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.</param>
        /// <returns>The cryptographically random string.</returns>
        public static string CreateRandomString(int length, string allowedChars = null)
        {
            if (length <= 0)
                throw new ArgumentException($"{nameof(length)} must be greater than 0");
            var allowed = allowedChars == null ? 
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*".ToCharArray() : 
                allowedChars.ToCharArray();

            using var csp = new RNGCryptoServiceProvider();
            var randomNumber = new byte[length];
            csp.GetBytes(randomNumber);

            var l = allowed.Length;
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                chars[i] = allowed[randomNumber[i] % l];
            }

            return new string(chars);
        }
    }
}
