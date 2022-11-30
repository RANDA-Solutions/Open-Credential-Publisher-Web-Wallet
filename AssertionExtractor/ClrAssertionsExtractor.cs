using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Constants;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Utils;
using OpenCredentialPublisher.Services.Implementations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataUtility
{
    public class DataTasks
    {
        private readonly WalletDbContext _context;
        private readonly SchemaService _schemaService;
        private readonly Serilog.ILogger _logger = Log.ForContext<ETLService>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CredentialService _credentialService;
        public DataTasks(SchemaService schemaService, WalletDbContext context, IHttpContextAccessor httpContextAccessor, CredentialService credentialService)
        {
            _httpContextAccessor = httpContextAccessor;
            _schemaService = schemaService;
            _context = context;
            _credentialService = credentialService;

        }
        public async Task PopulateAssertionNamesAsync()
        {
            var assertions = await _credentialService.GetAssertionsAsync();
            foreach (var assertion in assertions)
            {
                ConsoleUtil.ConsoleWrite($"Extracting Assertion: {assertion.Id}...", Configuration.ConsoleColors.InProgress);
                var assertionDType = JsonSerializer.Deserialize<AssertionDType>(assertion.Json);
                assertion.DisplayName = assertionDType.Achievement?.HumanCode == null ? assertionDType.Achievement?.Name : $"{assertionDType.Achievement?.HumanCode}:{assertionDType.Achievement?.Name}";
                await _context.SaveChangesAsync();
            }
        }
        public async Task ExtractAssertionsAsync()
        {
            var pkgIds = await _credentialService.GetPackageUniverseIdsAsync();
            var assertions = await _credentialService.GetAssertionsWithClrAsync();
            var clrIds = assertions.Select(x => x.ClrAssertion.ClrId).ToArray();
            var clrs = await _context.Clrs.AsNoTracking().Where(clr => clrIds.Contains(clr.ClrId)).ToDictionaryAsync(clr => clr.ClrId);
            foreach (var assertion in assertions)
            {
                ConsoleUtil.ConsoleWrite($"Extracting CLR: {assertion.ClrAssertion.ClrId}...", Configuration.ConsoleColors.InProgress);
                var clr = JsonSerializer.Deserialize<ClrDType>(clrs[assertion.ClrAssertion.ClrId].Json);
                var allAssertions = new List<MiniAssertion>();

                if (clr.SignedAssertions != null)
                {
                    allAssertions.AddRange(clr.SignedAssertions.Select(a =>
                    {
                        var assertion = a.DeserializePayload<AssertionDType>();
                        return MiniAssertion.FromDType(assertion, assertion.ToJson(), a);
                    }));
                }

                if (clr.Assertions != null)
                {
                    allAssertions.AddRange(clr.Assertions.ConvertAll(a => MiniAssertion.FromDType(a, a.ToJson())));
                }
                var found = allAssertions.FirstOrDefault(aa => aa.Id == assertion.Id);
                if (found != null)
                {
                    assertion.Json = (found).Json;
                    assertion.IsSigned = found.IsSigned;
                    assertion.SignedAssertion = found.SignedAssertion;
                }
                await _context.SaveChangesAsync();
            }
        }
    }

    internal class MiniAssertion
    {
        internal string Id { get; set; }
        internal string SignedAssertion { get; set; }
        internal bool IsSigned { get; set; }
        internal string Json { get; set; }
        internal static MiniAssertion FromDType(AssertionDType asrt, string json, string signedAssertion = null)
        {
            return new MiniAssertion
            {
                Id = asrt.Id,
                SignedAssertion = signedAssertion,
                Json = json,
                IsSigned = signedAssertion != null
            };
        }
    }
}
