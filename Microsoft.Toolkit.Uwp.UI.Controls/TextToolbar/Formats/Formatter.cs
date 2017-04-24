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

    /// <summary>
    /// Manipulates Selected Text into an applied format according to default buttons.
    /// </summary>
    public abstract class Formatter
    {
        public Formatter(TextToolbar model)
        {
            Model = model;
        }

        /// <summary>
        /// Applies Bold
        /// </summary>
        public abstract void FormatBold(ToolbarButton button);

        /// <summary>
        /// Applies Italics
        /// </summary>
        public abstract void FormatItalics(ToolbarButton button);

        /// <summary>
        /// Applies Strikethrough
        /// </summary>
        public abstract void FormatStrikethrough(ToolbarButton button);

        /// <summary>
        /// Applies Link
        /// </summary>
        public abstract void FormatLink(ToolbarButton button, string label, string link);

        /// <summary>
        /// Applies List
        /// </summary>
        public abstract void FormatList(ToolbarButton button);

        /// <summary>
        /// Applies Ordered List
        /// </summary>
        public abstract void FormatOrderedList(ToolbarButton button);

        /// <summary>
        /// Gets the source Toolbar
        /// </summary>
        public TextToolbar Model { get; }

        /// <summary>
        /// Gets the default list of buttons
        /// </summary>
        public abstract ButtonMap DefaultButtons { get; }

        /// <summary>
        /// Gets the formatted version of the Editor's Text
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// Gets shortcut to Carriage Return
        /// </summary>
        internal const string Return = "\r";

        /// <summary>
        /// Gets the current Editor Selection
        /// </summary>
        protected ITextSelection Select { get => Model.Editor.Document.Selection; }

        /// <summary>
        /// Determines the Position of the Selector, if not at a New Line, it will move the Selector to a new line.
        /// </summary>
        public virtual void EnsureAtNewLine()
        {
            int val = Select.StartPosition;
            int counter = 0;
            bool atNewLine = false;

            Model.Editor.Document.GetText(TextGetOptions.NoHidden, out string DocText);
            var lines = DocText.Split(new string[] { Return }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                if (counter == val)
                {
                    atNewLine = true;
                }

                foreach (var c in line)
                {
                    counter++;
                    if (counter >= val)
                    {
                        break;
                    }
                }

                counter++;
            }

            if (!atNewLine)
            {
                bool selectionEmpty = string.IsNullOrWhiteSpace(Select.Text);
                Select.Text = Select.Text.Insert(0, Return);
                Select.StartPosition += 1;

                if (selectionEmpty)
                {
                    Select.EndPosition = Select.StartPosition;
                }
            }
        }
    }
}