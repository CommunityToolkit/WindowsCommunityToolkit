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
    [ContentProperty(Name = nameof(Items))]
    [TemplatePart(Name = "PART_RibbonNavigationView", Type = typeof(NavigationView))]
    [TemplatePart(Name = "PART_RibbonContent", Type = typeof(ContentControl))]
    [TemplatePart(Name = "PART_TabChangedStoryboard", Type = typeof(Storyboard))]
    public class TabbedCommandBar : Control
    {
        private NavigationView _ribbonNavigationView = null;
        private ContentControl _ribbonContent = null;
        private Storyboard _tabChangedStoryboard = null;

        // This should probably be made public at some point
        private TabbedCommandBarItem SelectedTab { get; set; }

        /// <summary>
        /// Identifies the <see cref="Items"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items),
            typeof(IList<object>),
            typeof(TabbedCommandBar),
            new PropertyMetadata(new List<object>()));

        /// <summary>
        /// Identifies the <see cref="Footer"/> property.
        /// </summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
            nameof(Footer),
            typeof(UIElement),
            typeof(TabbedCommandBar),
            new PropertyMetadata(new Border()));

        // This should be an IList<TabbedCommandBarItem>, but Intellisense really doesn't like that.
        /// <summary>
        /// Gets or sets A list of <see cref="TabbedCommandBarItem"/>s to display in this <see cref="TabbedCommandBar"/>.
        /// </summary>
        public IList<object> Items
        {
            get { return (IList<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

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
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get RibbonContent first, since setting SelectedItem requires it
            _ribbonContent = GetTemplateChild("PART_RibbonContent") as ContentControl;

            _ribbonNavigationView = GetTemplateChild("PART_RibbonNavigationView") as NavigationView;
            if (_ribbonNavigationView != null)
            {
                // Populate the NavigationView with items
                // TODO: Get binding working, necessary for contextual tabs
                _ribbonNavigationView.MenuItems.Clear();
                foreach (TabbedCommandBarItem item in Items)
                {
                    _ribbonNavigationView.MenuItems.Add(item);
                }
                _ribbonNavigationView.PaneFooter = Footer;

                _ribbonNavigationView.SelectionChanged += RibbonNavigationView_SelectionChanged;
                _ribbonNavigationView.SelectedItem = _ribbonNavigationView.MenuItems.FirstOrDefault();
            }

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
                _ribbonContent.Content = Items[System.Math.Min(Items.Count - 1, _ribbonNavigationView.MenuItems.IndexOf(navItem))];
            }
        }
    }
}