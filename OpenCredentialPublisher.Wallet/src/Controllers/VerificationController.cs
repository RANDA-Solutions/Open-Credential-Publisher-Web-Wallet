using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using PemUtils;
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
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class VerificationController : ApiController<VerificationController>
    {
        private readonly CredentialPackageService _credentialPackageService;
        private readonly ClrDetailService _clrDetailService;
        private readonly CredentialService _credentialService;
        private readonly RevocationService _revocationService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _factory;
        private readonly ClrService _clrService;
        private readonly ConnectService _connectService;
        private readonly UserManager<ApplicationUser> _userManager;

        private RevocationListModel _revocationsListModel = new RevocationListModel(new List<RevocationModel>());
        private List<CredentialPackageViewModel> CredentialPackageVMs { get; set; } = new List<CredentialPackageViewModel>();
        public VerificationController(UserManager<ApplicationUser> userManager, ILogger<VerificationController> logger, ClrDetailService clrDetailService
            , IHttpClientFactory factory, RevocationService revocationService, IOptions<SiteSettingsOptions> siteSettings
            , CredentialService credentialService, CredentialPackageService credentialPackageService, IHttpContextAccessor httpContextAccessor
            , ClrService clrService, ConnectService connectService) : base(logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _credentialPackageService = credentialPackageService;
            _clrDetailService = clrDetailService;
            _credentialService = credentialService;
            _clrService = clrService;
            _revocationService = revocationService;
            _siteSettings = siteSettings?.Value ?? throw new NullReferenceException("Site settings were not set.");
            _httpContextAccessor = httpContextAccessor;
            _factory = factory;
            _connectService = connectService;
        }

        /// <summary>
        /// Verifies an entity
        /// POST api/verification/Verify
        /// </summary>
        /// <returns>verification response</returns>
        [HttpPost("Verify")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> Verify(VerifyVM input)
        {
            try
            {
                var userId = User.UserId();
                if (!ModelState.IsValid)
                {
                    return ApiOk(new Verification(input.AssertionId, error: "Cannot find CLR."));
                }

                var resultId = input.AssertionId ?? input.EndorsementId ?? input.ClrIdentifier;

                if (resultId == null)
                {
                    return ApiOk(new Verification(input.AssertionId, error: "No target specified"));
                }

                var clrEntity = await _credentialService.GetClrAsync(input.ClrId);

                if (clrEntity == null)
                {
                    return ApiOk(new Verification(resultId, error: "Cannot find CLR.", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message));
                }

                if (clrEntity?.CredentialPackage != null)
                {
                    if (clrEntity.CredentialPackage.Revoked)
                    {
                        return ApiOk(new Verification(resultId, message:"Revoked"
                            , infoBubble: true, bubbleText: clrEntity.CredentialPackage.RevocationReason
                        , revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message));
                    }
                    if (clrEntity.CredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential)
                    {
                        var package = await _credentialService.GetAsync(clrEntity.CredentialPackageId);
                        var revocationResult = await _credentialPackageService.CheckRevocationAsync(package);
                        if (revocationResult.revoked)
                        {
                            return ApiOk(new Verification(clrEntity.Id, message: "Revoked"
                            , infoBubble: true, bubbleText: revocationResult.revocationReason));
                        }
                    }
                    else
                    {
                        _revocationsListModel = await _revocationService.GetRevocationListAsync(_httpContextAccessor.HttpContext.Request, userId ?? clrEntity.CredentialPackage.UserId, input.ClrId);
                    }
                }

                Verification result = null;

                if (!string.IsNullOrEmpty(input.AssertionId))
                {
                    var assertion = await _credentialService.GetAssertionForVerificationAsync(clrEntity.ClrId, input.AssertionId);

                    if (assertion != null && !assertion.IsSigned)
                    {
                        result = await VerifyHostedAssertion(clrEntity, assertion);
                    }

                    else if (assertion != null && assertion.IsSigned)
                    {
                        var publicKey = assertion.Achievement.Issuer.PublicKey;
                        result = await VerifySignature(input.AssertionId, assertion.SignedAssertion, publicKey);
                    }

                    if (result == null)
                    {
                        return ApiOk(new Verification(input.AssertionId,
                            error: "Cannot find assertion.", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message));
                    }

                    if (result.Error == null)
                    {
                        result.RevocationsMessage = _revocationsListModel.Error ?? _revocationsListModel.Message;
                        if (assertion.Recipient == null)
                        {
                            result.Message += "Recipient not populated";
                        }
                        else if (VerifyRecipient(assertion.Recipient, clrEntity.Learner))
                        {
                            result.Message += " - Verified Recipient";
                        }
                        else
                        {
                            result.Error = "Recipient Does Not Match Learner";
                        }
                    }
                }

                else if (!string.IsNullOrEmpty(input.EndorsementId))
                {
                    var endorsement = await _credentialService.GetEndorsementForVerificationAsync(clrEntity.ClrId, input.EndorsementId, input);
                    if (endorsement != null)
                    {
                        if (!endorsement.IsSigned)
                        {
                            result = await VerifyHostedEndorsement(endorsement);
                        }
                        else
                        {

                            var publicKey = endorsement.Issuer.PublicKey;

                            result = await VerifySignature(input.EndorsementId, endorsement.SignedEndorsement, publicKey);
                        }
                    }
                    else
                    {
                        return ApiOk(new Verification(input.AssertionId,
                            error: "Cannot find endorsement.", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message));
                    }
                }
                else if (clrEntity.Verification?.Type == VerificationDType.TypeEnum.HostedEnum)
                {
                    result = await VerifyHostedClr(clrEntity);
                }
                else if (clrEntity.Verification?.Type == VerificationDType.TypeEnum.SignedEnum)
                {
                    if (clrEntity.SignedClr == null)
                    {
                        result = new Verification(input.ClrIdentifier, "Missing signature", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
                    }
                    else
                    {
                        var publicKey = clrEntity.Publisher.PublicKey;
                        if (clrEntity.Id == input.ClrIdentifier)
                        {
                            result = await VerifySignature(input.ClrIdentifier, clrEntity.SignedClr, publicKey);
                        }
                    }
                }

                return ApiOk(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Verifies a Verifiable Credential
        /// POST api/verification/VerifyVC/{id}
        /// </summary>
        /// <returns>verification response</returns>
        [HttpPost("VerifyVC/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> VerifyVC(int id)
        {
            try
            {
                var package = await _credentialService.GetAsync(id);

                if (package?.VerifiableCredential == null)
                    return new ObjectResult(new Verification(id.ToString(), error: "Cannot find the credential."));

                var revocationResult = await _credentialPackageService.CheckRevocationAsync(package);
                if (revocationResult.revoked)
                {
                    return new OkObjectResult(new Verification(id.ToString(), message: "Revoked"
                        , infoBubble: true, bubbleText: revocationResult.revocationReason));
                }

                var verifiableCredential = JsonSerializer.Deserialize<VerifiableCredential>(package.VerifiableCredential.Json);
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
                    return ApiOk(new Verification(id.ToString(), message: "Verified Proof"
                        , infoBubble: true, bubbleText: $"Verification method was used to verify proof: {verificationUrl}."));
                }

                return ApiOk(new Verification(id.ToString(), message:
                            "Proof could not be verified."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        /// <summary>
        /// GET the assertion and process the result
        /// </summary>
        private async Task<Verification> VerifyHostedAssertion(ClrModel clrEntity, AssertionModel assertion)
        {
            if (assertion.Verification == null || assertion.Achievement == null)
            {
                return new Verification(assertion.Id, error: "Not verifiable", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
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

                    if (assertion.Json != newAssertion.ToJson())
                    {
                        //2021-07-20 Can't simply replace the Assertion as it can change Associaed assertion/CLR structure
                        //var clr = JsonSerializer.Deserialize<ClrDType>(clrEntity.Json);
                        //var oldAssertion = clr.Assertions.Single(a => a.Id == assertion.Id);

                        //clr.Assertions.Remove(oldAssertion);
                        //clr.Assertions.Add(newAssertion);
                        //clrEntity.Json = JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true });

                        //await _credentialService.UpdateClrAsync(clrEntity);

                        return new Verification(assertion.Id,
                            "Verified - Assertion is outdated, Credential should be refreshed from source.", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
                    }

                    return new Verification(assertion.Id, "Verified - No changes", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
                }

                return new Verification(assertion.Id, error: response.ReasonPhrase);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing hosted verification");
                return new Verification(assertion.Id, error: e.Message, revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
            }
        }

        private async Task<Verification> VerifyHostedClr(ClrModel clrEntity)
        {
            if (clrEntity.Authorization == null || clrEntity.Verification == null)
            {
                return new Verification(clrEntity.Id, error: "Not verifiable", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
            }

            try
            {
                var newClr = await _clrService.GetClrAsync(this.ModelState, clrEntity.Authorization.Id, clrEntity.Id);

                if (ModelState.IsValid)
                {
                    // Replace clr in the database

                    if (clrEntity.Json != newClr.ToJson())
                    {
                        //2021-07-20 Can't simply replace the CLR as it can change CLR structure
                        //clrEntity.Json = JsonSerializer.Serialize(newClr, new JsonSerializerOptions { IgnoreNullValues = true });

                        //await _credentialService.UpdateClrAsync(clrEntity);

                        return new Verification(clrEntity.Id,
                            "Verified - CLR is outdated, Credential should be refreshed from source.", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
                    }

                    return new Verification(clrEntity.Id, "Verified - No changes", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
                }

                return new Verification(clrEntity.Id, error: "Error getting CLR.", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
            }
            catch (Exception e)
            {
                return new Verification(clrEntity.Id, error: e.Message);
            }
        }

        private async Task<Verification> VerifyHostedEndorsement(EndorsementModel endorsement)
        {
            if (endorsement.Verification == null)
            {
                return new Verification(endorsement.Id, error: "Not verifiable", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
            }

            var client = _factory.CreateClient(ClrHttpClient.Default);
            try
            {
                var response = await client.GetAsync(endorsement.Id);

                // Replace assertion in the database

                return response.IsSuccessStatusCode
                    ? new Verification(endorsement.Id, "Verified", revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message)
                    : new Verification(endorsement.Id, error: response.ReasonPhrase, revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
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
                    return new Verification(id, message: "Verified Signature"
                        , infoBubble: true, bubbleText: $"The issuer&apos;s signing key could not be verified."
                    , revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
                }
                return new Verification(id, message: "Verified Signature"
                    , infoBubble: true, bubbleText: $"The issuer's signing key" +
                    $" is hosted by: {certificate.ToSubjectHtml()}"
                , revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while verifying signature.");
                return new Verification(id, error: e.Message, revocationsMessage: _revocationsListModel.Error ?? _revocationsListModel.Message);
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

        private static bool VerifyRecipient(Data.Models.ClrEntities.IdentityModel recipient, ProfileModel learner)
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

            return recipient.Identity.Equals(learner.Id, StringComparison.InvariantCultureIgnoreCase);
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
    }
}
