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

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// Strong type for sharing data to LinkedIn.
    /// </summary>
    public partial class LinkedInShareRequest
    {
        /// <summary>
        /// Gets or sets comment property.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets visibility property.
        /// </summary>
        public LinkedInVisibility Visibility { get; set; }

        /// <summary>
        /// Gets or sets content property.
        /// </summary>
        public LinkedInContent Content { get; set; }
    }
}
