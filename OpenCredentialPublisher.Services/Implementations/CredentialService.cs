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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenCredentialPublisher.Data.Abstracts;
using System.Linq;

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

        public async Task<List<CredentialPackageModel>> GetAllAsync(string userId)
        {
            var packages = await GetAllDeep(userId)
                .ToListAsync();

            return packages;
        }
        public IQueryable<CredentialPackageModel> GetAllDeep(string userId)
        {
            var packages = _context.CredentialPackages
                .Include(cp => cp.Authorization)
                .ThenInclude(auth => auth.Source)
                .AsNoTracking()
                .Include(p => p.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(cp => cp.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(clra => clra.Source)
                .Include(cp => cp.ClrSet)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.Clr)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.BadgrBackpack)
                .ThenInclude(bp => bp.BadgrAssertions)
                .Where(package => package.UserId == userId)
                .OrderBy(x => x.CreatedAt);

            return packages;
        }
        public async Task<List<ClrModel>> GetAllClrsAsync(string userId)
        {
            return await _context.Clrs
                .Include(clr => clr.VerifiableCredential)
                .ThenInclude(vc => vc.CredentialPackage)
                .Include(clr => clr.CredentialPackage)
                .Include(clr => clr.ClrSet)
                .ThenInclude(cs => cs.CredentialPackage)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => (c.CredentialPackage != null && c.CredentialPackage.UserId == userId) || (c.ClrSet != null && c.ClrSet.CredentialPackage.UserId == userId)).ToListAsync();
        }
        public async Task<List<ClrModel>> GetAllClrsAsync(string userId, int[] ids)
        {
            return await _context.Clrs
                .Include(clr => clr.CredentialPackage)
                .Include(clr => clr.ClrSet)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => ids.Contains(c.Id) && ((c.CredentialPackage != null && c.CredentialPackage.UserId == userId) || (c.ClrSet != null && c.ClrSet.CredentialPackage.UserId == userId)))
                .ToListAsync();
        }

        public IQueryable<ClrModel> GetAllClrs(string userId)
        {
            return _context.Clrs
               .Include(clr => clr.CredentialPackage)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => c.CredentialPackage.UserId == userId);
        }

        public async Task<List<LinkModel>> GetAllLinksAsync(string userId)
        {
            var result = await _context.Links
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return result;
        }
        public async Task<CredentialPackageModel> GetWithSourcesAsync(string userId, int id)
        {
            var credentialPackage = await _context.CredentialPackages
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(cp => cp.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(clra => clra.Source)
                .Include(p => p.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.ClrSet)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.Clr)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.Id == id);
            return credentialPackage;
        }

        public async Task<CredentialPackageModel> GetAsync(string userId, int id)
        {
            var credentialPackage = await _context.CredentialPackages
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(cp => cp.Clrs)
                .Include(p => p.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(sets => sets.Clrs)
                .Include(cp => cp.ClrSet)
                .ThenInclude(sets => sets.Clrs)
                .Include(cp => cp.Clr)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.Id == id);
            return credentialPackage;
        }

        public async Task<bool> PackageExistsAsync(string userId, int id)
        {
            return await _context.CredentialPackages.AnyAsync(cp => cp.UserId == userId && cp.Id == id);
        }


        public async Task<CredentialPackageModel> GetAsync(int id)
        {
            var credentialPackage = await _context.CredentialPackages.AsNoTracking()
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(clrSets => clrSets.Clrs)
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(vc => vc.Clrs)
                .Include(cp => cp.Clr)
                .FirstOrDefaultAsync(cp => cp.Id == id);
            return credentialPackage;
        }
        public async Task<CredentialPackageModel> UpdateAsync(CredentialPackageModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<CertificateModel> AddCertificateAsync(CertificateModel input)
        {
            _context.Certificates.Add(input);
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<CertificateModel> UpdateCertificateAsync(CertificateModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<CertificateModel> GetCertificateAsync(string id)
        {
            return await _context.Certificates.FindAsync(id);
        }
        public async Task<ClrModel> GetClrAsync(string userId, int id)
        {
            var clr = await _context.Clrs
                .Include(c => c.CredentialPackage)
                .FirstOrDefaultAsync(c => c.Id == id && c.CredentialPackage.UserId == userId);

            return clr;
        }
        public async Task<ClrModel> GetClrAsync(int id)
        {
            return await _context.Clrs
                    .AsNoTracking()
                    .Include(x => x.CredentialPackage)
                    .Include(x => x.Authorization)
                    .ThenInclude(x => x.Source)
                    .SingleAsync(x => x.Id == id);
        }

        public async Task DeleteClrAsync(int id)
        {
            var item =  await _context.Clrs
                    .AsNoTracking()
                    .Include(x => x.Authorization)
                    .ThenInclude(x => x.Source)
                    .SingleAsync(x => x.Id == id);

            _context.Clrs.Remove(item);

            await _context.SaveChangesAsync();
        }
        public async Task<ClrModel> UpdateClrAsync(ClrModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        
        public async Task CreateClrFromSelectedAsync(string userId, string name, int[] ids) 
        {
            var clrs = await GetAllClrsAsync(userId, ids);

            if (!clrs.Any())
            {
                throw new Exception("There weren't any CLRs matching your selections.");
            }

            var newClr = new ClrDType
            {
                Assertions = new List<AssertionDType>(),
                Id = $"urn:uuid:{Guid.NewGuid():D}",
                IssuedOn = DateTime.Now.ToLocalTime(),
                Name = name,
                SignedAssertions = new List<string>()
            };

            foreach (var clrModel in clrs)
            {
                    var clr = JsonSerializer.Deserialize<ClrDType>(clrModel.Json);

                    if (!string.IsNullOrEmpty(clrModel.SignedClr))
                    {
                        clr = clrModel.SignedClr.DeserializePayload<ClrDType>();
                    }

                    // Assume all the CLRs are for the same person

                    newClr.Learner = clr.Learner;
                    newClr.Publisher = clr.Learner;

                    foreach (var assertion in clr.Assertions ?? new List<AssertionDType>())
                    {
                        newClr.Assertions.Add(assertion);
                    }

                    foreach (var signedAssertion in clr.SignedAssertions ?? new List<string>())
                    {
                        newClr.SignedAssertions.Add(signedAssertion);
                    }
            }

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
                    Json = JsonSerializer.Serialize(newClr),
                    LearnerName = newClr.Learner.Name,
                    Name = newClr.Name,
                    PublisherName = newClr.Publisher.Name,
                    RefreshedAt = newClr.IssuedOn
                }
            };

            await _context.CredentialPackages.AddAsync(credentialPackage);
            await _context.SaveChangesAsync();

        }

        public async Task<CredentialResponse> ProcessJson(PageModel page, string json, AuthorizationModel authorization)
        {
            if (json.Contains("https://www.w3.org/2018/credentials/v1"))
            {
                await _schemaService.ValidateSchemaAsync<VerifiableCredential>(page.Request, json);
                if (page.ModelState.IsValid)
                    return await ProcessVerifiableCredential(page.User.UserId(), json, authorization);
            }
            else if (json.Contains("CLRSet"))
            {
                await _schemaService.ValidateSchemaAsync<ClrSetDType>(page.Request, json);
                if (page.ModelState.IsValid)
                    return await ProcessClrSet(page.User.UserId(), json, authorization);
            }
            else
            {
                await _schemaService.ValidateSchemaAsync<ClrDType>(page.Request, json);
                if (page.ModelState.IsValid)
                    return await ProcessClr(page.User.UserId(), json, authorization);
            }
            return null;
        }

        public async Task<CredentialResponse> ProcessJson(ControllerBase controller, string userId, string json, AuthorizationModel authorization)
        {
            if (json.Contains("https://www.w3.org/2018/credentials/v1"))
            {
                var schemaResult = await _schemaService.ValidateSchemaAsync<VerifiableCredential>(controller.Request, json);
                if (schemaResult.IsValid)
                    return await ProcessVerifiableCredential(userId, json, authorization);
            }
            else if (json.Contains("CLRSet"))
            {
                var schemaResult = await _schemaService.ValidateSchemaAsync<ClrSetDType>(controller.Request, json);
                if (schemaResult.IsValid)
                    return await ProcessClrSet(userId, json, authorization);
            }
            else
            {
                var schemaResult = await _schemaService.ValidateSchemaAsync<ClrDType>(controller.Request, json);
                if (schemaResult.IsValid)
                    return await ProcessClr(userId, json, authorization);
            }
            return null;
        }

        private async Task<CredentialResponse> ProcessVerifiableCredential(string userId, string json, AuthorizationModel authorization)
        {
            var credentialResponse = new CredentialResponse();
            try
            {
                VerifiableCredential vc;
                try
                {
                    vc = System.Text.Json.JsonSerializer.Deserialize<VerifiableCredential>(json);
                }
                catch (Exception e)
                {
                    credentialResponse.ErrorMessages.Add(e.Message);
                    return credentialResponse;
                }

                if (await _context.VerifiableCredentials.AnyAsync(v => v.Identifier == vc.Id))
                {
                    credentialResponse.ErrorMessages.Add("That credential has already been loaded.");
                    return credentialResponse;
                }

                var credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.VerifiableCredential,
                    AuthorizationForeignKey = authorization == null ? null : authorization.Id,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    UserId = userId,
                    
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
                credentialResponse.Id = credentialPackage.Id;
            }
            catch (Exception ex)
            {
                credentialResponse.ErrorMessages.Add(ex.Message);
            }
            return credentialResponse;
        }

        private async Task<CredentialResponse> ProcessClrSet(string userId, string json, AuthorizationModel authorization)
        {
            var credentialResponse = new CredentialResponse();
            try
            {
                ClrSetDType clrSet;
                try
                {
                    clrSet = System.Text.Json.JsonSerializer.Deserialize<ClrSetDType>(json);
                }
                catch (Exception e)
                {
                    credentialResponse.ErrorMessages.Add(e.Message);
                    return credentialResponse;
                }

                var credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.ClrSet,
                    AuthorizationForeignKey = authorization == null ? null : authorization.Id,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    UserId = userId,
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
                credentialResponse.Id = credentialPackage.Id;
            }
            catch (Exception ex)
            {
                credentialResponse.ErrorMessages.Add(ex.Message);
            }
            return credentialResponse;
        }

        private async Task<CredentialResponse> ProcessClr(string userId, string json, AuthorizationModel authorization)
        {
            var credentialResponse = new CredentialResponse();
            try
            {
                ClrDType clr;

                try
                {
                    clr = System.Text.Json.JsonSerializer.Deserialize<ClrDType>(json);
                }
                catch (Exception e)
                {
                    credentialResponse.ErrorMessages.Add(e.Message);
                    return credentialResponse;
                }

                var credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.Clr,
                    AuthorizationForeignKey = authorization == null ? null : authorization.Id,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    UserId = userId
                };
                await _context.CredentialPackages.AddAsync(credentialPackage);
                await _context.SaveChangesAsync();

                credentialPackage.Clr = CreateClr(credentialPackage, clr, json);
                await _context.SaveChangesAsync();
                credentialResponse.Id = credentialPackage.Id;
            }
            catch (Exception ex)
            {
                credentialResponse.ErrorMessages.Add(ex.Message);
            }
            return credentialResponse;
        }

        public async Task<CredentialResponse> ProcessClr(string userId, ClrDType clr)
        {
            var credentialResponse = new CredentialResponse();
            try
            {
                var credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.Clr,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    UserId = userId
                };
                await _context.CredentialPackages.AddAsync(credentialPackage);
                await _context.SaveChangesAsync();

                var clrEntity = new ClrModel
                {
                    AssertionsCount = (clr.Assertions?.Count ?? 0)
                                + (clr.SignedAssertions?.Count ?? 0),
                    IssuedOn = clr.IssuedOn,
                    Identifier = clr.Id,
                    Json = System.Text.Json.JsonSerializer.Serialize(clr),
                    LearnerName = clr.Learner?.Name,
                    Name = clr.Name,
                    PublisherName = clr.Publisher?.Name,
                    RefreshedAt = DateTime.Now,
                    CredentialPackage = credentialPackage
                };

                credentialPackage.Clr = clrEntity;
                await _context.SaveChangesAsync();
                credentialResponse.Id = credentialPackage.Id;
            }
            catch (Exception ex)
            {
                credentialResponse.ErrorMessages.Add(ex.Message);
            }
            return credentialResponse;
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

    public class CredentialResponse: GenericModel
    {
        public int? Id { get; set; }
    }
}
