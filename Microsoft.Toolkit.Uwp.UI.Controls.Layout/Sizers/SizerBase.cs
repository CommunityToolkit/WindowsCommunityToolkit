// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Base class for splitting/resizing type controls like <see cref="GridSplitter"/> and <see cref="ContentSizer"/>. Acts similar to an enlarged <see cref="Windows.UI.Xaml.Controls.Primitives.Thumb"/> type control, but with keyboard support. Subclasses should override the various abstract methods here to implement their behavior.
    /// </summary>
    [ContentProperty(Name = nameof(Content))]
    public abstract partial class SizerBase : Control
    {
        /// <summary>
        /// Called when the control has been initialized.
        /// </summary>
        /// <param name="e">Loaded event args.</param>
        protected virtual void OnLoaded(RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="SizerBase"/> control starts to be dragged by the user.
        /// Implementor should record current state of manipulated target at this point in time.
        /// They will receive the cumulative change in <see cref="OnDragHorizontal(double)"/> or
        /// <see cref="OnDragVertical(double)"/> based on the <see cref="Orientation"/> property.
        /// </summary>
        protected abstract void OnDragStarting();

        /// <summary>
        /// Method to process the requested horizontal resize.
        /// </summary>
        /// <param name="horizontalChange">The <see cref="ManipulationDeltaRoutedEventArgs.Cumulative"/> horizontal change amount from the start in device-independent pixels DIP.</param>
        /// <returns><see cref="bool"/> indicates if a change was made</returns>
        protected abstract bool OnDragHorizontal(double horizontalChange);

        /// <summary>
        /// Method to process the requested vertical resize.
        /// </summary>
        /// <param name="verticalChange">The <see cref="ManipulationDeltaRoutedEventArgs.Cumulative"/> vertical change amount from the start in device-independent pixels DIP.</param>
        /// <returns><see cref="bool"/> indicates if a change was made</returns>
        protected abstract bool OnDragVertical(double verticalChange);

        /// <summary>
        /// Initializes a new instance of the <see cref="SizerBase"/> class.
        /// </summary>
        public SizerBase()
        {
            this.DefaultStyleKey = typeof(SizerBase);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Unregister Events
            Loaded -= SizerBase_Loaded;
            PointerEntered -= GridSplitter_PointerEntered;
            PointerExited -= GridSplitter_PointerExited;
            PointerPressed -= GridSplitter_PointerPressed;
            PointerReleased -= GridSplitter_PointerReleased;
            ManipulationStarted -= GridSplitter_ManipulationStarted;
            ManipulationCompleted -= GridSplitter_ManipulationCompleted;

            // Register Events
            Loaded += SizerBase_Loaded;
            PointerEntered += GridSplitter_PointerEntered;
            PointerExited += GridSplitter_PointerExited;
            PointerPressed += GridSplitter_PointerPressed;
            PointerReleased += GridSplitter_PointerReleased;
            ManipulationStarted += GridSplitter_ManipulationStarted;
            ManipulationCompleted += GridSplitter_ManipulationCompleted;
        }

        private void SizerBase_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= SizerBase_Loaded;

            OnLoaded(e);
        }
    }
}