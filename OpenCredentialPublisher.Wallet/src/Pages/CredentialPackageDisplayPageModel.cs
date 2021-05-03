using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using PemUtils;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet.Pages
{
    public class CredentialPackageDisplayPageModel : DisplayPageModel
    {
        protected CredentialPackageDisplayPageModel(
            IHttpClientFactory factory,
            ClrService clrService,
            ConnectService connectService,
            CredentialService credentialService,
            CredentialPackageService credentialPackageService,
            BadgrService badgrService,
            RevocationService revocationService):
                base(factory, clrService, connectService, credentialService, credentialPackageService, badgrService, revocationService)
        {
            
        }

        public CredentialPackageViewModel CredentialPackageViewModel { get; set; }


        public async Task<IActionResult> OnPostVerifiableCredential([FromForm]int credentialPackageId)
        {
            var package = await _credentialService.GetAsync(credentialPackageId);

            if (package?.VerifiableCredential == null)
                return new ObjectResult(new Verification(credentialPackageId.ToString(), error: "Cannot find the credential."));

            return await CheckVerifiableCredential(package, credentialPackageId.ToString());
        }

        /// <summary>
        /// Verify the assertion, clr, or endorsement
        /// </summary>
        public async Task<IActionResult> OnPost(
            [FromForm] int clrEntityId,
            [FromForm] string assertionId,
            [FromForm] string clrId,
            [FromForm] string endorsementId)
        {
            if (!ModelState.IsValid)
            {
                return new ObjectResult(new Verification(assertionId, error: "Cannot find CLR."));
            }

            var resultId = assertionId ?? endorsementId ?? clrId;

            if (resultId == null)
            {
                throw new ArgumentException("No target specified");
            }

            var clrEntity = await _credentialService.GetClrAsync(clrEntityId);

            if (clrEntity == null)
            {
                return new ObjectResult(new Verification(resultId, error: "Cannot find CLR."));
            }

            if (clrEntity?.CredentialPackage != null)
            {
                if (clrEntity.CredentialPackage.Revoked)
                {
                    return new OkObjectResult(new Verification(resultId, message:
                    "Revoked <img class='key-host-statement'" +
                    $" src='{Request.PathBase}/images/noun_Info_742307.svg' style='width: 1.5em'" +
                    $" data-toggle='tooltip' data-html='true' title='<div>{clrEntity.CredentialPackage.RevocationReason}</div>' />"));
                }
            }

            var clrVM = ClrViewModel.FromClrModel(clrEntity);

            Verification result = null;

            if (!string.IsNullOrEmpty(assertionId))
            {
                var assertionVM = clrVM.AllAssertions?.SingleOrDefault(a => a.Assertion.Id == assertionId);

                if (assertionVM != null && !assertionVM.Assertion.IsSigned)
                {                    
                    result = await VerifyHostedAssertion(clrEntity, assertionVM.Assertion);
                }

                else if (assertionVM != null && assertionVM.Assertion.IsSigned)
                {
                    var publicKey = assertionVM.Assertion.Achievement.Issuer.PublicKey;
                    result = await VerifySignature(assertionId, assertionVM.SignedAssertion, publicKey);
                }

                if (result == null)
                {
                    return new ObjectResult(new Verification(assertionId,
                        error: "Cannot find assertion."));
                }

                if (result.Error == null)
                {
                    if (VerifyRecipient(assertionVM.Assertion.Recipient, clrVM.RawClrDType.Learner))
                    {
                        result.Message += " - Verified Recipient";
                    }
                    else
                    {
                        result.Error = "Recipient Does Not Match Learner";
                    }
                }
            }

            else if (!string.IsNullOrEmpty(endorsementId))
            {
                var endorsement = GetAllEndorsements(clrVM.RawClrDType).SingleOrDefault(e => e.Id == endorsementId);

                if (endorsement != null)
                {
                    result = await VerifyHostedEndorsement(endorsement);
                }

                else if (clrVM.RawClrDType.SignedAssertions != null)
                {
                    foreach (var signedEndorsement in GetAllSignedEndorsements(clrVM.RawClrDType))
                    {
                        endorsement = signedEndorsement.DeserializePayload<EndorsementDType>();
                        var publicKey = endorsement.Issuer.PublicKey;

                        if (endorsement.Id == endorsementId)
                        {
                            result = await VerifySignature(endorsementId, signedEndorsement, publicKey);
                            break;
                        }
                    }
                }

                if (result == null)
                {
                    return new ObjectResult(new Verification(assertionId,
                        error: "Cannot find endorsement."));
                }
            }
            else if (clrVM.RawClrDType.Verification?.Type == VerificationDType.TypeEnum.HostedEnum)
            {
                result = await VerifyHostedClr(clrEntity, clrVM.RawClrDType);
            }
            else if (clrVM.RawClrDType.Verification?.Type == VerificationDType.TypeEnum.SignedEnum)
            {
                if (clrEntity.SignedClr == null)
                {
                    result = new Verification(clrId, "Missing signature");
                }
                else
                {
                    var publicKey = clrVM.RawClrDType.Publisher.PublicKey;
                    if (clrVM.Clr.Identifier == clrId)
                    {
                        result = await VerifySignature(clrId, clrEntity.SignedClr, publicKey);
                    }
                }
            }

            return new ObjectResult(result);
        }

        private List<AssertionDType> GetAllAssertions(ClrDType clr)
        {
            var assertions = new List<AssertionDType>();
            if (clr.Assertions != null)
            {
                assertions.AddRange(clr.Assertions);
            }

            if (clr.SignedAssertions != null)
            {
                assertions.AddRange(clr.SignedAssertions.Select(s => s.DeserializePayload<AssertionDType>()));
            }

            return assertions;
        }

        private List<EndorsementDType> GetAllEndorsements(ClrDType clr)
        {
            var endorsements = new List<EndorsementDType>();

            if (clr.Learner.Endorsements != null)
            {
                endorsements.AddRange(clr.Learner.Endorsements);
            }

            if (clr.Publisher.Endorsements != null)
            {
                endorsements.AddRange(clr.Publisher.Endorsements);
            }

            var assertions = GetAllAssertions(clr);

            foreach (var assertion in assertions)
            {
                if (assertion.Endorsements != null)
                {
                    endorsements.AddRange(assertion.Endorsements);
                }

                if (assertion.Achievement != null)
                {
                    var achievement = assertion.Achievement;

                    if (achievement.Endorsements != null)
                    {
                        endorsements.AddRange(achievement.Endorsements);
                    }

                    var issuer = achievement.Issuer;

                    if (issuer.Endorsements != null)
                    {
                        endorsements.AddRange(issuer.Endorsements);
                    }
                }
            }

            return endorsements;
        }

        private List<string> GetAllSignedEndorsements(ClrDType clr)
        {
            var endorsements = new List<string>();

            if (clr.Learner.SignedEndorsements != null)
            {
                endorsements.AddRange(clr.Learner.SignedEndorsements);
            }

            if (clr.Publisher.SignedEndorsements != null)
            {
                endorsements.AddRange(clr.Publisher.SignedEndorsements);
            }

            var assertions = GetAllAssertions(clr);

            foreach (var assertion in assertions)
            {
                if (assertion.SignedEndorsements != null)
                {
                    endorsements.AddRange(assertion.SignedEndorsements);
                }

                if (assertion.Achievement != null)
                {
                    var achievement = assertion.Achievement;

                    if (achievement.SignedEndorsements != null)
                    {
                        endorsements.AddRange(achievement.SignedEndorsements);
                    }

                    var issuer = achievement.Issuer;

                    if (issuer.SignedEndorsements != null)
                    {
                        endorsements.AddRange(issuer.SignedEndorsements);
                    }
                }
            }

            return endorsements;
        }

        /// <summary>
        /// GET the assertion and process the result
        /// </summary>
        private async Task<Verification> VerifyHostedAssertion(ClrModel clrEntity, AssertionDType assertion)
        {
            if (assertion.Verification == null || assertion.Achievement == null)
            {
                return new Verification(assertion.Id, error: "Not verifiable");
            }

            var client = _factory.CreateClient(ClrHttpClient.Default);
            try
            {
                var response = await client.GetAsync(assertion.Id);

                if (response.IsSuccessStatusCode)
                {
                    // Replace assertion in the database

                    var content = await response.Content.ReadAsStringAsync();

                    var newAssertion = JsonSerializer.Deserialize<AssertionDType>(content);

                    if (assertion.ToJson() != newAssertion.ToJson())
                    {
                        var clr = JsonSerializer.Deserialize<ClrDType>(clrEntity.Json);
                        var oldAssertion = clr.Assertions.Single(a => a.Id == assertion.Id);

                        clr.Assertions.Remove(oldAssertion);
                        clr.Assertions.Add(newAssertion);
                        clrEntity.Json = JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true });

                        await _credentialService.UpdateClrAsync(clrEntity);

                        return new Verification(assertion.Id,
                            "Verified - Refresh page to see updates");
                    }

                    return new Verification(assertion.Id, "Verified - No changes");
                }

                return new Verification(assertion.Id, error: response.ReasonPhrase);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error processing hosted verification");
                return new Verification(assertion.Id, error: e.Message);
            }
        }

        private async Task<Verification> VerifyHostedClr(ClrModel clrEntity, ClrDType clr)
        {
            if (clrEntity.Authorization == null || clr.Verification == null)
            {
                return new Verification(clr.Id, error: "Not verifiable");
            }

            try
            {
                var newClr = await _clrService.GetClrAsync(this, clrEntity.Authorization.Id, clr.Id);

                if (ModelState.IsValid)
                {
                    // Replace clr in the database

                    if (clr.ToJson() != newClr.ToJson())
                    {
                        clrEntity.Json = JsonSerializer.Serialize(newClr, new JsonSerializerOptions { IgnoreNullValues = true });

                        await _credentialService.UpdateClrAsync(clrEntity);

                        return new Verification(clr.Id,
                            "Verified - Refresh page to see updates");
                    }

                    return new Verification(clr.Id, "Verified - No changes");
                }

                return new Verification(clr.Id, error: "Error getting CLR.");
            }
            catch (Exception e)
            {
                return new Verification(clr.Id, error: e.Message);
            }
        }

        private async Task<Verification> VerifyHostedEndorsement(EndorsementDType endorsement)
        {
            if (endorsement.Verification == null)
            {
                return new Verification(endorsement.Id, error: "Not verifiable");
            }

            var client = _factory.CreateClient(ClrHttpClient.Default);
            try
            {
                var response = await client.GetAsync(endorsement.Id);

                // Replace assertion in the database

                return response.IsSuccessStatusCode
                    ? new Verification(endorsement.Id, "Verified")
                    : new Verification(endorsement.Id, error: response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return new Verification(endorsement.Id, error: e.Message);
            }
        }

        private async Task<Verification> VerifySignature(string id, string compactJws,
            CryptographicKeyDType publicKey)
        {
            // Attempt to get a fresh copy of the public key and read the identity information
            // in the SSL certificate

            try
            {
                var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = CustomCallback
                };
                var client = new HttpClient(clientHandler);
                var content = await client.GetStringAsync(publicKey.Id);
                publicKey = JsonSerializer.Deserialize<CryptographicKeyDType>(content);
            }
            catch (Exception e)
            {
                //TODO Inform user that key is not available online
                Console.WriteLine(e);
            }

            try
            {
                RsaSecurityKey key;

                await using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(publicKey.PublicKeyPem)))
                {
                    using var reader = new PemReader(stream);
                    key = new RsaSecurityKey(reader.ReadRsaKey());
                }

                // Just check the signature

                var parameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateTokenReplay = false
                };

                var handler = new JwtSecurityTokenHandler
                {
                    MaximumTokenSizeInBytes = int.MaxValue
                };
                handler.ValidateToken(compactJws, parameters, out _);

                var keyUri = new Uri(publicKey.Id);
                var certificate = await _credentialService.GetCertificateAsync(keyUri.Host);

                if (certificate == null)
                {
                    return new Verification(id, message:
                        "Verified Signature <img class='key-host-statement'" +
                        $" src='{Request.PathBase}/images/noun_Info_742307.svg' style='width: 1.5em'" +
                        " data-toggle='tooltip' data-html='true'" +
                        " title='<div>The issuer&apos;s signing key could not be verified</div>' />");
                }

                return new Verification(id, message:
                    "Verified Signature <img class='key-host-statement'" +
                    $" src='{Request.PathBase}/images/noun_Info_742307.svg' style='width: 1.5em'" +
                    " data-toggle='tooltip' data-html='true' title='<div>The issuer&apos;s signing key" +
                    $" is hosted by:</div>{certificate.ToSubjectHtml()}' />");

            }
            catch (Exception e)
            {
                Log.Error(e, "Error while verifying signature.");
                return new Verification(id, error: e.Message);
            }
        }

        private bool CustomCallback(HttpRequestMessage arg1, X509Certificate2 arg2, X509Chain arg3, SslPolicyErrors arg4)
        {
            var certificate = _credentialService.GetCertificateAsync(arg1.RequestUri.Host).Result;
            if (certificate == null)
            {
                certificate = new CertificateModel { Host = arg1.RequestUri.Host };
                _credentialService.AddCertificateAsync(certificate).Wait();
            }

            certificate.IssuedByName = arg2.IssuerName.Format(false);
            certificate.IssuedToName = arg2.SubjectName.Format(false);

            _credentialService.UpdateCertificateAsync(certificate).Wait();

            return arg4 == SslPolicyErrors.None;
        }

        private static bool VerifyRecipient(IdentityDType recipient, ProfileDType learner)
        {
            if (recipient.Hashed)
            {
                using var sha256 = SHA256.Create();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(learner.Email));
                var builder = new StringBuilder();
                foreach (var x in bytes)
                {
                    builder.Append(x.ToString("x2"));
                }

                return recipient.Identity == $"sha256${builder}";
            }

            return recipient.Identity.Equals(learner.Email, StringComparison.InvariantCultureIgnoreCase);
        }

        private class Verification
        {
            public Verification(string id, string message = null, string error = null)
            {
                Error = error;
                Id = id.SafeId();
                Message = message;

                if (string.IsNullOrEmpty(error))
                {
                    if (string.IsNullOrEmpty(message))
                    {
                        Message = "Verified";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(message))
                    {
                        Message = "Not Verified";
                    }
                }
            }

            [JsonPropertyName("error")]
            public string Error { get; set; }

            [JsonPropertyName("id"), UsedImplicitly]
            public string Id { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}
