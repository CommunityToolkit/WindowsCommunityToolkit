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

using Microsoft.Toolkit.Parsers.Markdown.Render;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render
{
    /// <summary>
    /// The Context of the Current Document Rendering.
    /// </summary>
    public class UIElementCollectionRenderContext : RenderContext
    {
        internal UIElementCollectionRenderContext(UIElementCollection blockUIElementCollection)
        {
            BlockUIElementCollection = blockUIElementCollection;
        }

        internal UIElementCollectionRenderContext(UIElementCollection blockUIElementCollection, IRenderContext context)
            : this(blockUIElementCollection)
        {
            TrimLeadingWhitespace = context.TrimLeadingWhitespace;
            Parent = context.Parent;

            if (context is RenderContext localcontext)
            {
                Foreground = localcontext.Foreground;
                OverrideForeground = localcontext.OverrideForeground;
            }
        }

        /// <summary>
        /// Gets or sets the list to add to.
        /// </summary>
        public UIElementCollection BlockUIElementCollection { get; set; }
    }
}