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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats
{
    using System;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Windows.UI.Text;

    public class RichTextFormatter : Formatter
    {
        public RichTextFormatter(TextToolbar model)
            : base(model)
        {
        }

        public override string Text
        {
            get
            {
                Model.Editor.Document.GetText(TextGetOptions.FormatRtf, out string currentvalue);
                return currentvalue;
            }
        }

        public override ButtonMap DefaultButtons
        {
            get
            {
                return new ButtonMap
                {
                    new ToolbarButton { Content = "WIP" }
                };
            }
        }

        public override void FormatBold()
        {
            throw new NotImplementedException();
        }

        public override void FormatItalics()
        {
            throw new NotImplementedException();
        }

        public override void FormatStrikethrough()
        {
            throw new NotImplementedException();
        }

        public override void FormatLink(string label, string link)
        {
            throw new NotImplementedException();
        }

        public override void FormatList()
        {
            throw new NotImplementedException();
        }

        public override void FormatOrderedList()
        {
            throw new NotImplementedException();
        }
    }
}