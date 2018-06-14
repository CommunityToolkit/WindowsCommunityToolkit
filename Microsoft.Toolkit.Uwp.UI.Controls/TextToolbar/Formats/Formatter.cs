// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats
{
    /// <summary>
    /// Manipulates Selected Text into an applied format according to default buttons.
    /// </summary>
    public abstract class Formatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Formatter"/> class.
        /// </summary>
        /// <param name="model">The <see cref="TextToolbar"/>where Formatter is used</param>
        public Formatter(TextToolbar model)
        {
            Model = model;
            Model.EditorChanged += Model_EditorChanged;
        }

        /// <summary>
        /// Called when text editor has changed
        /// </summary>
        /// <param name="sender"><see cref="TextToolbar"/> invoking the event</param>
        /// <param name="e"><see cref="EditorChangedArgs"/></param>
        protected virtual void Model_EditorChanged(object sender, EditorChangedArgs e)
        {
            if (e.Old != null)
            {
                e.Old.SelectionChanged -= Editor_SelectionChanged;
            }

            if (e.New != null)
            {
                e.New.SelectionChanged += Editor_SelectionChanged;
            }
        }

        /// <summary>
        /// Called for Changes to Selction (Requires unhook if switching RichEditBox).
        /// </summary>
        /// <param name="sender">Editor</param>
        /// <param name="e">Args</param>
        private void Editor_SelectionChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            OnSelectionChanged();
        }

        /// <summary>
        /// Decrements the selected position until it is at the start of the current line.
        /// </summary>
        public virtual void EnsureAtStartOfCurrentLine()
        {
            while (!Selected.Text.StartsWith(NewLineChars))
            {
                Selected.StartPosition -= 1;
                if (Selected.StartPosition == 0)
                {
                    break;
                }
            }

            if (Selected.StartPosition != 0)
            {
                Selected.StartPosition += NewLineChars.Length;
            }
        }

        /// <summary>
        /// Determines the Position of the Selector, if not at a New Line, it will move the Selector to a new line.
        /// </summary>
        public virtual void EnsureAtNewLine()
        {
            int val = Selected.StartPosition;
            int counter = 0;
            bool atNewLine = false;

            string docText = string.Empty;
            Model.Editor.Document.GetText(TextGetOptions.NoHidden, out docText);
            var lines = docText.Split(new string[] { Return }, StringSplitOptions.None);

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
                bool selectionEmpty = string.IsNullOrWhiteSpace(Selected.Text);
                Selected.Text = Selected.Text.Insert(0, Return);
                Selected.StartPosition += 1;

                if (selectionEmpty)
                {
                    Selected.EndPosition = Selected.StartPosition;
                }
            }
        }

        /// <summary>
        /// Gets an array of the Lines of Text in the Editor.
        /// </summary>
        /// <returns>Text Array</returns>
        public virtual string[] GetLines()
        {
            Model.Editor.Document.GetText(TextGetOptions.None, out string doc);
            var lines = doc.Split(new string[] { NewLineChars }, StringSplitOptions.None);
            return lines;
        }

        /// <summary>
        /// Gets the line from the index provided (Skips last Carriage Return)
        /// </summary>
        /// <returns>Last line text</returns>
        public virtual string GetLine(int index)
        {
            return GetLines()[index];
        }

        /// <summary>
        /// Gets the last line (Skips last Carriage Return)
        /// </summary>
        /// <returns>Last line text</returns>
        public virtual string GetLastLine()
        {
            var lines = GetLines();
            return lines[lines.Length - 2];
        }

        /// <summary>
        /// Called after the Selected Text changes.
        /// </summary>
        public virtual void OnSelectionChanged()
        {
        }

        /// <summary>
        /// Gets the source Toolbar
        /// </summary>
        public TextToolbar Model { get; }

        /// <summary>
        /// Gets or sets a map of the Actions taken when a button is pressed. Required for Common Button actions (Unless you override both Activation and ShiftActivation)
        /// </summary>
        public ButtonActions ButtonActions { get; protected set; }

        /// <summary>
        /// Gets the default list of buttons
        /// </summary>
        public abstract ButtonMap DefaultButtons { get; }

        /// <summary>
        /// Gets the formatted version of the Editor's Text
        /// </summary>
        public virtual string Text
        {
            get
            {
                string currentvalue = string.Empty;
                Model.Editor.Document.GetText(TextGetOptions.FormatRtf, out currentvalue);
                return currentvalue;
            }
        }

        /// <summary>
        /// Gets the Characters used to indicate a New Line
        /// </summary>
        public virtual string NewLineChars
        {
            get
            {
                return "\r\n";
            }
        }

        /// <summary>
        /// Gets the current Editor Selection
        /// </summary>
        public ITextSelection Selected
        {
            get { return Model.Editor.Document.Selection; }
        }

        /// <summary>
        /// Shortcut to Carriage Return
        /// </summary>
        protected const string Return = "\r";
    }
}