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
        /// Method to process the requested horizontal resizing.
        /// </summary>
        /// <param name="horizontalChange">The requested horizontal change</param>
        /// <returns><see cref="bool"/> indicates if the change was made</returns>
        protected abstract bool OnHorizontalMove(double horizontalChange);

        /// <summary>
        /// Method to process the requested vertical resizing.
        /// </summary>
        /// <param name="verticalChange">The requested vertical change</param>
        /// <returns><see cref="bool"/> indicates if the change was made</returns>
        protected abstract bool OnVerticalMove(double verticalChange);

        /// <summary>
        /// Called when the control has been initialized.
        /// </summary>
        /// <param name="e">Loaded event args.</param>
        protected abstract void OnLoaded(RoutedEventArgs e);

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