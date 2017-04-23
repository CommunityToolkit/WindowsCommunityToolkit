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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using System;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Toolbar for Editing Text attached to a RichEditBox
    /// </summary>
    [TemplatePart(Name = RootControl, Type = typeof(CommandBar))]
    public partial class TextToolbar : Control
    {
        internal const string RootControl = "Root";
        internal const string BoldElement = "Bold";
        internal const string ItalicsElement = "Italics";
        internal const string StrikethoughElement = "Strikethrough";
        internal const string CodeElement = "Code";
        internal const string QuoteElement = "Quote";
        internal const string LinkElement = "Link";
        internal const string ListElement = "List";
        internal const string OrderedElement = "OrderedList";

        /// <summary>
        /// Gets access to Generic Buttons that activate Formatter Methods
        /// </summary>
        internal CommonButtons CommonButtons { get; }

        public TextToolbar()
        {
            this.DefaultStyleKey = typeof(TextToolbar);
            CommonButtons = new CommonButtons(this);
        }

        protected override void OnApplyTemplate()
        {
            if (Formatter == null)
            {
                switch (Format)
                {
                    case Format.Custom:
                        throw new NullReferenceException("Custom Formatter was null");

                    default:
                        CreateFormatter();
                        break;
                }
            }
            else if (formatterLoadedBeforeTemplate)
            {
                BuildBar();
            }

            base.OnApplyTemplate();
        }
    }
}