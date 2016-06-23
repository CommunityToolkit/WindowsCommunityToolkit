// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************
using System;

namespace Microsoft.Windows.Toolkit.Services.Facebook
{
    /// <summary>
    /// Data model for collecting the data to be shared.
    /// </summary>
    public class FacebookShareModel
    {
        /// <summary>
        /// Gets or sets title of the data to be shared.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description of the data to be shared.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets link to the data being shared.
        /// </summary>
        public string Link { get; set; }
    }
}
