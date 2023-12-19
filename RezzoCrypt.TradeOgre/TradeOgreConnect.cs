/*
 *   Copyright (c) 2023 Alexey Vinogradov
 *   All rights reserved.

 *   Permission is hereby granted, free of charge, to any person obtaining a copy
 *   of this software and associated documentation files (the "Software"), to deal
 *   in the Software without restriction, including without limitation the rights
 *   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *   copies of the Software, and to permit persons to whom the Software is
 *   furnished to do so, subject to the following conditions:
 
 *   The above copyright notice and this permission notice shall be included in all
 *   copies or substantial portions of the Software.
 
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *   SOFTWARE.
 */

using Flurl;
using Flurl.Http;
using RezzoCrypt.TradeOgre.APIs;

namespace RezzoCrypt.TradeOgre
{
    public class TradeOgreConnect
    {
        #region Common variables and methods

        /// <summary>
        /// Flurl client
        /// </summary>
        private readonly FlurlClient client;
        /// <summary>
        /// Base api path
        /// </summary>
        private readonly string baseApiPath = "https://tradeogre.com/api/v1";
        /// <summary>
        /// Api key
        /// </summary>
        private string ApiKey { get; }
        /// <summary>
        /// Api secred
        /// </summary>
        private string ApiSecret { get; }
        /// <summary>
        /// Data load method
        /// </summary>
        internal enum Method { Get, Post }
        /// <summary>
        /// Recieve data from api
        /// </summary>
        /// <typeparam name="T">Result object type</typeparam>
        /// <param name="url">Method url</param>
        /// <param name="data">Parameters</param>
        /// <param name="method">Method data recieve</param>
        /// <param name="secure">Is private method</param>
        /// <exception cref="Exception">Data revieve exceptions</exception>
        internal T GetUrlResult<T>(string url, object? data = null, Method method = Method.Get, bool secure = false)
                    where T : class
        {
            var currentRequest = client.Request(url);

            if (secure)
                currentRequest.WithBasicAuth(ApiKey, ApiSecret);

            try
            {
                var responseResult = method switch
                {
                    Method.Post => data != null
                        ? currentRequest.PostUrlEncodedAsync(data).Result
                        : currentRequest.PostAsync().Result,
                    _ => currentRequest.SetQueryParams(data).GetAsync().Result,
                };

                return typeof(T) == typeof(string)
                    ? (T)(responseResult.GetStringAsync().Result as object)
                    : responseResult.GetJsonAsync<T>().Result;
            }
            catch (Exception ex)
            {
                if (ex is FlurlHttpException fhttpex)
                {
                    string serverErrorMessage = string.Empty;
                    try
                    {
                        serverErrorMessage = $"url: {url}, data: {currentRequest.Url.Query}, error: {fhttpex.Call.Response.GetStringAsync().Result}";
                    }
                    catch
                    {
                        // Could not extract server side error , just continue with original exception.
                    }

                    if (serverErrorMessage != null)
                    {
                        throw new Exception(serverErrorMessage);
                    }
                }
                throw;
            }
        }

        #endregion

        public TradeOgreConnect(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            client = new FlurlClient(baseApiPath);
            client.Settings.JsonSerializer = new NewtonsoftJsonSerializer();
        }

        /// <summary>
        /// Account info
        /// </summary>
        public AccountInfo Account => new(this);

        /// <summary>
        /// Exchange operations
        /// </summary>
        public AccountExchange Exchange => new(this);

        /// <summary>
        /// Global market data
        /// </summary>
        public MarketData Market => new(this);
    }
}