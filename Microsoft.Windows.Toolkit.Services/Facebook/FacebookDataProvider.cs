using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Windows.Toolkit.Services.Core;
using Microsoft.Windows.Toolkit.Services.Exceptions;
using System.Net;

namespace Microsoft.Windows.Toolkit.Services.Facebook
{
    [ConnectedServiceProvider("Facebook", "https://developers.facebook.com/")]
    public class FacebookDataProvider : DataProviderBase<FacebookDataConfig, FacebookSchema>
    {
        private const string BaseUrl = @"https://graph.facebook.com/v2.5";

        private FacebookOAuthTokens _tokens;

        public FacebookDataProvider(FacebookOAuthTokens tokens)
        {
            _tokens = tokens;
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(FacebookDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri($"{BaseUrl}/{config.UserId}/posts?&access_token={_tokens.AppId}|{ _tokens.AppSecret}&fields=id,message,from,created_time,link,full_picture&limit={maxRecords}", UriKind.Absolute)
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
            if (_tokens == null)
            {
                throw new ConfigParameterNullException("Tokens");
            }
            if (string.IsNullOrEmpty(_tokens.AppId))
            {
                throw new OAuthKeysNotPresentException("AppId");
            }
            if (string.IsNullOrEmpty(_tokens.AppSecret))
            {
                throw new OAuthKeysNotPresentException("AppSecret");
            }
        }
    }
}
