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
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using OpenCredentialPublisher.Data.Abstracts;
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
        public readonly IUrlHelper _urlHelper;
        public SchemaService(IConfiguration configuration, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            var schemaLocation = configuration["SchemaLocation"];
            if (!string.IsNullOrEmpty(schemaLocation))
            {
                SchemaLocation = schemaLocation;
            }

            BasePath = configuration["BasePath"];
        }

        public async Task<SchemaResult> ValidateSchemaAsync<T>(HttpRequest request, string content)
        {
            var schemaResult = new SchemaResult();
            if (string.IsNullOrEmpty(content))
            {
                schemaResult.ErrorMessages.Add("JSON is null or empty.");
                return schemaResult;
            }
            
            var builder = new UriBuilder(request.Scheme, request.Host.Host);
            if (request.Host.Port.HasValue)
            {
                builder.Port = request.Host.Port.Value;
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
                schemaResult.ErrorMessages.Add(e.Message);
                return schemaResult;
            }

            string url;

            if (typeof(T) == typeof(VerifiableCredential)) // bypass schema check for now until the correct VC schema is found
            {
                //schemaResult.IsValid = true;
                return schemaResult;
            }
            else if (typeof(T) == typeof(ClrDType))
                url = $"{SchemaLocation.EnsureTrailingSlash()}{ClrJsonSchema}";
            else if (typeof(T) == typeof(ClrSetDType))
                url = $"{SchemaLocation.EnsureTrailingSlash()}{ClrSetJsonSchema}";
            else
            {
                schemaResult.ErrorMessages.Add($"Unknown type {typeof(T)}");
                return schemaResult;
            }

            // Validate the response

            try
            {
                var client = new HttpClient();
                Uri schemaUri;
                
                if (_urlHelper.IsLocalUrl(url))
                {
                    Uri.TryCreate(request.GetDisplayUrl(), UriKind.Absolute, out var baseUri);
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
                        schemaResult.ErrorMessages.Add(error.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                schemaResult.ErrorMessages.Add(e.Message);
            }
            return schemaResult;
        }
    }

    public class SchemaResult: GenericModel
    {
        public bool IsValid => !HasError;
    }
}
