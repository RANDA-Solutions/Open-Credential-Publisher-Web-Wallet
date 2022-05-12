using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using Microsoft.AspNetCore.Http;
using OpenCredentialPublisher.Data.Models.Enums;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class RevocationService
    {
        private readonly WalletDbContext _context;
        private readonly AuthorizationsService _authorizationsService;
        private readonly CredentialService _credentialService;
        private readonly IHttpClientFactory _factory;
        private readonly SchemaService _schemaService;
        private readonly LogHttpClientService _logHttpClientService;

        public RevocationService(WalletDbContext context, IHttpClientFactory factory, IConfiguration configuration, AuthorizationsService authorizationsService, SchemaService schemaService
            , CredentialService credentialService, LogHttpClientService logHttpClientService)
        {
            _context = context;
            _authorizationsService = authorizationsService;
            _factory = factory;
            _schemaService = schemaService;
            _logHttpClientService = logHttpClientService;
            _credentialService = credentialService;
        }
        public bool IsEntityRevoked(string userId, int sourceId, string entityId)
        {
            return _context.Revocations.AsNoTracking().Where(r => r.SourceId == sourceId && r.UserId == userId && r.RevokedId == entityId).Any();
        }
        public async Task MarkPackageViewModelRevocationsAsync(string userId, CredentialPackageViewModel packageVM)
        {
            var revocations = null as string[];
            if (packageVM.CredentialPackage.Authorization != null)
            {
                revocations = (await GetSavedRevocationListAsync(userId, packageVM.CredentialPackage.Authorization.SourceForeignKey)).Select(e => e.RevokedId).ToArray();
           
                if (revocations.Length == 0)
                {
                    return;
                }
            }
            else
            {
                return;
            }
            ///TODO For repackaged Clr's, do we need to check each one individually for revocation ?
            switch (packageVM.CredentialPackage.TypeId)
            {
                case PackageTypeEnum.Clr:
                    if (revocations.Contains(packageVM.ClrVM.Clr.Id))
                    {
                        packageVM.ClrVM.Clr.IsRevoked = true;
                        await MarkClrViewModelRevocationsAsync(userId, packageVM.ClrVM, revocations);
                    }
                    break;
                case PackageTypeEnum.ClrSet:
                    foreach (var clrVM in packageVM.ClrSetVM.ClrVMs)
                    {
                        await MarkClrViewModelRevocationsAsync(userId, clrVM, revocations);
                    }
                    break;
                case PackageTypeEnum.VerifiableCredential:
                    if (packageVM.VerifiableCredentialVM.ClrSetVMs != null)
                    {
                        foreach (var clrSetVM in packageVM.VerifiableCredentialVM.ClrSetVMs)
                        {
                            if (clrSetVM.ClrVMs != null)
                            {
                                foreach (var clrVM in clrSetVM.ClrVMs)
                                {
                                    await MarkClrViewModelRevocationsAsync(userId, clrVM, revocations);
                                }
                            }
                        }
                    }
                    if (packageVM.VerifiableCredentialVM.ClrVMs != null)
                    {
                        foreach (var clrVM in packageVM.VerifiableCredentialVM.ClrVMs)
                        {
                            await MarkClrViewModelRevocationsAsync(userId, clrVM, revocations);
                        }
                    }
                    break;
                default:
                    break;
            }

        }
        public async Task MarkPackageRevocationsAsync(string userId, PackageVM packageVM)
        {
            //TODO nG Revocation
            return;
        }
        public async Task MarkAssertionDTypeRevocationsAsync(string userId, int sourceId, AssertionDType assertion, string[] revocationListIds = null)
        {
            var revocations = revocationListIds;
            if (revocations == null)
            {
                revocations = (await GetSavedRevocationListAsync(userId, sourceId)).Select(e => e.RevokedId).ToArray();
            }
            if (revocations.Length == 0)
            {
                return;
            }
            if (revocations.Contains(assertion.Id))
            {
                assertion.Revoked = true;
            }
            if (assertion.Endorsements != null)
            {
                foreach (var endorsement in assertion.Endorsements)
                {
                    if (revocations.Contains(endorsement.Id))
                    {
                        endorsement.Revoked = true;
                    }
                }
            }
            if (assertion.Achievement != null)
            {
                if (assertion.Achievement.Endorsements != null)
                {
                    foreach (var endorsement in assertion.Achievement.Endorsements)
                        {
                        if (revocations.Contains(endorsement.Id))
                        {
                            endorsement.Revoked = true;
                        }
                    }
                }
                var issuer = assertion.Achievement.Issuer;

                if (issuer.Endorsements != null)
                {
                    foreach (var endorsement in issuer.Endorsements)
                    {
                        if (revocations.Contains(endorsement.Id))
                        {
                            endorsement.Revoked = true;
                        }
                    }
                }
            }
        }
        //public async Task MarkPackageRevocationsAsync(string userId, CredentialPackageModel package)
        //{
        //    var revocations = null as string[];
        //    if (package.Authorization != null)
        //    {
        //        revocations = (await GetSavedRevocationListAsync(userId, package.Authorization.SourceForeignKey)).Select(e => e.RevokedId).ToArray();

        //        if (revocations.Length == 0)
        //        {
        //            return;
        //        }
        //    }
        //    switch (package.TypeId)
        //    {
        //        case PackageTypeEnum.Clr:
        //            if (revocations.Contains(package.Clr.Identifier))
        //            {
        //                package.Clr.IsRevoked = true;
        //                await MarkClrModelRevocationsAsync(userId, package.Clr, revocations);
        //            }
        //            break;
        //        case PackageTypeEnum.ClrSet:
        //            foreach (var clr in package.ClrSet.Clrs)
        //            {
        //                await MarkClrModelRevocationsAsync(userId, clr, revocations);
        //            }
        //            break;
        //        case PackageTypeEnum.VerifiableCredential:
        //            if (package.VerifiableCredential.ClrSets != null)
        //            {
        //                foreach (var clrSet in package.VerifiableCredential.ClrSets)
        //                {
        //                    if (clrSet.Clrs != null)
        //                    {
        //                        foreach (var clr in clrSet.Clrs)
        //                        {
        //                            await MarkClrModelRevocationsAsync(userId, clr, revocations);
        //                        }
        //                    }
        //                }
        //            }
        //            if (package.VerifiableCredential.Clrs != null)
        //            {
        //                foreach(var clr in package.VerifiableCredential.Clrs)
        //                {
        //                    await MarkClrModelRevocationsAsync(userId, clr, revocations);
        //                }
        //            }
        //            break;
        //        default:
        //            break;
        //    }
              
        //}
        //public async Task MarkClrModelRevocationsAsync(string userId, ClrModel clr, string[] revocationListIds = null)
        //{
        //    var revocations = revocationListIds;
        //    if (revocations == null)
        //    {
        //        revocations = (await GetSavedRevocationListAsync(userId, clr.Authorization.SourceForeignKey)).Select(e => e.RevokedId).ToArray();
        //    }
        //    if (revocations.Length == 0)
        //    {
        //        return;
        //    }

        //    if (revocations.Contains(clr.Identifier))
        //    {
        //        clr.IsRevoked = true;
        //    }

        //    foreach (var assertion in clr.AllAssertions)
        //    {
        //        MarkAssertionModelRevocations(userId, assertion, revocations);
        //    }
        //}
        public async Task MarkClrViewModelRevocationsAsync(string userId, ClrViewModel clrVM, string[] revocationListIds = null)
        {
            var revocations = revocationListIds;

            if (clrVM.RawClrDType?.Publisher?.RevocationList == null)
                return;

            if (clrVM.Clr?.Authorization?.SourceForeignKey == null)
                return;

            if (revocations == null)
            {
                revocations = (await GetSavedRevocationListAsync(userId, clrVM.Clr.Authorization.SourceForeignKey)).Select(e => e.RevokedId).ToArray();
            }
            if (revocations.Length == 0)
            {
                return;
            }
            
            if (revocations.Contains(clrVM.Clr.Id))
            {
                clrVM.Clr.IsRevoked = true;
            }

            foreach (var assertion in clrVM.AllAssertions)
            {
                MarkAssertionModelRevocations(userId, assertion, revocations);
            }
        }
        public void MarkAssertionModelRevocations(string userId, AssertionViewModel assertionVM, string[] revocationListIds = null)
        {
            var revocations = revocationListIds;
            if (revocations == null)
            {
                return;
            }
            if (revocations.Length == 0)
            {
                return;
            }
            if (revocations.Contains(assertionVM.Assertion.Id))
            {
                assertionVM.Assertion.Revoked = true;
            }
            if (assertionVM.AllEndorsements != null)
            {
                foreach (var endorsement in assertionVM.AllEndorsements)
                {
                    if (revocations.Contains(endorsement.Id))
                    {
                        endorsement.Revoked = true;
                    }
                }
            }
            if (assertionVM.AchievementVM != null)
            {

                if (assertionVM.AchievementVM.AllEndorsements != null)
                {
                    foreach (var endorsement in assertionVM.AchievementVM.AllEndorsements)
                        {
                        if (revocations.Contains(endorsement.Id))
                        {
                            endorsement.Revoked = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Get Revocation list from the resource server.
        /// </summary>
        /// <param name="id">The authorization id for the resource server.</param>
        /// <param name="rlId">The revocationlist id (uri) the resource server.</param>
        public async Task<RevocationListModel> GetRevocationListAsync(HttpRequest pageRequest, string userId, int? clrEntityId)
        {
            if (clrEntityId == null)
            {
                return new RevocationListModel(null, error: "Missing Clr id.");
            }
            var clrEntity = await _credentialService.GetClrAsync(clrEntityId.Value);

            if (clrEntity == null)
            {
                return new RevocationListModel(null, error: "Cannot find CLR.");
            }
            var clr = new ClrDType();
            if (String.IsNullOrEmpty(clrEntity.SignedClr))
            {
                clr = JsonSerializer.Deserialize<ClrDType>(clrEntity.Json);
            }
            else
            {
                clr = clrEntity.SignedClr.DeserializePayload<ClrDType>();
            }

            var rlId = clr?.Publisher?.RevocationList;
            if (rlId == null)
            {
                return new RevocationListModel(null, error: "Missing revocation list id.");
            }

            var authorization = clrEntity.Authorization;

            if (authorization != null)
            {
                authorization = await _authorizationsService.GetDeepAsync(authorization.Id);

                if (authorization.AccessToken == null)
                {
                    return new RevocationListModel(null, error: "No access token.");
                }

                if (!await _authorizationsService.RefreshTokenAsync(modelState: null, authorization))
                {
                    return new RevocationListModel(null, error: "The access token has expired and cannot be refreshed.");
                }
            }

            // Get the RevocationList
            var revocations = new List<RevocationModel>();
            var request = new HttpRequestMessage(HttpMethod.Get, rlId);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);

            var client = _factory.CreateClient("default");

            var response = await client.SendAsync(request);

            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Validate the response data

                await _schemaService.ValidateSchemaAsync<RevocationListDType>(pageRequest, content);

                var revocationListDType = JsonSerializer.Deserialize<RevocationListDType>(content);

                List<RevocationModel> priorRevocations;
                if (authorization != null)
                {
                    priorRevocations = await GetSavedRevocationListAsync(userId, authorization.SourceForeignKey);
                    revocations = await SaveRevocationListAsync(userId, authorization.SourceForeignKey, revocationListDType);
                }
                else
                {
                    priorRevocations = await GetSavedRevocationListAsync(userId, rlId);
                    revocations = await SaveRevocationListAsync(userId, rlId, revocationListDType);
                }

                if (string.Join("|", revocations.Select(r => r.RevokedId).ToArray()) != string.Join("|", priorRevocations.Select(r => r.RevokedId).ToArray()))
                {
                    return new RevocationListModel(revocations, "Revocation list updated, refresh page to see any changes.");
                }
                else
                {
                    return new RevocationListModel(revocations, "Revocation list refreshed, no changes.");
                }
            }
            return new RevocationListModel(null, error: "Could not retrieve revocation list.");
        }
        
        public async Task<List<RevocationModel>> GetSavedRevocationListAsync(string userId, int sourceId)
        {
            return await _context.Revocations.AsNoTracking().Where(r => r.SourceId == sourceId && r.UserId == userId).OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<List<RevocationModel>> GetSavedRevocationListAsync(string userId, string revocationListId)
        {
            return await _context.Revocations.AsNoTracking().Where(r => r.RevocationListId == revocationListId && r.UserId == userId).OrderBy(r => r.Id).ToListAsync();
        }

        private async Task<List<RevocationModel>> SaveRevocationListAsync(string userId, int sourceId, RevocationListDType revocationListDType)
        {
            var revokes = _context.Revocations.Where(r => r.SourceId == sourceId && r.UserId == userId);
            await revokes.ForEachAsync(r => r.Delete());
            await _context.SaveChangesAsync();

            if (revocationListDType.RevokedAssertions.Count > 0)
            {
                foreach (var id in revocationListDType.RevokedAssertions)
                {
                    var revoked = new RevocationModel()
                    {
                        CreatedAt = DateTime.UtcNow,
                        IssuerId = revocationListDType.Issuer,
                        SourceId = sourceId,
                        RevokedId = id,
                        UserId = userId
                    };
                    _context.Revocations.Add(revoked);
                }
                await _context.SaveChangesAsync();

                return await GetSavedRevocationListAsync(userId, sourceId);
            }

            return new List<RevocationModel>();
        }

        private async Task<List<RevocationModel>> SaveRevocationListAsync(string userId, string revocationListId, RevocationListDType revocationListDType)
        {
            var revokes = _context.Revocations.Where(r => r.RevocationListId == revocationListId && r.UserId == userId);
            await revokes.ForEachAsync(r => r.Delete());
            await _context.SaveChangesAsync();

            if (revocationListDType.RevokedAssertions.Count > 0)
            {
                foreach (var id in revocationListDType.RevokedAssertions)
                {
                    var revoked = new RevocationModel()
                    {
                        CreatedAt = DateTime.UtcNow,
                        IssuerId = revocationListDType.Issuer,
                        RevocationListId = revocationListId,
                        RevokedId = id,
                        UserId = userId
                    };
                    _context.Revocations.Add(revoked);
                }
                await _context.SaveChangesAsync();

                return await GetSavedRevocationListAsync(userId, revocationListId);
            }

            return new List<RevocationModel>();
        }
    }
}
