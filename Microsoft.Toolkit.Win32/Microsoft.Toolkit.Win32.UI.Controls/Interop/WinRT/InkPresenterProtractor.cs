// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Input.Inking.InkPresenterProtractor"/>
    /// </summary>
    public class InkPresenterProtractor
    {
        private Windows.UI.Input.Inking.InkPresenterProtractor UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkPresenterProtractor"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Input.Inking.InkPresenterProtractor"/>
        /// </summary>
        public InkPresenterProtractor(Windows.UI.Input.Inking.InkPresenterProtractor instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.Radius"/>
        /// </summary>
        public double Radius
        {
            get => UwpInstance.Radius;
            set => UwpInstance.Radius = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.IsResizable"/>
        /// </summary>
        public bool IsResizable
        {
            get => UwpInstance.IsResizable;
            set => UwpInstance.IsResizable = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.IsCenterMarkerVisible"/>
        /// </summary>
        public bool IsCenterMarkerVisible
        {
            get => UwpInstance.IsCenterMarkerVisible;
            set => UwpInstance.IsCenterMarkerVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.IsAngleReadoutVisible"/>
        /// </summary>
        public bool IsAngleReadoutVisible
        {
            get => UwpInstance.IsAngleReadoutVisible;
            set => UwpInstance.IsAngleReadoutVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.AreTickMarksVisible"/>
        /// </summary>
        public bool AreTickMarksVisible
        {
            get => UwpInstance.AreTickMarksVisible;
            set => UwpInstance.AreTickMarksVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.AreRaysVisible"/>
        /// </summary>
        public bool AreRaysVisible
        {
            get => UwpInstance.AreRaysVisible;
            set => UwpInstance.AreRaysVisible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.AccentColor"/>
        /// </summary>
        public Windows.UI.Color AccentColor
        {
            get => UwpInstance.AccentColor;
            set => UwpInstance.AccentColor = value;
        }

        /*
        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.Transform"/>
        /// </summary>
        public System.Numerics.Matrix3x2 Transform
        {
            get => UwpInstance.Transform;
            set => UwpInstance.Transform = value;
        }
        */

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.IsVisible"/>
        /// </summary>
        public bool IsVisible
        {
            get => UwpInstance.IsVisible;
            set => UwpInstance.IsVisible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.ForegroundColor"/>
        /// </summary>
        public Windows.UI.Color ForegroundColor
        {
            get => UwpInstance.ForegroundColor;
            set => UwpInstance.ForegroundColor = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.BackgroundColor"/>
        /// </summary>
        public Windows.UI.Color BackgroundColor
        {
            get => UwpInstance.BackgroundColor;
            set => UwpInstance.BackgroundColor = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.Inking.InkPresenterProtractor.Kind"/>
        /// </summary>
        public Windows.UI.Input.Inking.InkPresenterStencilKind Kind
        {
            get => UwpInstance.Kind;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Input.Inking.InkPresenterProtractor"/> to <see cref="InkPresenterProtractor"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Input.Inking.InkPresenterProtractor"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkPresenterProtractor(
            Windows.UI.Input.Inking.InkPresenterProtractor args)
        {
            return FromInkPresenterProtractor(args);
        }

        /// <summary>
        /// Creates a <see cref="InkPresenterProtractor"/> from <see cref="Windows.UI.Input.Inking.InkPresenterProtractor"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Input.Inking.InkPresenterProtractor"/> instance containing the event data.</param>
        /// <returns><see cref="InkPresenterProtractor"/></returns>
        public static InkPresenterProtractor FromInkPresenterProtractor(Windows.UI.Input.Inking.InkPresenterProtractor args)
        {
            return new InkPresenterProtractor(args);
        }
    }
}