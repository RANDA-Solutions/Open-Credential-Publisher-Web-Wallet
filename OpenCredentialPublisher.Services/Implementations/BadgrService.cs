using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.Services.Extensions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class BadgrService
    {
        private readonly WalletDbContext _context;
        private readonly AuthorizationsService _authorizationsService;
        private readonly IHttpClientFactory _factory;
        private readonly LogHttpClientService _logHttpClientService;

        public BadgrService(WalletDbContext context, AuthorizationsService authorizationsService, IHttpClientFactory factory, LogHttpClientService logHttpClientService)
        {
            _context = context;
            _authorizationsService = authorizationsService;
            _factory = factory;
            _logHttpClientService = logHttpClientService;
        }
        /// <summary>
        /// Request an access token using basic auth
        /// </summary>
        public async Task<int> ConvertClrFromBadgeAsync(int id, string userId)
        {
            var badge = await GetBadgeAsync(id);

            var newClr = new ClrDType
            {
                Context = "https://purl.imsglobal.org/spec/clr/v1p0/context",
                Assertions = new List<AssertionDType>(),
                Id = $"urn:uuid:{Guid.NewGuid():D}",
                IssuedOn = DateTime.Now.ToLocalTime(),
                SignedAssertions = new List<string>()
            };
            var emailAddress = (string)null;
            var email = (BadgrUserEmailDType)null;
            var badgrUserResponse = JsonConvert.DeserializeObject<BadgrUserResponse>(badge.RecipientJson);
            var badgrUser = badgrUserResponse.BadgrBadgrUsers.FirstOrDefault();
            if (badgrUser == null)
            {
                badgrUser = new BadgrUserDType();
            }
            if (badgrUser.Emails.Count > 0)
            {
                email = badgrUser.Emails.Where(e => e.Primary).FirstOrDefault();
                if (email == null)
                {
                    email = badgrUser.Emails.Where(e => e.Primary).FirstOrDefault();
                }
                if (email != null)
                {
                    emailAddress = email.Email;
                }
            }

            var tele = badgrUser.Telephone.FirstOrDefault();

            newClr.Learner = new ProfileDType()
            {
                Type = "Profile",
                Id = badgrUser.Id,
                Email = emailAddress,
                Name = badgrUser.FirstName + " " + badgrUser.LastName,
                Telephone = badgrUser.Telephone.FirstOrDefault(),
                SourcedId = badgrUser.Id,
                Url = badgrUser.Url.FirstOrDefault()
            };
            var badgrIssuer = JsonConvert.DeserializeObject<BadgrIssuerDType>(badge.IssuerJson);
            if (badgrIssuer == null)
            {
                badgrIssuer = new BadgrIssuerDType();
            }
            newClr.Publisher = new ProfileDType()
            {
                Type = "Profile",
                Id = badgrIssuer.Id,
                Email = badgrIssuer.Email,
                Name = badgrIssuer.Name,
                SourcedId = badgrIssuer.Id,
                Url = badgrIssuer.Url,
                Description = badgrIssuer.Description
            };

            newClr.Name = badgrIssuer.Name + " Issued Badges";

            var addlProperties = new Dictionary<string, object>();

            var badgeClass = JsonConvert.DeserializeObject<BadgrBadgeClassDType>(badge.BadgeClassJson);

            var achievement = new AchievementDType()
            {
                Id = badgeClass.Id,
                AchievementType = "Badge",
                Description = badgeClass.Description,
                Image = badgeClass.Image,
                Issuer = newClr.Publisher,
                Name = badgeClass.Name,
                Alignments = badgeClass.Alignments,
                Tags = badgeClass.Tags
            };

            var assertion = new AssertionDType()
            {
                Achievement = achievement,
                Id = badge.Id,
                Type =  "Assertion",
                IssuedOn = badge.IssuedOn,
                Recipient = badge.Recipient,
                Narrative = badge.Narrative,
                AdditionalProperties = badge.AdditionalProperties,
                Revoked = badge.Revoked,
                RevocationReason = badge.RevocationReason
            };

            newClr.Assertions.Add(assertion);

            var credentialPackage = new CredentialPackageModel()
            {
                UserId = userId,
                TypeId = PackageTypeEnum.Clr,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Clr = new ClrModel
                {
                    AssertionsCount = newClr.Assertions.Count + newClr.SignedAssertions.Count,
                    Identifier = newClr.Id,
                    IssuedOn = newClr.IssuedOn,
                    LearnerName = newClr.Learner.Name,
                    Name = newClr.Name,
                    PublisherName = newClr.Publisher.Name,
                    RefreshedAt = newClr.IssuedOn,
                    Json = System.Text.Json.JsonSerializer.Serialize(newClr,  new JsonSerializerOptions { IgnoreNullValues = true }),
                }
            };

            await _context.CredentialPackages.AddAsync(credentialPackage);
            await _context.SaveChangesAsync();

            return newClr.ClrKey;
        }

        /// <summary>
        /// Request an access token using basic auth
        /// </summary>
        public async Task<int> CreateClrFromBadgeAsync(int id, string userId)
        {
            var badge = await GetBadgeAsync(id);

            var newClr = new ClrDType
            {
                Context = System.Text.Json.JsonSerializer.Serialize(new List<string> { "https://purl.imsglobal.org/spec/clr/v1p0/context", "https://w3id.org/openbadges/v2" }, new JsonSerializerOptions { IgnoreNullValues = true }),
                Assertions = new List<AssertionDType>(),
                Id = $"urn:uuid:{Guid.NewGuid():D}",
                IssuedOn = DateTime.Now.ToLocalTime(),
                SignedAssertions = new List<string>()
            };
            var emailAddress = (string)null;
            var email = (BadgrUserEmailDType)null;
            var badgrUserResponse = JsonConvert.DeserializeObject<BadgrUserResponse>(badge.RecipientJson);
            var badgrUser = badgrUserResponse.BadgrBadgrUsers.FirstOrDefault();
            if (badgrUser == null)
            {
                badgrUser = new BadgrUserDType();
            }
            if (badgrUser.Emails.Count > 0)
            {
                email = badgrUser.Emails.Where(e => e.Primary).FirstOrDefault();
                if (email == null)
                {
                    email = badgrUser.Emails.Where(e => e.Primary).FirstOrDefault();
                }
                if (email != null)
                {
                    emailAddress = email.Email;
                }
            }

            var tele = badgrUser.Telephone.FirstOrDefault();

            newClr.Learner = new ProfileDType()
            {
                Type = "Profile",
                Id = badgrUser.Id,
                Email = emailAddress,
                Name = badgrUser.FirstName + " " + badgrUser.LastName,
                Telephone = badgrUser.Telephone.FirstOrDefault(),
                SourcedId = badgrUser.Id,
                Url = badgrUser.Url.FirstOrDefault()
            };
            var badgrIssuer = JsonConvert.DeserializeObject<BadgrIssuerDType>(badge.IssuerJson);
            if (badgrIssuer == null)
            {
                badgrIssuer = new BadgrIssuerDType();
            }
            newClr.Publisher = new ProfileDType()
            {
                Type = "Profile",
                Id = badgrIssuer.Id,
                Email = badgrIssuer.Email,
                Name = badgrIssuer.Name,
                SourcedId = badgrIssuer.Id,
                Url = badgrIssuer.Url,
                Description = badgrIssuer.Description
            };

            newClr.Name = badgrIssuer.Name + " Issued Badges";

            var addlProperties = new Dictionary<string, object>();

            addlProperties.Add("obi:BadgeClass", badge.Badgeclass);
            addlProperties.Add("obi:BadgeClassOpenBadgeId", badge.BadgeClassOpenBadgeId);
            addlProperties.Add("obi:BadgrAssertionId", badge.BadgrAssertionId);
            addlProperties.Add("obi:Expires", badge.Expires);
            addlProperties.Add("obi:Image", badge.Image);
            addlProperties.Add("obi:Id", badge.Id);
            addlProperties.Add("obi:InternalIdentifier", badge.InternalIdentifier);
            addlProperties.Add("obi:Issuer", badge.Issuer);
            addlProperties.Add("obi:Narrative", badge.Narrative);
            addlProperties.Add("obi:OpenBadgeId", badge.OpenBadgeId);
            addlProperties.Add("obi:Pending", badge.Pending);
            addlProperties.Add("obi:Recipient", badge.Recipient);
            addlProperties.Add("obi:RevocationReason", badge.RevocationReason);
            addlProperties.Add("obi:Revoked", badge.Revoked);
            addlProperties.Add("obi:SignedAssertion", badge.SignedAssertion);
            addlProperties.Add("obi:Type", badge.Type);
            addlProperties.Add("obi:ValidationStatus", badge.ValidationStatus);


            var assertion = new AssertionDType()
            {
                Id = badge.Id,
                Type = @"[""Assertion"", ""obi:Assertion""]",
                IssuedOn = badge.IssuedOn,
                AdditionalProperties = addlProperties
            };

            newClr.Assertions.Add(assertion);

            var credentialPackage = new CredentialPackageModel()
            {
                UserId = userId,
                TypeId = PackageTypeEnum.Clr,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Clr = new ClrModel
                {
                    AssertionsCount = newClr.Assertions.Count + newClr.SignedAssertions.Count,
                    Identifier = newClr.Id,
                    IssuedOn = newClr.IssuedOn,
                    Json = System.Text.Json.JsonSerializer.Serialize(newClr, new JsonSerializerOptions { IgnoreNullValues = true }),
                    LearnerName = newClr.Learner.Name,
                    Name = newClr.Name,
                    PublisherName = newClr.Publisher.Name,
                    RefreshedAt = newClr.IssuedOn
                }
            };

            await _context.CredentialPackages.AddAsync(credentialPackage);
            await _context.SaveChangesAsync();

            return newClr.ClrKey;
        }
        /// <summary>
        /// Read a Badge from the database
        /// </summary>
        public async Task<BadgrAssertionModel> GetBadgeAsync(int id)
        {
            var assertion = await _context.BadgrAssertions.AsNoTracking()
                .Include(a => a.BadgrBackpack)
                .ThenInclude(bp => bp.CredentialPackage)
                .ThenInclude(cp => cp.Authorization)
                .ThenInclude(a => a.Source)
                .Where(a => a.BadgrAssertionId == id)
                .FirstOrDefaultAsync();           

            return assertion;
        }
        /// <summary>
        /// Request an access token using basic auth
        /// </summary>
        public async Task GetAccessTokenBasic(SourceModel source, string userName, string password, string userId)
        {
            // Generate a code_verifier for PKCE

            var authorization = new AuthorizationModel
            {                
                SourceForeignKey = source.Id,
                UserId = userId
            };
            await _authorizationsService.AddAsync(authorization);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{source.Url}/o/token");

            var parameters = new Dictionary<string, string>
            {
                {OidcConstants.TokenRequest.UserName, userName},
                {OidcConstants.TokenRequest.Password, password}
            };

            request.Content = new FormUrlEncodedContent(parameters);
           
            HttpResponseMessage response;
            try
            {
                var client = _factory.CreateClient(ClrHttpClient.Default);
                response = await client.SendAsync(request);
                await _logHttpClientService.LogAsync(response, parameters); //Note Logging Will asterisk out user name & password

                // Only use the authorization_code and code_verifier once

                authorization.AuthorizationCode = null;
                authorization.CodeVerifier = null;
                await _authorizationsService.UpdateAsync(authorization);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);

                // Remove the source

                await _authorizationsService.DeleteAsync(authorization.Id);

                // Reload the known sources

                throw;
            }

            // Save valid tokens

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);
                authorization.AccessToken = tokenResponse.AccessToken;
                authorization.RefreshToken = tokenResponse.RefreshToken;
                authorization.Scopes = tokenResponse.Scope?.Replace(System.Environment.NewLine, " ").Split(' ').ToList();
                authorization.ValidTo = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
                await _authorizationsService.UpdateAsync(authorization);
            }
            else
            {
                // Remove the source

                await _authorizationsService.DeleteAsync(authorization.Id);

                // Reload the known sources

                throw new Exception($"Response Failure ({response.StatusCode})");
            }
        }

        /// <summary>
        /// Get fresh copies of the Open Badges from the Badgr server.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="id">The authorization id for the resource server.</param>
        public async Task RefreshBackpackAsync(PageModel page, string id)
        {
            if (id == null)
            {
                page.ModelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            var authorization = await _authorizationsService.GetDeepAsync(id);

            if (authorization == null)
            {
                page.ModelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
                return;
            }

            if (authorization.AccessToken == null)
            {
                page.ModelState.AddModelError(string.Empty, "No access token.");
                return;
            }

            if (!await _authorizationsService.RefreshTokenAsync(page, authorization))
            {
                page.ModelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return;
            }

            var serviceUrl = string.Concat(authorization.Source.Url.EnsureTrailingSlash(), "v2/backpack/assertions");

            var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient("default");

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                await SaveBackpackDataAsync(page, content, authorization);
            }
            else
            {
                page.ModelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }
        private async Task SaveBackpackDataAsync(PageModel page, string content, AuthorizationModel authorization)
        {

            //Turn EF Tracking on for untracked authorization
            _context.Attach(authorization);

            var badgrBackpackAssertionsResponse = JsonConvert.DeserializeObject<BadgrBackpackAssertionsResponse>(content);

            var credentialPackage = await _context.CredentialPackages
                    .Include(cp => cp.BadgrBackpack)
                    .ThenInclude(bp => bp.BadgrAssertions)
                    .FirstOrDefaultAsync(cp => cp.UserId == page.User.UserId() && cp.AuthorizationForeignKey == authorization.Id);

            if (credentialPackage == null)
            {
                credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.OpenBadge,
                    AuthorizationForeignKey = authorization.Id,
                    UserId = page.User.UserId(),
                    CreatedAt = DateTime.UtcNow,
                    BadgrBackpack = new BadgrBackpackModel
                    {
                        CredentialPackage = credentialPackage,
                        Identifier = authorization.Id,
                        Json = content,
                        IssuedOn = DateTime.UtcNow,
                        AssertionsCount = badgrBackpackAssertionsResponse.BadgrAssertions.Count,
                        BadgrAssertions = new List<BadgrAssertionModel>()
                    }
                };
                _context.CredentialPackages.Add(credentialPackage);
            }

            // Save each Assertion

            foreach (var currentAssertion in badgrBackpackAssertionsResponse.BadgrAssertions)
            {
                try
                {                
                    var savedAssertion = credentialPackage.BadgrBackpack.BadgrAssertions.SingleOrDefault(a => a.Id == currentAssertion.Id);
                
                    if (await EnhanceAssertionResponseAsync(page, currentAssertion, authorization))
                    {
                        if (savedAssertion != null)
                        {
                            currentAssertion.BadgrBackpackId = savedAssertion.BadgrBackpackId;
                            currentAssertion.BadgrAssertionId = savedAssertion.BadgrAssertionId;
                            _context.Entry(currentAssertion).State = EntityState.Modified;
                        }
                        else
                        {
                            credentialPackage.BadgrBackpack.BadgrAssertions.Add(currentAssertion);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            await _context.SaveChangesAsync();
        }
        private async Task<bool> EnhanceAssertionResponseAsync(PageModel page, BadgrAssertionModel assertion, AuthorizationModel authorization)
        {
            var status = await GetBadgeDetailAsync(assertion.BadgeClassOpenBadgeId, authorization);
            assertion.BadgeClassJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                page.ModelState.AddModelError(string.Empty, status.Description);
                return false;
            }
            status = await GetBadgeDetailAsync(assertion.IssuerOpenBadgeId, authorization);
            assertion.IssuerJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                page.ModelState.AddModelError(string.Empty, status.Description);
                return false;
            }
            status = await GetBadgeDetailAsync(assertion.OpenBadgeId, authorization);
            assertion.BadgeJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                page.ModelState.AddModelError(string.Empty, status.Description);
                return false;
            }
            status = await GetBadgeDetailAsync(string.Concat(authorization.Source.Url.EnsureTrailingSlash(), $"v2/users/self"), authorization);
            assertion.RecipientJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                page.ModelState.AddModelError(string.Empty, status.Description);
                return false;
            }
            return true;

        }
        private async Task<Status> GetBadgeDetailAsync(string url, AuthorizationModel authorization)
        {
            // Get the BadgeClass

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient();

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Validate the response data

                //await _schemaService.ValidateSchemaAsync<ClrDType>(page.Request, content);

                //if (!page.ModelState.IsValid) return null;

                return new Status { Success = true, Description = content};
            }
            else
            {
                return new Status { Success = false, Description = response.ReasonPhrase };
            }
        }
    }
}
