// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A basic ribbon control that houses <see cref="TabbedCommandBarItem"/>s
    /// </summary>
    [ContentProperty(Name = nameof(MenuItems))]
    [TemplatePart(Name = "PART_RibbonNavigationView", Type = typeof(NavigationView))]
    [TemplatePart(Name = "PART_RibbonContent", Type = typeof(ContentControl))]
    [TemplatePart(Name = "PART_TabChangedStoryboard", Type = typeof(Storyboard))]
    public class TabbedCommandBar : NavigationView
    {
        private NavigationView _ribbonNavigationView = null;
        private ContentControl _ribbonContent = null;
        private Storyboard _tabChangedStoryboard = null;

        /// <summary>
        /// Identifies the <see cref="Footer"/> property.
        /// </summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
            nameof(Footer),
            typeof(UIElement),
            typeof(TabbedCommandBar),
            new PropertyMetadata(new Border()));

        /// <summary>
        /// Gets or sets the <see cref="UIElement"/> to be displayed in the footer of the ribbon tab strip.
        /// </summary>
        public UIElement Footer
        {
            get { return (UIElement)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedCommandBar"/> class.
        /// </summary>
        public TabbedCommandBar()
        {
            DefaultStyleKey = typeof(TabbedCommandBar);

            SelectionChanged += RibbonNavigationView_SelectionChanged;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_ribbonContent != null)
            {
                _ribbonContent.Content = null;
            }

            // Get RibbonContent first, since setting SelectedItem requires it
            _ribbonContent = GetTemplateChild("PART_RibbonContent") as ContentControl;

            SelectedItem = MenuItems.FirstOrDefault();

            _tabChangedStoryboard = GetTemplateChild(nameof(_tabChangedStoryboard)) as Storyboard;
        }

        private void RibbonNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is TabbedCommandBarItem item)
            {
                _ribbonContent.Content = item;
                _tabChangedStoryboard?.Begin();
            }
            else if (args.SelectedItem is NavigationViewItem navItem)
            {
                // This code is a hack, but it's necessary if binding doesn't work.
                // RibbonContent might be null here, there should be a check
                _ribbonContent.Content = MenuItems[System.Math.Min(MenuItems.Count - 1, MenuItems.IndexOf(navItem))];
            }
        }
    }
}