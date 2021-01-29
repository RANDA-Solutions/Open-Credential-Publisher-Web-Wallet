using OpenCredentialPublisher.ClrLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using System.Text.Json;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialService
    {
        private readonly WalletDbContext _context;
        private readonly SchemaService _schemaService;

        public CredentialService(WalletDbContext context, SchemaService schemaService)
        {
            _context = context;
            _schemaService = schemaService;
        }

        public async Task<int?> ProcessJson(PageModel page, string json)
        {
            if (json.Contains("https://www.w3.org/2018/credentials/v1"))
            {
                await _schemaService.ValidateSchemaAsync<VerifiableCredential>(page, json);
                if (page.ModelState.IsValid)
                    return await ProcessVerifiableCredential(page, json);
            }
            else if (json.Contains("CLRSet"))
            {
                await _schemaService.ValidateSchemaAsync<ClrSetDType>(page, json);
                if (page.ModelState.IsValid)
                    return await ProcessClrSet(page, json);
            }
            else
            {
                await _schemaService.ValidateSchemaAsync<ClrDType>(page, json);
                if (page.ModelState.IsValid)
                    return await ProcessClr(page, json);
            }
            return null;
        }

        private async Task<int?> ProcessVerifiableCredential(PageModel page, string json)
        {
            try
            {
                VerifiableCredential vc;
                try
                {
                    vc = System.Text.Json.JsonSerializer.Deserialize<VerifiableCredential>(json);
                }
                catch (Exception e)
                {
                    page.ModelState.AddModelError(string.Empty, e.Message);
                    return null;
                }

                if (await _context.VerifiableCredentials.AnyAsync(v => v.Identifier == vc.Id))
                {
                    return null;
                }

                var credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.VerifiableCredential,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    UserId = page.User.UserId(),
                    
                    VerifiableCredential = new VerifiableCredentialModel
                    {
                        CredentialsCount = vc.CredentialSubjects.Count,
                        Identifier = vc.Id,
                        IssuedOn = vc.IssuanceDate,
                        Issuer = vc.Issuer,
                        Json = json,
                        Clrs = new List<ClrModel>(),
                        ClrSets = new List<ClrSetModel>()
                    }
                };

                foreach (var cred in vc.CredentialSubjects)
                {
                    if (cred is ClrSetSubject clrSet)
                    {
                        var clrSetJson = System.Text.Json.JsonSerializer.Serialize(clrSet, new JsonSerializerOptions { IgnoreNullValues = true });
                        var clrSetModel = new ClrSetModel
                        {
                            Json = clrSetJson,
                            Identifier = clrSet.Id,
                            ClrsCount = (clrSet.Clrs?.Count ?? 0) + (clrSet.SignedClrs?.Count ?? 0),
                            Clrs = new List<ClrModel>()
                        };

                        foreach (var clr in clrSet.Clrs)
                        {
                            clrSetModel.Clrs.Add(CreateClr(credentialPackage, clr, System.Text.Json.JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true })));
                        }

                        foreach (var signedClr in clrSet.SignedClrs)
                        {
                            var clr = signedClr.DeserializePayload<ClrDType>();
                            clrSetModel.Clrs.Add(CreateClr(credentialPackage, clr, System.Text.Json.JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true }), signedClr));
                        }
                        credentialPackage.VerifiableCredential.ClrSets.Add(clrSetModel);
                    }

                    if (cred is ClrSubject clrSubject)
                    {
                        var clrModel = CreateClr(credentialPackage, clrSubject, System.Text.Json.JsonSerializer.Serialize(clrSubject, new JsonSerializerOptions { IgnoreNullValues = true }));
                        credentialPackage.VerifiableCredential.Clrs.Add(clrModel);
                    }
                }

                await _context.CredentialPackages.AddAsync(credentialPackage);
                await _context.SaveChangesAsync();
                return credentialPackage.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<int?> ProcessClrSet(PageModel page, string json)
        {
            try
            {
                ClrSetDType clrSet;
                try
                {
                    clrSet = System.Text.Json.JsonSerializer.Deserialize<ClrSetDType>(json);
                }
                catch (Exception e)
                {
                    page.ModelState.AddModelError(string.Empty, e.Message);
                    return null;
                }

                var credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.ClrSet,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    UserId = page.User.UserId(),
                    ClrSet = new ClrSetModel
                    {
                        Json = json,
                        Identifier = clrSet.Id,
                        ClrsCount = (clrSet.Clrs?.Count ?? 0) + (clrSet.SignedClrs?.Count ?? 0),
                        Clrs = new List<ClrModel>()
                    }
                };

                foreach (var clr in clrSet.Clrs)
                {
                    var clrModel = CreateClr(credentialPackage, clr, System.Text.Json.JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true }));
                    credentialPackage.ClrSet.Clrs.Add(clrModel);
                }

                foreach (var signedClr in clrSet.SignedClrs)
                {
                    var clr = signedClr.DeserializePayload<ClrDType>();
                    credentialPackage.ClrSet.Clrs.Add(CreateClr(credentialPackage, clr, System.Text.Json.JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true }), signedClr));
                }

                await _context.CredentialPackages.AddAsync(credentialPackage);
                await _context.SaveChangesAsync();
                return credentialPackage.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<int?> ProcessClr(PageModel page, string json)
        {
            try
            {
                ClrDType clr;

                try
                {
                    clr = System.Text.Json.JsonSerializer.Deserialize<ClrDType>(json);
                }
                catch (Exception e)
                {
                    page.ModelState.AddModelError(string.Empty, e.Message);
                    return null;
                }

                var credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.Clr,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    UserId = page.User.UserId()
                };
                await _context.CredentialPackages.AddAsync(credentialPackage);
                await _context.SaveChangesAsync();

                credentialPackage.Clr = CreateClr(credentialPackage, clr, json);
                await _context.SaveChangesAsync();
                return credentialPackage.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private ClrModel CreateClr(CredentialPackageModel credentialPackage, ClrDType clr, String json, String signedClr = "")
        {
            var clrEntity = new ClrModel
            {
                AssertionsCount = (clr.Assertions?.Count ?? 0)
                                + (clr.SignedAssertions?.Count ?? 0),
                IssuedOn = clr.IssuedOn,
                Identifier = clr.Id,
                Json = json,
                SignedClr = signedClr,
                LearnerName = clr.Learner?.Name,
                Name = clr.Name,
                PublisherName = clr.Publisher?.Name,
                RefreshedAt = DateTime.Now,
                CredentialPackage = credentialPackage
            };

            return clrEntity;
        }
    }
}
