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

using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Toolbar for Editing Text attached to a RichEditBox
    /// </summary>
    public partial class TextToolbar
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditorProperty =
            DependencyProperty.Register(nameof(Editor), typeof(RichEditBox), typeof(TextToolbar), new PropertyMetadata(null, OnEditorChanged));

        // Using a DependencyProperty as the backing store for Formatting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register(nameof(Format), typeof(Format), typeof(TextToolbar), new PropertyMetadata(Format.RichText, OnFormatTypeChanged));

        // Using a DependencyProperty as the backing store for TextFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormatterProperty =
            DependencyProperty.Register(nameof(Formatter), typeof(Formatter), typeof(TextToolbar), new PropertyMetadata(null, OnFormatterChanged));

        // Using a DependencyProperty as the backing store for DefaultButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultButtonsProperty =
            DependencyProperty.Register(nameof(DefaultButtons), typeof(ButtonMap), typeof(TextToolbar), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for CustomButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomButtonsProperty =
            DependencyProperty.Register(nameof(CustomButtons), typeof(ButtonMap), typeof(TextToolbar), new PropertyMetadata(null, OnButtonMapChanged));

        // Using a DependencyProperty as the backing store for RemoveDefaultButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultButtonModificationsProperty =
            DependencyProperty.Register(nameof(ButtonModifications), typeof(DefaultButtonModificationList), typeof(TextToolbar), new PropertyMetadata(null, OnDefaultButtonModificationsChanged));

        // Using a DependencyProperty as the backing store for Labels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelsProperty =
            DependencyProperty.Register(nameof(Labels), typeof(TextToolbarStrings), typeof(TextToolbar), new PropertyMetadata(new TextToolbarStrings()));

        /// <summary>
        /// Gets or sets the RichEditBox to Attach to, this is required for any formatting to work.
        /// </summary>
        public RichEditBox Editor
        {
            get { return (RichEditBox)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        /// <summary>
        /// Gets or sets which formatter to use, and which buttons to provide.
        /// </summary>
        public Format Format
        {
            get { return (Format)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the formatter which is used to format the text from the buttons.
        /// </summary>
        public Formatter Formatter
        {
            get { return (Formatter)GetValue(FormatterProperty); }
            set { SetValue(FormatterProperty, value); }
        }

        /// <summary>
        /// Gets the default buttons for this format
        /// </summary>
        public ButtonMap DefaultButtons
        {
            get { return (ButtonMap)GetValue(DefaultButtonsProperty); }
            private set { SetValue(DefaultButtonsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a list of buttons to add to the Default Button set.
        /// </summary>
        public ButtonMap CustomButtons
        {
            get { return (ButtonMap)GetValue(CustomButtonsProperty); }
            set { SetValue(CustomButtonsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a list of Default buttons to remove from the UI.
        /// </summary>
        public DefaultButtonModificationList ButtonModifications
        {
            get { return (DefaultButtonModificationList)GetValue(DefaultButtonModificationsProperty); }
            set { SetValue(DefaultButtonModificationsProperty, value); }
        }

        public TextToolbarStrings Labels
        {
            get { return (TextToolbarStrings)GetValue(LabelsProperty); }
            set { SetValue(LabelsProperty, value); }
        }

        public VirtualKey LastKeyPress { get; private set; }

        internal static bool InDesignMode
        {
            get { return Windows.ApplicationModel.DesignMode.DesignModeEnabled; }
        }
    }
}