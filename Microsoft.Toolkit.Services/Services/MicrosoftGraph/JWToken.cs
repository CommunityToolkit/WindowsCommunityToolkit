// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JwToken"/> class.
    /// </summary>
    public class JwToken
    {
        /// <summary>
        /// Gets or sets the type of the token issued.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the lifetime in seconds of the access token.
        /// </summary>
        ///
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the scope of the access token.
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the access token issued by the authorization server.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token, which can be used to obtain new access tokens using the same authorization grant.
        /// </summary>
        [JsonProperty("refresh-token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the a security token that contains Claims about the authentication of an End-User by an Authorization Server.
        /// </summary>
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}
