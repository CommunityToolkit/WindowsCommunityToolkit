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

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Strongly typed object for presenting picture data returned from service provider.
    /// </summary>
    public class FacebookPicture
    {
        /// <summary>
        /// Gets or sets a value indicating whether the picture is a silhouette or not.
        /// </summary>
        public bool Is_Silhouette { get; set; }

        /// <summary>
        /// Gets or sets an url to the picture.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the ID of the picture.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the url of the page with the picture.
        /// </summary>
        public string Link { get; set; }
    }
}
