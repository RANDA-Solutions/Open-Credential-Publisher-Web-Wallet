using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infotekka.ND.IdRampAPI.EndPoints
{
    static class EndPointBase
    {
        #region Statics
        static EndPointBase() {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public static async Task<T> ConnectJson<T>(string EndPoint, object RequestVM, IdRampApiOptions options) /*where T : new()*/ => await ConnectJson<T>(EndPoint, JsonSerializer.Serialize(RequestVM, new JsonSerializerOptions() { IgnoreNullValues = true }), options);
        public static async Task<T> ConnectJson<T>(string EndPoint, string RequestJson, IdRampApiOptions options) /*where T : new()*/ {
            var c = new HttpClient() {
                BaseAddress = new Uri(options.ApiBaseUri)
            };

            if (!String.IsNullOrEmpty(options.BearerToken)) {
                c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.BearerToken);
            }
            c.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = !String.IsNullOrEmpty(RequestJson) ? new StringContent(RequestJson, Encoding.UTF8, "application/json") : null;

            var result = await c.PostAsync(EndPoint, content);

            if (result.IsSuccessStatusCode) {
                string data = await result.Content.ReadAsStringAsync();
                T vm = JsonSerializer.Deserialize<T>(data);
                return vm;
            } else {
                throw new Exception($"Unable to connect. {result.StatusCode}: {result.ReasonPhrase}");
            }
        }

        public static async Task<T> ConnectForm<T>(string EndPoint, Dictionary<string, string> FormData, IdRampApiOptions options) /*where T : new()*/ {
            var c = new HttpClient() {
                BaseAddress = new Uri(options.ApiBaseUri)
            };

            if (!String.IsNullOrEmpty(options.BearerToken)) {
                c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.BearerToken);
            }

            var content = new FormUrlEncodedContent(FormData);

            var result = await c.PostAsync(EndPoint, content);

            if (result.IsSuccessStatusCode) {
                string data = await result.Content.ReadAsStringAsync();
                T vm = JsonSerializer.Deserialize<T>(data);
                return vm;
            } else {
                throw new Exception($"Unable to connect. {result.StatusCode}: {result.ReasonPhrase}");
            }
        }

        public static async Task<T> ConnectGet<T>(string EndPoint, string RouteValues, IdRampApiOptions options) /*where T : new()*/ {
            var c = new HttpClient() {
                BaseAddress = new Uri(options.ApiBaseUri)
            };

            if (!String.IsNullOrEmpty(options.BearerToken)) {
                c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.BearerToken);
            }

            var result = await c.GetAsync($"{EndPoint}{RouteValues}");

            if (result.IsSuccessStatusCode) {
                string data = await result.Content.ReadAsStringAsync();
                T vm = JsonSerializer.Deserialize<T>(data);
                return vm;
            } else {
                throw new Exception($"Unable to connect. {result.StatusCode}: {result.ReasonPhrase}");
            }
        }

        public static async Task<T> ConnectDelete<T>(string EndPoint, string RouteValues, IdRampApiOptions options) /*where T : new()*/ {
            var c = new HttpClient() {
                BaseAddress = new Uri(options.ApiBaseUri)
            };

            if (!String.IsNullOrEmpty(options.BearerToken)) {
                c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.BearerToken);
            }

            var result = await c.DeleteAsync($"{EndPoint}{RouteValues}");

            if (result.IsSuccessStatusCode) {
                string data = await result.Content.ReadAsStringAsync();
                T vm = JsonSerializer.Deserialize<T>(data);
                return vm;
            } else {
                throw new Exception($"Unable to connect. {result.StatusCode}: {result.ReasonPhrase}");
            }
        }

        //public static async Task<T> ConnectPut<T>(string EndPoint, string RouteValues, string BearerToken = null) /*where T : new()*/ {
        //    var c = new HttpClient() {
        //        BaseAddress = Runtime.ApiBaseUri
        //    };

        //    if (!String.IsNullOrEmpty(BearerToken)) {
        //        c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
        //    }

        //    var result = await c.PutAsync($"{EndPoint}{RouteValues}");

        //    if (result.IsSuccessStatusCode) {
        //        string data = await result.Content.ReadAsStringAsync();
        //        T vm = JsonConvert.DeserializeObject<T>(data);
        //        return vm;
        //    } else {
        //        throw new Exception($"Unable to connect. {result.StatusCode}: {result.ReasonPhrase}");
        //    }
        //}
        #endregion
    }
}
