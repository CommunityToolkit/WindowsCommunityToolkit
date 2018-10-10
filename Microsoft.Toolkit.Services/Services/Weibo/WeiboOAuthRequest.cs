// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.OAuth;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// OAuth request.
    /// </summary>
    internal class WeiboOAuthRequest
    {
        private static HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboOAuthRequest"/> class.
        /// </summary>
        public WeiboOAuthRequest()
        {
            if (_client == null)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip;
                _client = new HttpClient(handler);
            }
        }

        /// <summary>
        /// HTTP Get request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <returns>String result.</returns>
        public async Task<string> ExecuteGetAsync(Uri requestUri, WeiboOAuthTokens tokens)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                UriBuilder requestUriBuilder = new UriBuilder(request.RequestUri);
                if (requestUriBuilder.Query.StartsWith("?"))
                {
                    requestUriBuilder.Query = requestUriBuilder.Query.Substring(1) + "&access_token=" + OAuthEncoder.UrlEncode(tokens.AccessToken);
                }
                else
                {
                    requestUriBuilder.Query = requestUriBuilder.Query + "?access_token=" + OAuthEncoder.UrlEncode(tokens.AccessToken);
                }

                request.RequestUri = requestUriBuilder.Uri;

                using (HttpResponseMessage response = await _client.SendAsync(request).ConfigureAwait(false))
                {
                    response.ThrowIfNotValid();
                    return ProcessError(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
            }
        }

        /// <summary>
        /// HTTP Post request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <param name="status">Status text.</param>
        /// <returns>String result.</returns>
        public async Task<WeiboStatus> ExecutePostAsync(Uri requestUri, WeiboOAuthTokens tokens, string status)
        {
            var contentDict = new Dictionary<string, string>();
            contentDict.Add("status", status);

            using (var formUrlEncodedContent = new FormUrlEncodedContent(contentDict))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    UriBuilder requestUriBuilder = new UriBuilder(request.RequestUri);
                    if (requestUriBuilder.Query.StartsWith("?"))
                    {
                        requestUriBuilder.Query = requestUriBuilder.Query.Substring(1) + "&access_token=" + OAuthEncoder.UrlEncode(tokens.AccessToken);
                    }
                    else
                    {
                        requestUriBuilder.Query = requestUriBuilder.Query + "access_token=" + OAuthEncoder.UrlEncode(tokens.AccessToken);
                    }

                    request.RequestUri = requestUriBuilder.Uri;

                    request.Content = formUrlEncodedContent;

                    using (var response = await _client.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return JsonConvert.DeserializeObject<WeiboStatus>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                        }
                        else
                        {
                            response.ThrowIfNotValid();
                            ProcessError(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                            return null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// HTTP Post request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <param name="status">Status text.</param>
        /// <param name="content">Data to post to server.</param>
        /// <returns>String result.</returns>
        public async Task<WeiboStatus> ExecutePostMultipartAsync(Uri requestUri, WeiboOAuthTokens tokens, string status, byte[] content)
        {
            try
            {
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    using (var stringContent = new StringContent(status))
                    {
                        multipartFormDataContent.Add(stringContent, "status");
                        using (var byteContent = new ByteArrayContent(content))
                        {
                            // Somehow Weibo's backend requires a Filename field to work
                            byteContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = "attachment", Name = "pic" };
                            multipartFormDataContent.Add(byteContent, "pic");

                            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                            {
                                UriBuilder requestUriBuilder = new UriBuilder(request.RequestUri);
                                if (requestUriBuilder.Query.StartsWith("?"))
                                {
                                    requestUriBuilder.Query = requestUriBuilder.Query.Substring(1) + "&access_token=" + OAuthEncoder.UrlEncode(tokens.AccessToken);
                                }
                                else
                                {
                                    requestUriBuilder.Query = requestUriBuilder.Query + "access_token=" + OAuthEncoder.UrlEncode(tokens.AccessToken);
                                }

                                request.RequestUri = requestUriBuilder.Uri;

                                request.Content = multipartFormDataContent;

                                using (var response = await _client.SendAsync(request).ConfigureAwait(false))
                                {
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        return JsonConvert.DeserializeObject<WeiboStatus>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                                    }
                                    else
                                    {
                                        response.ThrowIfNotValid();
                                        ProcessError(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // known issue
                // http://stackoverflow.com/questions/39109060/httpmultipartformdatacontent-dispose-throws-objectdisposedexception
            }

            return null;
        }

        private string ProcessError(string content)
        {
            if (content.StartsWith("{\"error\":"))
            {
                WeiboError error = JsonConvert.DeserializeObject<WeiboError>(content, new JsonSerializerSettings()
                {
                    Error = (sender, args) => throw new JsonException("Invalid Weibo error response!", args.ErrorContext.Error)
                });

                throw new WeiboException { Error = error };
            }

            return content;
        }
    }
}
