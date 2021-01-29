using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Services.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NJsonSchema;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable StringLiteralTypo

namespace OpenCredentialPublisher.Services.Implementations
{
    public class SchemaService
    {
        public string SchemaLocation = "https://purl.imsglobal.org/spec/clr/v1p0/schema/json/";
        public string BasePath;
        public const string SchemaPath = "/schema/json";
        public const string ClrJsonSchema = "clrv1p0-getclr-200-responsepayload-schemav2p0.json";
        public const string ClrSetJsonSchema = "clrv1p0-getclrs-200-responsepayload-schemav2p0.json";

        public SchemaService(IConfiguration configuration)
        {
            var schemaLocation = configuration["SchemaLocation"];
            if (!string.IsNullOrEmpty(schemaLocation))
            {
                SchemaLocation = schemaLocation;
            }

            BasePath = configuration["BasePath"];
        }

        /// <summary>
        /// Validate JSON against the JSON Schema for a given operation and status code.
        /// Validation errors are added to the page ModelState.
        /// </summary>
        /// <param name="page">The page requesting validation.</param>
        /// <param name="content">The JSON to be validated.</param>
        public async Task ValidateSchemaAsync<T>(PageModel page, string content)
        {
            if (string.IsNullOrEmpty(content)) return;

            var builder = new UriBuilder(page.Request.Scheme, page.Request.Host.Host);
            if (page.Request.Host.Port.HasValue)
            {
                builder.Port = page.Request.Host.Port.Value;
            }
            builder.Path = SchemaPath;
            SchemaLocation = builder.ToString();

            // Re-serialize the content to remove nulls
            string nullRemovedContent;
            T value;
            try
            {
                value = JsonSerializer.Deserialize<T>(content);
                nullRemovedContent = JsonSerializer.Serialize(value,
                    new JsonSerializerOptions { IgnoreNullValues = true });
            }
            catch (Exception e)
            {
                page.ModelState.AddModelError(string.Empty, e.Message);
                return;
            }

            string url;

            if (typeof(T) == typeof(VerifiableCredential)) // bypass schema check for now until the correct VC schema is found
                return;
            else if (typeof(T) == typeof(ClrDType))
                url = $"{SchemaLocation.EnsureTrailingSlash()}{ClrJsonSchema}";
            else if (typeof(T) == typeof(ClrSetDType))
                url = $"{SchemaLocation.EnsureTrailingSlash()}{ClrSetJsonSchema}";
            else
            {
                page.ModelState.AddModelError(string.Empty, $"Unknown type {typeof(T)}");
                return;
            }

            // Validate the response

            try
            {
                var client = new HttpClient();
                Uri schemaUri;
                if (page.Url.IsLocalUrl(url))
                {
                    Uri.TryCreate(page.Request.GetDisplayUrl(), UriKind.Absolute, out var baseUri);
                    Uri.TryCreate(baseUri, $"{BasePath}{url}", out schemaUri);
                }
                else
                {
                    Uri.TryCreate(url, UriKind.Absolute, out schemaUri);
                }
                var schemaJson = await client.GetStringAsync(schemaUri);
                var schema = await JsonSchema.FromJsonAsync(schemaJson);
                var result = schema.Validate(nullRemovedContent);

                if (result.Any())
                {
                    foreach (var error in result)
                    {
                        page.ModelState.AddModelError(string.Empty, error.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                page.ModelState.AddModelError(string.Empty, e.Message);
            }
        }
    }
}
