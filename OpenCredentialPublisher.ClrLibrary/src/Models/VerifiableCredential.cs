using Newtonsoft.Json;
using OpenCredentialPublisher.Cryptography;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.ClrLibrary.Converters.Newtonsoft;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace OpenCredentialPublisher.ClrLibrary.Models
{

    public class VerifiableCredential
    {
        [JsonProperty("@context", Order = 1), JsonPropertyName("@context")]
        public List<String> Contexts { get; set; }

        [JsonProperty("id", Order = 2, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("id")]
        public String Id { get; set; }

        [JsonProperty("type", Order = 3), JsonPropertyName("type")]
        public List<String> Types { get; set; }

        [JsonProperty("issuer", Order = 4), JsonPropertyName("issuer")]
        public String Issuer { get; set; }

        [JsonProperty("issuanceDate", Order = 5), JsonPropertyName("issuanceDate")]
        [Newtonsoft.Json.JsonConverter(typeof(DateConverter<DateTime>), "o"), System.Text.Json.Serialization.JsonConverter(typeof(Converters.Json.DateConverter))]
        public DateTime IssuanceDate { get; set; }

        [JsonProperty("credentialSubject", Order = 6), JsonPropertyName("credentialSubject")]
        [Newtonsoft.Json.JsonConverter(typeof(SingleOrArrayConverter<ICredentialSubject>)), System.Text.Json.Serialization.JsonConverter(typeof(Converters.Json.PolymorphicConverter<ICredentialSubject, List<ICredentialSubject>>))]
        public List<ICredentialSubject> CredentialSubjects { get; set; }

        [JsonProperty("credentialStatus", Order = 7, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("credentialStatus")]
        public CredentialStatus CredentialStatus { get; set; }

        [JsonProperty("proof", Order = 8, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("proof")]
        public Proof Proof { get; set; }

        public string Sign(KeyAlgorithmEnum keyAlgorithm, byte[] keyBytes, String challenge = default)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.None);
            json += challenge;

            var signature = CryptoMethods.SignString(keyAlgorithm, keyBytes, json);
            return signature;
        }

        public void CreateProof(KeyAlgorithmEnum keyAlgorithm, byte[] keyBytes, ProofPurposeEnum proofPurpose, Uri verificationMethod, String challenge)
        {
            var proof = new Proof()
            {
                Created = DateTime.UtcNow,
                Challenge = challenge,
                ProofPurpose = proofPurpose.ToString(),
                VerificationMethod = verificationMethod.ToString()
            };

            proof.Type = keyAlgorithm switch
            {
                KeyAlgorithmEnum.Ed25519 => ProofTypeEnum.Ed25519Signature2018.ToString(),
                _ => ProofTypeEnum.RsaSignature2018.ToString()
            };

            proof.Signature = Sign(keyAlgorithm, keyBytes, challenge);

            Proof = proof;
        }

        public Boolean VerifyProof(KeyAlgorithmEnum keyAlgorithm, byte[] publicKeyBytes)
        {
            var proof = Proof;
            Proof = null;

            var json = System.Text.Json.JsonSerializer.Serialize(this, new JsonSerializerOptions { IgnoreNullValues = true });
            json += proof.Challenge;

            Proof = proof;

            using var crypto = new RSACryptoServiceProvider();
            crypto.ImportCspBlob(publicKeyBytes);

            var signedBytes = WebEncoders.Base64UrlDecode(proof.Signature);
            var originalBytes = UTF8Encoding.UTF8.GetBytes(json);
            var digest = ComputeHash("RS512", originalBytes);

            return crypto.VerifyHash(digest, signedBytes, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
        }

        public Boolean VerifyProof(RSAParameters rsaParameters)
        {
            using var crypto = new RSACryptoServiceProvider();
            crypto.ImportParameters(rsaParameters);

            return VerifyProof(KeyAlgorithmEnum.RSA, crypto.ExportCspBlob(false));
        }

        public void SetIssuer(Uri uri)
        {
            Issuer = uri.ToString();
        }

        public void SetIssuer(String did, String name)
        {
            var issuer = new Issuer
            {
                Id = did,
                Name = name
            };

            Issuer = JsonConvert.SerializeObject(issuer);
        }

        public void SetIssuer(Uri uri, String name)
        {
            var issuer = new Issuer
            {
                Id = uri.ToString(),
                Name = name
            };

            Issuer = JsonConvert.SerializeObject(issuer);
        }

        private byte[] ComputeHash(string algorithm, byte[] bytesData) =>
            algorithm switch
            {
                "RS256" => new SHA256CryptoServiceProvider().ComputeHash(bytesData),
                "RS512" => new SHA512CryptoServiceProvider().ComputeHash(bytesData),
                _ => throw new NotImplementedException($"{algorithm} has not been implemented yet.")
            };


    }
}
