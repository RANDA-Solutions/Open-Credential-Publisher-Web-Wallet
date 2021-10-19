using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Dtos.Search;
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
    public class SearchController : ApiController<SearchController>
    {
        private readonly SearchService _searchService;
        private readonly SiteSettingsOptions _siteSettings;
        
        public SearchController(SearchService searchService, ILogger<SearchController> logger,
            IOptions<SiteSettingsOptions> siteSettings) : base(logger)
        {
            _searchService = searchService;
            _siteSettings = siteSettings?.Value ?? throw new NullReferenceException("Site settings were not set.");
        }

        /// <summary>
        /// List Search Terms
        /// GET api/search/list
        /// </summary>
        /// <returns>list of search terms response</returns>
        [HttpGet("List/{word:minlength(1)}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ListAsync(string word)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ApiOk(new WordList());
                }
                var words = await _searchService.ListAsync(word);
                return ApiOk(words);
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
        [HttpGet("{word:minlength(1)}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> SearchAsync(string word)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ApiOk(new WordList());
                }
                var credentials = await _searchService.SearchAsync(word);
                var searchResponse = new SearchResponse
                {
                    SearchTerm = word,
                    Records = credentials.Count,
                    Credentials = credentials.Select(c => new Credential
                    {
                        Id = c.Id,
                        CredentialDescription = c.CredentialDescription,
                        CredentialName = c.CredentialName,
                        CredentialNarrative = c.CredentialNarrative,
                        CredentialType = c.CredentialType
                    }).ToList()
                };

                return ApiOk(searchResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
