// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using CommunityToolkit.WinUI.UI.Controls.TextToolbarButtons;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace CommunityToolkit.WinUI.UI.Controls
{
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
        internal const string LinkElement = "Link";
        internal const string ListElement = "List";
        internal const string OrderedElement = "OrderedList";

        /// <summary>
        /// Initializes a new instance of the <see cref="TextToolbar"/> class.
        /// </summary>
        public TextToolbar()
        {
            DefaultStyleKey = typeof(TextToolbar);
            DefaultStyleResourceUri = new Uri("ms-appx:///CommunityToolkit.WinUI.UI.Controls.Core/Themes/Generic.xaml");

            CustomButtons = new ButtonMap();
            ButtonModifications = new DefaultButtonModificationList();

            if (!InDesignMode)
            {
                KeyEventHandler = new KeyEventHandler(Editor_KeyDown);
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            if (Formatter == null)
            {
                throw new InvalidOperationException("No formatter specified.");
            }
            else
            {
                BuildBar();
            }

            base.OnApplyTemplate();
        }
    }
}