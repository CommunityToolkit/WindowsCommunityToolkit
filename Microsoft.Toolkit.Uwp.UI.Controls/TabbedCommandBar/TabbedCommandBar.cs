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
    [TemplatePart(Name = "PART_RibbonContentBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_TabChangedStoryboard", Type = typeof(Storyboard))]
    public class TabbedCommandBar : NavigationView
    {
        private ContentControl _ribbonContent = null;
        private Border _ribbonContentBorder = null;
        private Storyboard _tabChangedStoryboard = null;

        /// <summary>
        /// Gets or sets the brush to use as the background for <see cref="TabbedCommandBarItem"/>s in MenuItems.
        /// </summary>
        public Brush ItemBackground
        {
            get => (Brush)GetValue(ItemBackgroundProperty);
            set => SetValue(ItemBackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ItemBackground"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.Register(nameof(ItemBackground), typeof(Brush), typeof(TabbedCommandBar), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedCommandBar"/> class.
        /// </summary>
        public TabbedCommandBar()
        {
            DefaultStyleKey = typeof(TabbedCommandBar);

            SelectionChanged += (NavigationView sender, NavigationViewSelectionChangedEventArgs args) =>
            {
                _ribbonContentBorder.Background = (sender.SelectedItem as Control).Background;
                _tabChangedStoryboard?.Begin();
            };
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
            _ribbonContentBorder = GetTemplateChild("PART_RibbonContentBorder") as Border;
            _tabChangedStoryboard = GetTemplateChild("TabChangedStoryboard") as Storyboard;

            SelectedItem = MenuItems.FirstOrDefault();
        }
    }
}
