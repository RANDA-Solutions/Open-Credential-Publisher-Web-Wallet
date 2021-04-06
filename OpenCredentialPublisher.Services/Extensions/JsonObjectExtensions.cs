using System;
using System.Collections.Generic;
using System.Json;
using System.Text;
using VeritySDK.Utils;

namespace OpenCredentialPublisher.Services.Extensions
{
    public static class JsonObjectExtensions
    {
        public static string GetThreadId(this JsonObject jsonObject)
        {
            return jsonObject.GetValue("~thread")["thid"];
        }

        public static string GetRelationshipDID(this JsonObject jsonObject)
        {
            return jsonObject.GetValue("myDID");
        }
        public static string GetDID(this JsonObject jsonObject)
        {
            return jsonObject.GetValue("did");
        }

        public static string GetVerKey(this JsonObject jsonObject)
        {
            return jsonObject.GetValue("verKey");
        }

        public static string GetInviteUrl(this JsonObject jsonObject)
        {
            return jsonObject.GetValue("inviteURL");
        }

        public static string GetSchemaId(this JsonObject jsonObject)
        {
            return jsonObject.GetValue("schemaId");
        }

        public static string GetCredentialDefinitionId(this JsonObject jsonObject)
        {
            return jsonObject.GetValue("credDefId");
        }

        /// <summary>
        /// Get and return JSON value by key without \\"
        /// </summary>
        /// <param name="json">This object</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string GetString(this JsonObject json, string key)
        {
            JsonValue value = null;
            try
            {
                json.TryGetValue(key, out value);
                return value.ToString().Trim('"');
            }
            catch
            {
                return null;
            }
        }

        public static string AsString(this JsonObject json)
        {
            try
            {
                return json.ToString().Trim('"');
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get and return JSON value by key without \\"
        /// </summary>
        /// <param name="json">This object</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string GetString(this JsonValue json, string key)
        {
            JsonValue value = null;
            try
            {
                if (json.ContainsKey(key))
                {
                    value = json[key];
                    return value.ToString().Trim('"');
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
