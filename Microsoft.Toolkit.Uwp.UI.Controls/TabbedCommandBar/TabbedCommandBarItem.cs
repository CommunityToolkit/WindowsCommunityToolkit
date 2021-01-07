// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A <see cref="CommandBar"/> to be displayed in a <see cref="TabbedCommandBar"/>
    /// </summary>
    [TemplatePart(Name = "PrimaryItemsControl", Type = typeof(ItemsControl))]
    [TemplatePart(Name = "MoreButton", Type = typeof(Button))]
    public class TabbedCommandBarItem : CommandBar
    {
        private ItemsControl _primaryItemsControl;
        private Button _moreButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedCommandBarItem"/> class.
        /// </summary>
        public TabbedCommandBarItem()
        {
            DefaultStyleKey = typeof(TabbedCommandBarItem);
        }

        /// <summary>
        /// Identifies the <see cref="Header"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(TabbedCommandBarItem),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the title of this ribbon tab.
        /// </summary>
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Footer"/> property.
        /// </summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
            nameof(Footer),
            typeof(UIElement),
            typeof(TabbedCommandBarItem),
            new PropertyMetadata(new Border()));

        /// <summary>
        /// Gets or sets the <see cref="UIElement"/> to be displayed in the footer of the tab.
        /// </summary>
        public UIElement Footer
        {
            get => (UIElement)GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsContextual"/> property.
        /// </summary>
        public static readonly DependencyProperty IsContextualProperty = DependencyProperty.Register(
            nameof(IsContextual),
            typeof(bool),
            typeof(TabbedCommandBarItem),
            new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether this tab is contextual.
        /// </summary>
        public bool IsContextual
        {
            get => (bool)GetValue(IsContextualProperty);
            set => SetValue(IsContextualProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OverflowButtonAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty OverflowButtonAlignmentProperty = DependencyProperty.Register(
            nameof(OverflowButtonAlignment),
            typeof(HorizontalAlignment),
            typeof(TabbedCommandBarItem),
            new PropertyMetadata(HorizontalAlignment.Left));

        /// <summary>
        /// Gets or sets a value indicating whether this tab is contextual.
        /// </summary>
        public HorizontalAlignment OverflowButtonAlignment
        {
            get => (HorizontalAlignment)GetValue(OverflowButtonAlignmentProperty);
            set => SetValue(OverflowButtonAlignmentProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _primaryItemsControl = GetTemplateChild("PrimaryItemsControl") as ItemsControl;
            if (_primaryItemsControl != null)
            {
                _primaryItemsControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            }

            _moreButton = GetTemplateChild("MoreButton") as Button;
            if (_moreButton != null)
            {
                _moreButton.HorizontalAlignment = OverflowButtonAlignment;
            }
        }
    }
}