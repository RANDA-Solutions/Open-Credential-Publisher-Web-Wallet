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
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PemUtils;
using Serilog;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Extensions;

namespace OpenCredentialPublisher.ClrWallet.Pages
{
    public class CredentialPackageDisplayPageModel : PageModel
    {
        protected readonly WalletDbContext Context;
        protected readonly IHttpClientFactory Factory;
        protected readonly ClrService ClrService;
        protected readonly ConnectService _connectService;

        protected CredentialPackageDisplayPageModel(
            WalletDbContext context,
            IHttpClientFactory factory,
            IConfiguration configuration,
            ClrService clrService,
            ConnectService connectService)
        {
            Context = context;
            Factory = factory;
            ClrService = clrService;
            _connectService = connectService;
        }

        public CredentialPackageModel CredentialPackage { get; set; }


        public async Task<IActionResult> OnPostVerifiableCredential([FromForm]int credentialPackageId)
        {
            var package = await Context.CredentialPackages
                .Include(cp => cp.VerifiableCredential)
                .FirstOrDefaultAsync(cp => cp.Id == credentialPackageId);

            if (package?.VerifiableCredential == null)
                return new ObjectResult(new Verification(credentialPackageId.ToString(), error: "Cannot find the credential."));

            var verifiableCredential = JsonSerializer.Deserialize<VerifiableCredential>(package.VerifiableCredential.Json);
            if (verifiableCredential.CredentialStatus != null)
            {
                var document = await _connectService.GetRevocationDocumentAsync(verifiableCredential.CredentialStatus.Id);
                if (document?.Revocations != null && document.Revocations.Any())
                {
                    var revocation = document.Revocations.FirstOrDefault(r => r.Id == verifiableCredential.Id);
                    if (revocation != null)
                    {
                        package.Revoked = true;
                        package.RevocationReason = document.Statuses[revocation.Status];
                        await Context.SaveChangesAsync();
                        return new OkObjectResult(new Verification(credentialPackageId.ToString(), message:
                    "Revoked <img class='key-host-statement'" +
                    $" src='{Request.PathBase}/images/noun_Info_742307.svg' style='width: 1.5em'" +
                    $" data-toggle='tooltip' data-html='true' title='<div>{document.Statuses[revocation.Status]}</div>' />"));
                    }
                }
            }

            var verificationUrl = SanitizePath(verifiableCredential.Proof.VerificationMethod);

            var pemString = await _connectService.GetKeyAsync(verificationUrl);

            RSAParameters rsaParameters;
            await using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(pemString)))
            {
                using var reader = new PemReader(stream);
                rsaParameters = reader.ReadRsaKey();
            }

            if (verifiableCredential.VerifyProof(rsaParameters))
            {
                return new OkObjectResult(new Verification(credentialPackageId.ToString(), message:
                    "Verified Proof <img class='key-host-statement'" +
                    $" src='{Request.PathBase}/images/noun_Info_742307.svg' style='width: 1.5em'" +
                    " data-toggle='tooltip' data-html='true' title='<div>Verification method" +
                    $" was used to verify proof:</div>{verificationUrl}' />"));
            }

            return new OkObjectResult(new Verification(credentialPackageId.ToString(), message:
                        "Proof could not be verified."));
        }

        private string SanitizePath(string url)
        {
            var builder = new UriBuilder(url);
            if (builder.Path.StartsWith("//"))
            {
                builder.Path = builder.Path.Trim('/');
            }
            return builder.ToString();
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

            var clrEntity =  await Context.Clrs
                    .Include(x => x.CredentialPackage)
                    .Include(x => x.Authorization)
                    .SingleOrDefaultAsync(c => c.Id == clrEntityId);

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

            var clr = JsonSerializer.Deserialize<ClrViewModel>(clrEntity.Json);

            Verification result = null;

            if (!string.IsNullOrEmpty(assertionId))
            {
                var assertion = clr.Assertions?.SingleOrDefault(a => a.Id == assertionId);

                if (assertion != null)
                {
                    result = await VerifyHostedAssertion(clrEntity, assertion);
                }

                else if (clr.SignedAssertions != null)
                {
                    foreach (var signedAssertion in clr.SignedAssertions)
                    {
                        assertion = signedAssertion.DeserializePayload<AssertionDType>();
                        var publicKey = assertion.Achievement.Issuer.PublicKey;

                        if (assertion.Id == assertionId)
                        {
                            result = await VerifySignature(assertionId, signedAssertion, publicKey);
                            break;
                        }
                    }
                }

                if (result == null)
                {
                    return new ObjectResult(new Verification(assertionId,
                        error: "Cannot find assertion."));
                }

                if (result.Error == null)
                {
                    if (VerifyRecipient(assertion.Recipient, clr.Learner))
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
                var endorsement = GetAllEndorsements(clr).SingleOrDefault(e => e.Id == endorsementId);

                if (endorsement != null)
                {
                    result = await VerifyHostedEndorsement(endorsement);
                }

                else if (clr.SignedAssertions != null)
                {
                    foreach (var signedEndorsement in GetAllSignedEndorsements(clr))
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
            else if (clr.Verification?.Type == VerificationDType.TypeEnum.HostedEnum)
            {
                result = await VerifyHostedClr(clrEntity, clr);
            }
            else if (clr.Verification?.Type == VerificationDType.TypeEnum.SignedEnum)
            {
                if (clrEntity.SignedClr == null)
                {
                    result = new Verification(clrId, "Missing signature");
                }
                else
                {
                    var publicKey = clr.Publisher.PublicKey;
                    if (clr.Id == clrId)
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

            var client = Factory.CreateClient(ClrHttpClient.Default);
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

                        await Context.SaveChangesAsync();

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
                var newClr = await ClrService.GetClrAsync(this, clrEntity.Authorization.Id, clr.Id);

                if (ModelState.IsValid)
                {
                    // Replace clr in the database

                    if (clr.ToJson() != newClr.ToJson())
                    {
                        clrEntity.Json = JsonSerializer.Serialize(newClr, new JsonSerializerOptions { IgnoreNullValues = true });

                        await Context.SaveChangesAsync();

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

            var client = Factory.CreateClient(ClrHttpClient.Default);
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
                var certificate = await Context.Certificates.FindAsync(keyUri.Host);

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
            var certificate = Context.Certificates.Find(arg1.RequestUri.Host);
            if (certificate == null)
            {
                certificate = new CertificateModel { Host = arg1.RequestUri.Host };
                Context.Certificates.Add(certificate);
            }

            certificate.IssuedByName = arg2.IssuerName.Format(false);
            certificate.IssuedToName = arg2.SubjectName.Format(false);

            Context.SaveChanges();

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
