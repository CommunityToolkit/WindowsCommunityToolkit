// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Primitives
{
    /// <summary>
    /// Presents a <see cref="ColorPicker"/>'s preview color with optional accent colors.
    /// </summary>
    [TemplatePart(Name = nameof(ColorPreviewer.CenterCheckeredBackgroundBorder), Type = typeof(Border))]
    [TemplatePart(Name = nameof(ColorPreviewer.LeftCheckeredBackgroundBorder),   Type = typeof(Border))]
    [TemplatePart(Name = nameof(ColorPreviewer.RightCheckeredBackgroundBorder),  Type = typeof(Border))]
    [TemplatePart(Name = nameof(ColorPreviewer.P1PreviewBorder),                 Type = typeof(Border))]
    [TemplatePart(Name = nameof(ColorPreviewer.P2PreviewBorder),                 Type = typeof(Border))]
    [TemplatePart(Name = nameof(ColorPreviewer.N1PreviewBorder),                 Type = typeof(Border))]
    [TemplatePart(Name = nameof(ColorPreviewer.N2PreviewBorder),                 Type = typeof(Border))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:Statement should not be on a single line", Justification = "Inline brackets are used to improve code readability with repeated null checks.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "Whitespace is used to align code in columns for readability.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:Field names should begin with lower-case letter", Justification = "Only template parts start with a capital letter. This differentiates them from other fields.")]
    public partial class ColorPreviewer : Control
    {
        /// <summary>
        /// Event for when a color change is requested by user interaction with the previewer.
        /// </summary>
        public event EventHandler<HsvColor> ColorChangeRequested;

        private bool eventsConnected = false;

        private Border CenterCheckeredBackgroundBorder;
        private Border LeftCheckeredBackgroundBorder;
        private Border RightCheckeredBackgroundBorder;

        private Border N1PreviewBorder;
        private Border N2PreviewBorder;
        private Border P1PreviewBorder;
        private Border P2PreviewBorder;

        /***************************************************************************************
         *
         * Constructor/Destructor
         *
         ***************************************************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPreviewer"/> class.
        /// </summary>
        public ColorPreviewer()
            : base()
        {
            this.DefaultStyleKey = typeof(ColorPreviewer);
        }

        /***************************************************************************************
         *
         * Methods
         *
         ***************************************************************************************/

        /// <summary>
        /// Retrieves the named element in the instantiated ControlTemplate visual tree.
        /// </summary>
        /// <param name="childName">The name of the element to find.</param>
        /// <param name="isRequired">Whether the element is required and will throw an exception if missing.</param>
        /// <returns>The template child matching the given name and type.</returns>
        private T GetTemplateChild<T>(string childName, bool isRequired = false)
            where T : DependencyObject
        {
            T child = this.GetTemplateChild(childName) as T;
            if ((child == null) && isRequired)
            {
                ThrowArgumentNullException();
            }

            return child;

            static void ThrowArgumentNullException() => throw new ArgumentNullException(nameof(childName));
        }

        /// <summary>
        /// Connects or disconnects all control event handlers.
        /// </summary>
        /// <param name="connected">True to connect event handlers, otherwise false.</param>
        private void ConnectEvents(bool connected)
        {
            if (connected == true && this.eventsConnected == false)
            {
                // Add all events
                if (this.CenterCheckeredBackgroundBorder != null) { this.CenterCheckeredBackgroundBorder.Loaded += CheckeredBackgroundBorder_Loaded; }
                if (this.LeftCheckeredBackgroundBorder   != null) { this.LeftCheckeredBackgroundBorder.Loaded   += CheckeredBackgroundBorder_Loaded; }
                if (this.RightCheckeredBackgroundBorder  != null) { this.RightCheckeredBackgroundBorder.Loaded  += CheckeredBackgroundBorder_Loaded; }

                if (this.N1PreviewBorder != null) { this.N1PreviewBorder.PointerPressed += PreviewBorder_PointerPressed; }
                if (this.N2PreviewBorder != null) { this.N2PreviewBorder.PointerPressed += PreviewBorder_PointerPressed; }
                if (this.P1PreviewBorder != null) { this.P1PreviewBorder.PointerPressed += PreviewBorder_PointerPressed; }
                if (this.P2PreviewBorder != null) { this.P2PreviewBorder.PointerPressed += PreviewBorder_PointerPressed; }

                this.eventsConnected = true;
            }
            else if (connected == false && this.eventsConnected == true)
            {
                // Remove all events
                if (this.CenterCheckeredBackgroundBorder != null) { this.CenterCheckeredBackgroundBorder.Loaded -= CheckeredBackgroundBorder_Loaded; }
                if (this.LeftCheckeredBackgroundBorder   != null) { this.LeftCheckeredBackgroundBorder.Loaded   -= CheckeredBackgroundBorder_Loaded; }
                if (this.RightCheckeredBackgroundBorder  != null) { this.RightCheckeredBackgroundBorder.Loaded  -= CheckeredBackgroundBorder_Loaded; }

                if (this.N1PreviewBorder != null) { this.N1PreviewBorder.PointerPressed -= PreviewBorder_PointerPressed; }
                if (this.N2PreviewBorder != null) { this.N2PreviewBorder.PointerPressed -= PreviewBorder_PointerPressed; }
                if (this.P1PreviewBorder != null) { this.P1PreviewBorder.PointerPressed -= PreviewBorder_PointerPressed; }
                if (this.P2PreviewBorder != null) { this.P2PreviewBorder.PointerPressed -= PreviewBorder_PointerPressed; }

                this.eventsConnected = false;
            }

            return;
        }

        /***************************************************************************************
         *
         * OnEvent Overridable Methods
         *
         ***************************************************************************************/

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            // Remove any existing events present if the control was previously loaded then unloaded
            this.ConnectEvents(false);

            this.CenterCheckeredBackgroundBorder = this.GetTemplateChild<Border>(nameof(CenterCheckeredBackgroundBorder));
            this.LeftCheckeredBackgroundBorder   = this.GetTemplateChild<Border>(nameof(LeftCheckeredBackgroundBorder));
            this.RightCheckeredBackgroundBorder  = this.GetTemplateChild<Border>(nameof(RightCheckeredBackgroundBorder));

            this.N1PreviewBorder = this.GetTemplateChild<Border>(nameof(N1PreviewBorder));
            this.N2PreviewBorder = this.GetTemplateChild<Border>(nameof(N2PreviewBorder));
            this.P1PreviewBorder = this.GetTemplateChild<Border>(nameof(P1PreviewBorder));
            this.P2PreviewBorder = this.GetTemplateChild<Border>(nameof(P2PreviewBorder));

            // Must connect after controls are resolved
            this.ConnectEvents(true);

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Method called whenever a dependency property value is changed.
        /// This may happen through binding directly or a property setter.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The event arguments.</param>
        protected virtual void OnDependencyPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            return;
        }

        /// <summary>
        /// Called before the <see cref="ColorChangeRequested"/> event occurs.
        /// </summary>
        /// <param name="color">The newly requested color.</param>
        protected virtual void OnColorChangeRequested(HsvColor color)
        {
            this.ColorChangeRequested?.Invoke(this, color);
            return;
        }

        /***************************************************************************************
         *
         * Event Handling
         *
         ***************************************************************************************/

        /// <summary>
        /// Event handler to draw checkered backgrounds on-demand as controls are loaded.
        /// </summary>
        private async void CheckeredBackgroundBorder_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Border border)
            {
                int width = Convert.ToInt32(border.ActualWidth);
                int height = Convert.ToInt32(border.ActualHeight);

                var bitmap = await ColorPickerRenderingHelpers.CreateCheckeredBitmapAsync(
                    width,
                    height,
                    ColorPickerRenderingHelpers.CheckerBackgroundColor);

                if (bitmap != null)
                {
                    border.Background = await ColorPickerRenderingHelpers.BitmapToBrushAsync(bitmap, width, height);
                }
            }

            return;
        }

        /// <summary>
        /// Event handler for when a preview color panel is pressed.
        /// This will update the color to the background of the pressed panel.
        /// </summary>
        private void PreviewBorder_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Border border = sender as Border;
            int accentStep = 0;
            HsvColor hsvColor = this.HsvColor;

            // Get the value component delta
            try
            {
                accentStep = int.Parse(border.Tag?.ToString(), CultureInfo.InvariantCulture);
            }
            catch { }

            HsvColor newHsvColor = AccentColorConverter.GetAccent(hsvColor, accentStep);
            this.OnColorChangeRequested(newHsvColor);

            return;
        }
    }
}
