using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    // Summary:
    //     Provides properties, methods, and events for managing the input, processing,
    //     and rendering of ink input (standard and modified) for an InkCanvas control.
    public sealed class InkPresenter
    {
        private global::Windows.UI.Input.Inking.InkPresenter uwpInstance;

        public InkPresenter(global::Windows.UI.Input.Inking.InkPresenter args)
        {
            this.uwpInstance = args;
        }

        // Summary:
        //     Retrieves the InkDrawingAttributes used by the InkPresenter when rendering a
        //     new InkStroke on an InkCanvas control.
        //
        // Returns:
        //     The drawing attributes applied to a new ink stroke.
        public InkDrawingAttributes CopyDefaultDrawingAttributes() => uwpInstance.CopyDefaultDrawingAttributes();

        // Summary:
        //     Sets the InkDrawingAttributes used by the InkPresenter when rendering a new InkStroke
        //     on an InkCanvas control.
        //
        // Parameters:
        //   value:
        //     The drawing attributes for new ink strokes.
        public void UpdateDefaultDrawingAttributes(InkDrawingAttributes value) => uwpInstance.UpdateDefaultDrawingAttributes(value.ToUwp());

        // Summary:
        //     Indicates that your app requires complete control of ink input rendering.
        //
        // Returns:
        //     The object used for custom ink stroke rendering.
        public InkSynchronizer ActivateCustomDrying() => uwpInstance.ActivateCustomDrying();

        // Summary:
        //     Sets the inking behavior of one or more contact points on the associated InkCanvas
        //     control.
        //
        // Parameters:
        //   value:
        //     The inking behavior of one or more contact points. The default is SimpleSinglePointer.
        public void SetPredefinedConfiguration(InkPresenterPredefinedConfiguration value) => uwpInstance.SetPredefinedConfiguration((global::Windows.UI.Input.Inking.InkPresenterPredefinedConfiguration)(int)value);

        // Summary:
        //     Gets or sets an InkStrokeContainer object to store and manage the collection
        //     of InkStroke objects rendered by the InkPresenter.
        //
        // Returns:
        //     Stores and manages one or more InkStroke objects.
        public InkStrokeContainer StrokeContainer { get => uwpInstance.StrokeContainer; set => uwpInstance.StrokeContainer = value.UwpInstance; }

        // Summary:
        //     Gets or sets whether input is enabled for inking.
        //
        // Returns:
        //     **true** if input is enabled for inking. Otherwise, **false**.
        public bool IsInputEnabled { get => uwpInstance.IsInputEnabled; set => uwpInstance.IsInputEnabled = value; }

        // Summary:
        //     Gets or sets the input device type from which input data is collected by the
        //     InkPresenter to construct and render an InkStroke. The default is Pen.
        //
        // Returns:
        //     The input device types.
        public CoreInputDeviceTypes InputDeviceTypes { get => (CoreInputDeviceTypes)(uint)uwpInstance.InputDeviceTypes; set => uwpInstance.InputDeviceTypes = (global::Windows.UI.Core.CoreInputDeviceTypes)(uint)value; }

        // Summary:
        //     Gets how input is processed by the InkPresenter object.
        //
        // Returns:
        //     The input behavior.
        public InkInputProcessingConfiguration InputProcessingConfiguration { get => uwpInstance.InputProcessingConfiguration; }

        // Summary:
        //     Gets an InkStrokeInput object for managing ink input events.
        //
        // Returns:
        //     The ink input.
        public InkStrokeInput StrokeInput { get => uwpInstance.StrokeInput; }

        // Summary:
        //     Gets input (standard or modified) from the associated InkCanvas control and passes
        //     the data through for custom processing by the app. The data is not processed
        //     by the InkPresenter.
        //
        // Returns:
        //     The input from the InkCanvas control.
        public InkUnprocessedInput UnprocessedInput { get => uwpInstance.UnprocessedInput; }

        // Summary:
        //     Gets or sets how the InkPresenter object handles input (standard and modified)
        //     from the associated InkCanvas control when system is in high contrast mode.
        //
        // Returns:
        //     The ink color (selected or system) that works best against the background color.
        public InkHighContrastAdjustment HighContrastAdjustment { get => (InkHighContrastAdjustment)(int)uwpInstance.HighContrastAdjustment; set => uwpInstance.HighContrastAdjustment = (global::Windows.UI.Input.Inking.InkHighContrastAdjustment)(int)value; }

        // Summary:
        //     Gets which types of secondary input can be processed by the the InkPresenter
        //     object.
        //
        // Returns:
        //     The types of secondary input that can be processed.
        public InkInputConfiguration InputConfiguration { get => uwpInstance.InputConfiguration; }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStrokesCollectedEventArgs> StrokesCollected = (sender, args) => { };

        private void OnStrokesCollected(global::Windows.UI.Input.Inking.InkPresenter sender, global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs args)
        {
            this.StrokesCollected?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStrokesErasedEventArgs> StrokesErased = (sender, args) => { };

        private void OnStrokesErased(global::Windows.UI.Input.Inking.InkPresenter sender, global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs args)
        {
            this.StrokesErased?.Invoke(this, args);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkPresenter"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPresenter"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkPresenter"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkPresenter(
            global::Windows.UI.Input.Inking.InkPresenter args)
        {
            return FromInkPresenter(args);
        }

        /// <summary>
        /// Creates a <see cref="InkPresenter"/> from <see cref="global::Windows.UI.Input.Inking.InkPresenter"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkPresenter"/> instance containing the event data.</param>
        /// <returns><see cref="InkPresenter"/></returns>
        public static InkPresenter FromInkPresenter(global::Windows.UI.Input.Inking.InkPresenter args)
        {
            return new InkPresenter(args);
        }
    }
}
