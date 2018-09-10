// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Input.Inking.InkPresenter"/>
    /// </summary>
    public class InkPresenter
    {
        private Windows.UI.Input.Inking.InkPresenter UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkPresenter"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Input.Inking.InkPresenter"/>
        /// </summary>
        public InkPresenter(Windows.UI.Input.Inking.InkPresenter instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Input.Inking.InkPresenter.StrokeContainer"/>
        /// </summary>
        public Windows.UI.Input.Inking.InkStrokeContainer StrokeContainer
        {
            get => UwpInstance.StrokeContainer;
            set => UwpInstance.StrokeContainer = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Input.Inking.InkPresenter.IsInputEnabled"/>
        /// </summary>
        public bool IsInputEnabled
        {
            get => UwpInstance.IsInputEnabled;
            set => UwpInstance.IsInputEnabled = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Input.Inking.InkPresenter.InputDeviceTypes"/>
        /// </summary>
        public CoreInputDeviceTypes InputDeviceTypes
        {
            get => (CoreInputDeviceTypes)(uint)UwpInstance.InputDeviceTypes;
            set => UwpInstance.InputDeviceTypes = (Windows.UI.Core.CoreInputDeviceTypes)(uint)value;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.Inking.InkPresenter.InputProcessingConfiguration"/>
        /// </summary>
        public Windows.UI.Input.Inking.InkInputProcessingConfiguration InputProcessingConfiguration
        {
            get => UwpInstance.InputProcessingConfiguration;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.Inking.InkPresenter.StrokeInput"/>
        /// </summary>
        public Windows.UI.Input.Inking.InkStrokeInput StrokeInput
        {
            get => UwpInstance.StrokeInput;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.Inking.InkPresenter.UnprocessedInput"/>
        /// </summary>
        public Windows.UI.Input.Inking.InkUnprocessedInput UnprocessedInput
        {
            get => UwpInstance.UnprocessedInput;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Input.Inking.InkPresenter.HighContrastAdjustment"/>
        /// </summary>
        public Windows.UI.Input.Inking.InkHighContrastAdjustment HighContrastAdjustment
        {
            get => UwpInstance.HighContrastAdjustment;
            set => UwpInstance.HighContrastAdjustment = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.Inking.InkPresenter.InputConfiguration"/>
        /// </summary>
        public Windows.UI.Input.Inking.InkInputConfiguration InputConfiguration
        {
            get => UwpInstance.InputConfiguration;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Input.Inking.InkPresenter"/> to <see cref="InkPresenter"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Input.Inking.InkPresenter"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkPresenter(
            Windows.UI.Input.Inking.InkPresenter args)
        {
            return FromInkPresenter(args);
        }

        /// <summary>
        /// Creates a <see cref="InkPresenter"/> from <see cref="Windows.UI.Input.Inking.InkPresenter"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Input.Inking.InkPresenter"/> instance containing the event data.</param>
        /// <returns><see cref="InkPresenter"/></returns>
        public static InkPresenter FromInkPresenter(Windows.UI.Input.Inking.InkPresenter args)
        {
            return new InkPresenter(args);
        }
    }
}