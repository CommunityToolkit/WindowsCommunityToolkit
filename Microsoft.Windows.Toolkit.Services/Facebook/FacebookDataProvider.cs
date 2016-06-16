// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Windows.Toolkit.Services.Core;
using Microsoft.Windows.Toolkit.Services.Exceptions;

namespace Microsoft.Windows.Toolkit.Services.Facebook
{
    [ConnectedServiceProvider("Facebook", "https://developers.facebook.com/")]
    public class FacebookDataProvider : DataProviderBase<FacebookDataConfig, FacebookSchema>
    {
        private const string BaseUrl = @"https://graph.facebook.com/v2.5";

        private FacebookOAuthTokens tokens;

        public FacebookDataProvider(FacebookOAuthTokens tokens)
        {
            this.tokens = tokens;
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FacebookDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri($"{BaseUrl}/{config.UserId}/posts?&access_token={tokens.AppId}|{ tokens.AppSecret}&fields=id,message,from,created_time,link,full_picture&limit={maxRecords}", UriKind.Absolute)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new OAuthKeysRevokedException($"Request failed with status code {(int)HttpStatusCode.BadRequest} and reason '{result.Result}'");
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        protected override IParser<FacebookSchema> GetDefaultParserInternal(FacebookDataConfig config)
        {
            return new FacebookParser();
        }

        protected override void ValidateConfig(FacebookDataConfig config)
        {
            if (config.UserId == null)
            {
                throw new ConfigParameterNullException("UserId");
            }

            if (tokens == null)
            {
                throw new ConfigParameterNullException("Tokens");
            }

            if (string.IsNullOrEmpty(tokens.AppId))
            {
                throw new OAuthKeysNotPresentException("AppId");
            }

            if (string.IsNullOrEmpty(tokens.AppSecret))
            {
                throw new OAuthKeysNotPresentException("AppSecret");
            }
        }
    }
}
