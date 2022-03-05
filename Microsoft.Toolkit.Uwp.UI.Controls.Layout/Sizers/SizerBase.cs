// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Automation.Peers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Base class for splitting/resizing type controls like <see cref="GridSplitter"/> and <see cref="ContentSizer"/>. Acts similar to an enlarged <see cref="Windows.UI.Xaml.Controls.Primitives.Thumb"/> type control, but with keyboard support. Subclasses should override the various abstract methods here to implement their behavior.
    /// </summary>
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
        /// <remarks>
        /// This method is also called at the start of a keyboard interaction. Keyboard strokes use the same pattern to emulate a mouse movement for a single change. The appropriate
        /// <see cref="OnDragHorizontal(double)"/> or <see cref="OnDragVertical(double)"/>
        /// method will also be called after when the keyboard is used.
        /// </remarks>
        protected abstract void OnDragStarting();

        /// <summary>
        /// Method to process the requested horizontal resize.
        /// </summary>
        /// <param name="horizontalChange">The <see cref="ManipulationDeltaRoutedEventArgs.Cumulative"/> horizontal change amount from the start in device-independent pixels DIP.</param>
        /// <returns><see cref="bool"/> indicates if a change was made</returns>
        /// <remarks>
        /// The value provided here is the cumulative change from the beginning of the
        /// manipulation. This method will be used regardless of input device. It will already
        /// be adjusted for RightToLeft <see cref="FlowDirection"/> of the containing
        /// layout/settings. It will also already account for any settings such as
        /// <see cref="DragIncrement"/> or <see cref="KeyboardIncrement"/>. The implementor
        /// just needs to use the provided value to manipulate their baseline stored
        /// in <see cref="OnDragStarting"/> to provide the desired change.
        /// </remarks>
        protected abstract bool OnDragHorizontal(double horizontalChange);

        /// <summary>
        /// Method to process the requested vertical resize.
        /// </summary>
        /// <param name="verticalChange">The <see cref="ManipulationDeltaRoutedEventArgs.Cumulative"/> vertical change amount from the start in device-independent pixels DIP.</param>
        /// <returns><see cref="bool"/> indicates if a change was made</returns>
        /// <remarks>
        /// The value provided here is the cumulative change from the beginning of the
        /// manipulation. This method will be used regardless of input device. It will also
        /// already account for any settings such as <see cref="DragIncrement"/> or
        /// <see cref="KeyboardIncrement"/>. The implementor just needs
        /// to use the provided value to manipulate their baseline stored
        /// in <see cref="OnDragStarting"/> to provide the desired change.
        /// </remarks>
        protected abstract bool OnDragVertical(double verticalChange);

        /// <summary>
        /// Initializes a new instance of the <see cref="SizerBase"/> class.
        /// </summary>
        public SizerBase()
        {
            this.DefaultStyleKey = typeof(SizerBase);
        }

        /// <summary>
        /// Creates AutomationPeer (<see cref="UIElement.OnCreateAutomationPeer"/>)
        /// </summary>
        /// <returns>An automation peer for this <see cref="SizerBase"/>.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new SizerAutomationPeer(this);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Unregister Events
            Loaded -= SizerBase_Loaded;
            PointerEntered -= SizerBase_PointerEntered;
            PointerExited -= SizerBase_PointerExited;
            PointerPressed -= SizerBase_PointerPressed;
            PointerReleased -= SizerBase_PointerReleased;
            ManipulationStarted -= SizerBase_ManipulationStarted;
            ManipulationCompleted -= SizerBase_ManipulationCompleted;
            IsEnabledChanged -= SizerBase_IsEnabledChanged;

            // Register Events
            Loaded += SizerBase_Loaded;
            PointerEntered += SizerBase_PointerEntered;
            PointerExited += SizerBase_PointerExited;
            PointerPressed += SizerBase_PointerPressed;
            PointerReleased += SizerBase_PointerReleased;
            ManipulationStarted += SizerBase_ManipulationStarted;
            ManipulationCompleted += SizerBase_ManipulationCompleted;
            IsEnabledChanged += SizerBase_IsEnabledChanged;

            // Trigger initial state transition based on if we're Enabled or not currently.
            SizerBase_IsEnabledChanged(this, null);
        }

        private void SizerBase_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= SizerBase_Loaded;

            OnLoaded(e);
        }
    }
}