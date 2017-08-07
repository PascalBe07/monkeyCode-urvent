using System;
using System.Net.Http;

namespace MonkeyCode.Framework.Portable.Urvent.Helper
{
    public class HttpException : Exception
    {
        private readonly HttpResponseMessage _response;

        public HttpException(HttpResponseMessage response)
        {
            this._response = response;
        }

        public override string Message => "The http request failed:" +
                                          $"{Environment.NewLine}Status Code: {this._response.StatusCode}" +
                                          $"{Environment.NewLine}Message: {this._response.ReasonPhrase}" +
                                          $"{Environment.NewLine}Http Version: {this._response.Version}";
    }
}
