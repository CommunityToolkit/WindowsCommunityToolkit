// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
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
        private ContentControl _ribbonContent = null;
        private Storyboard _tabChangedStoryboard = null;

        /// <summary>
        /// Gets or sets the brush to use as the background for content
        /// </summary>
        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ContentBackground"/> property.
        /// </summary>
        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(TabbedCommandBar), new PropertyMetadata(null));

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
            _tabChangedStoryboard = GetTemplateChild("TabChangedStoryboard") as Storyboard;

            SelectedItem = MenuItems.FirstOrDefault();
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
