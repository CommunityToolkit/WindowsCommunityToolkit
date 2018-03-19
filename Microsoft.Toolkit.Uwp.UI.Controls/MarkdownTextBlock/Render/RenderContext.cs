﻿// ******************************************************************
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

using Microsoft.Toolkit.Parsers.Markdown.Render;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render
{
    /// <summary>
    /// The Context of the Current Position
    /// </summary>
    public abstract class RenderContext : IRenderContext
    {
        /// <summary>
        /// Gets or sets the Foreground of the Current Context.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <inheritdoc/>
        public bool TrimLeadingWhitespace { get; set; }

        /// <inheritdoc/>
        public object Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to override the Foreground Property.
        /// </summary>
        public bool OverrideForeground { get; set; }

        /// <inheritdoc/>
        public IRenderContext Clone()
        {
            return (IRenderContext)MemberwiseClone();
        }
    }
}