using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ModernHttpClient;
using MonkeyCode.Framework.Portable.Common;
using MonkeyCode.Framework.Portable.Urvent.Services;
using Newtonsoft.Json;

namespace MonkeyCode.Framework.Portable.Urvent.Helper
{
    public abstract class HttpDataService : DataHolder
    {
        private static readonly HttpClient HttpClient = new HttpClient(new NativeMessageHandler()) { MaxResponseContentBufferSize = 0x7fffffffL };
        protected abstract string HttpAddress { get; }
        protected virtual KeyValuePair<string,string> AuthenticationHeader => default(KeyValuePair<string,string>);

        public async Task<TResult> Get<TResult>(string subUri, IJsonReader<TResult> customConverter = null)
        {
            var jsonString = await this.GetString(subUri);

            TResult result = customConverter == null ?  await Task.Factory.StartNew(()=> JsonConvert.DeserializeObject<TResult>(jsonString)) : await Task.Factory.StartNew(() => customConverter.ReadJson(jsonString));

            if (result == null)
            {
                throw new JsonException($"The json string {Environment.NewLine + jsonString + Environment.NewLine} could not be" +
                                        $"converted to the type {typeof(TResult).Name}.");
            }

            return result;
        }

        private async Task<HttpResponseMessage> GetResponse(string subUri)
        {
            var requestUri = this.HttpAddress + subUri;
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            if (!this.AuthenticationHeader.Equals(default(KeyValuePair<string, string>)))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(this.AuthenticationHeader.Key,
                    this.AuthenticationHeader.Value);
            }

            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    //  TODO: Create more specific HttpException classes
                    throw new HttpException(response);
                }
                return response;
            }
            catch (Exception ex)
            {           
                throw ex;
            }
            return null;
        }

        public async Task<string> GetString(string subUri)
        {
            var response = await GetResponse(subUri);  
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<byte[]> GetBytes(string subUri)
        {
            var response = await GetResponse(subUri);
            return await response.Content.ReadAsByteArrayAsync();
        }
    }

    public class HttpResponse<TData>
    {
        public TData Data { get; set; }
        public string Content { get; set; }
        public int StatusCode { get; set; }
    }
}