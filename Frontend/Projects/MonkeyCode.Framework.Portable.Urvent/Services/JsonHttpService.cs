using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using ModernHttpClient;
using MonkeyCode.Framework.Portable.Common;
using Newtonsoft.Json;

namespace MonkeyCode.Framework.Portable.Urvent.BusinessLogic
{
    public abstract class JsonHttpService : DataHolder
    {
        private static readonly HttpClient HttpClient = new HttpClient(new NativeMessageHandler()) { MaxResponseContentBufferSize = 0x7fffffffL };
        protected abstract string HttpAddress { get; }
        protected virtual KeyValuePair<string,string> AuthenticationHeader => default(KeyValuePair<string,string>);

        public TResult Get<TResult>(string subUri, IJsonReader<TResult> customConverter = null)
        {
            var jsonString = this.GetString(subUri);

            var result = customConverter == null ? JsonConvert.DeserializeObject<TResult>(jsonString) : 
                customConverter.ReadJson(jsonString);

            if (result == null)
            {
                throw new JsonException($"The json string {Environment.NewLine + jsonString + Environment.NewLine} could not be" +
                                        $"converted to the type {typeof(TResult).Name}.");
            }

            return result;
        }

        public string GetString(string subUri)
        {
            var requestUri = this.HttpAddress + subUri;
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            if (!this.AuthenticationHeader.Equals(default(KeyValuePair<string,string>)))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(this.AuthenticationHeader.Key,
                    this.AuthenticationHeader.Value);
            }


            var response = HttpClient.SendAsync(request).Result;
            if (!response.IsSuccessStatusCode)
            {
                //  TODO: Create more specific HttpException classes
                throw new HttpException(response);
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;
            return responseContent;
        }
    }
}