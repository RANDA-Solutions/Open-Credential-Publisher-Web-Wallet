using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using Randa.Portal.Shared.Models;

namespace OpenCredentialPublisher.ClrWallet.Pages.Connect
{
    public class PatientModel : PageModel
    {
        private readonly ILogger<PatientModel> _logger;
        private readonly CredentialService _credentialService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly IHttpClientFactory _httpClient;
        public PatientModel(CredentialService credentialService, IOptions<SiteSettingsOptions> siteSettings, IHttpClientFactory httpClient, ILogger<PatientModel> logger)
        {
            _credentialService = credentialService;
            _httpClient = httpClient;
            _siteSettings = siteSettings?.Value ?? throw new NullReferenceException("Site settings were not set.");
            _logger = logger;
        }

        public string TestPortalName { get; set; }

        [BindProperty(SupportsGet = true)]
        public PatientGetModel GetModel { get; set; }
        public async Task OnGetAsync()
        {
            TestPortalName = _siteSettings.TestPortalName;
            await Task.Delay(0);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, String.Format(_siteSettings.TestPortalCallbackUrl, GetModel.AccessKey));
                request.Headers.Add("X-CLIENT-KEY", _siteSettings.TestPortalClientKey);
                
                using var client = _httpClient.CreateClient();
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                if (response.Content is object)
                {
                    if (response.Content.Headers.ContentType.MediaType == "application/json")
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var accession = JsonConvert.DeserializeObject<Accession>(content);
                        var clr = new ClrDType();
                        clr.Context = "https://purl.imsglobal.org/spec/clr/v1p0/context/clr_v1p0.jsonld";
                        clr.Id = $"urn:uuid:{Guid.NewGuid()}";
                        clr.Achievements = new List<AchievementDType>();
                        clr.Assertions = new List<AssertionDType>();
                        clr.IssuedOn = accession?.AccessionReleased ?? DateTime.Now;
                        clr.Name = "Test Result";

                        var learner = new ProfileDType
                        {
                            Id = $"urn:uuid:{Guid.NewGuid()}",
                            Address = new AddressDType
                            {
                               AddressRegion = accession.PatientState,
                               PostalCode = accession.PatientZip,
                               StreetAddress = accession.PatientAddress
                            },
                            Name = accession.PatientName,
                            Telephone = accession.PatientPhone
                        };

                        var publisher = new ProfileDType
                        {
                            Id = $"urn:uuid:{Guid.NewGuid()}",
                            Address = new AddressDType
                            {
                                AddressRegion = accession.ClientState,
                                PostalCode = accession.ClientZip,
                                StreetAddress = accession.ClientAddress1
                            },
                            Name = accession.ClientName,
                            Telephone = accession.ClientPhone
                        };

                        var issuer = new ProfileDType
                        {
                            Id = $"urn:uuid:{Guid.NewGuid()}",
                            Name = accession.PhysicianName,
                            SourcedId = accession.PhysicianCode
                        };

                        var COVABResult = new ResultDescriptionDType
                        {
                            Id = $"urn:uuid:{Guid.NewGuid()}",
                            Name = "Result",
                            AllowedValues = new List<string> { "0.0-1.0", "NEGATIVE", "POSITIVE" }
                        };


                        foreach(var test in accession.Tests)
                        {
                            clr.Assertions.Add(new AssertionDType
                            {
                                Id = $"urn:uuid:{Guid.NewGuid()}",
                                Achievement = new AchievementDType
                                {
                                    Id = $"urn:uuid:{Guid.NewGuid()}",
                                    AchievementType = "Certificate",
                                    Description = test.TestOrdered,
                                    Name = test.TestName,
                                    Issuer = issuer,
                                    ResultDescriptions = new List<ResultDescriptionDType>()
                                    {
                                        COVABResult
                                    }
                                },
                                Results = new List<ResultDType>
                                {
                                    new ResultDType
                                    {
                                        Id = $"urn:uuid:{Guid.NewGuid()}",
                                        Value = test.TestResult,
                                        ResultDescription = COVABResult.Id
                                    }
                                },
                                IssuedOn = test.TestCreated
                            });
                        }

                        clr.Learner = learner;
                        clr.Publisher = publisher;
                        var result = await _credentialService.ProcessClr(User.UserId(), clr);
                        if (result.HasError)
                        {
                            foreach (var error in result.ErrorMessages)
                            {
                                ModelState.AddModelError(null, error);
                            }
                            return Page();
                        }
                        if (result.Id.HasValue)
                            return RedirectToPage("/Credentials/Display", new { id = result.Id.Value });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem processing this request.", GetModel);
                ModelState.AddModelError(null, ex.Message);
            }
            return Page();
        }

        public class PatientGetModel
        {
            [FromQuery]
            public string AccessKey { get; set; }
        }
    }
}
