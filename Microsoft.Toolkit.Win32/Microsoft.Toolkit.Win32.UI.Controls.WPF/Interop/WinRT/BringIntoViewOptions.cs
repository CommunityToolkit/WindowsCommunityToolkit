// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.BringIntoViewOptions"/>
    /// </summary>
    public class BringIntoViewOptions
    {
        internal Windows.UI.Xaml.BringIntoViewOptions UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BringIntoViewOptions"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.BringIntoViewOptions"/>
        /// </summary>
        public BringIntoViewOptions(Windows.UI.Xaml.BringIntoViewOptions instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.BringIntoViewOptions.TargetRect"/>
        /// </summary>
        public Windows.Foundation.Rect? TargetRect
        {
            get => UwpInstance.TargetRect;
            set => UwpInstance.TargetRect = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.BringIntoViewOptions.AnimationDesired"/>
        /// </summary>
        public bool AnimationDesired
        {
            get => UwpInstance.AnimationDesired;
            set => UwpInstance.AnimationDesired = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.BringIntoViewOptions.VerticalOffset"/>
        /// </summary>
        public double VerticalOffset
        {
            get => UwpInstance.VerticalOffset;
            set => UwpInstance.VerticalOffset = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.BringIntoViewOptions.VerticalAlignmentRatio"/>
        /// </summary>
        public double VerticalAlignmentRatio
        {
            get => UwpInstance.VerticalAlignmentRatio;
            set => UwpInstance.VerticalAlignmentRatio = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.BringIntoViewOptions.HorizontalOffset"/>
        /// </summary>
        public double HorizontalOffset
        {
            get => UwpInstance.HorizontalOffset;
            set => UwpInstance.HorizontalOffset = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.BringIntoViewOptions.HorizontalAlignmentRatio"/>
        /// </summary>
        public double HorizontalAlignmentRatio
        {
            get => UwpInstance.HorizontalAlignmentRatio;
            set => UwpInstance.HorizontalAlignmentRatio = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.BringIntoViewOptions"/> to <see cref="BringIntoViewOptions"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.BringIntoViewOptions"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BringIntoViewOptions(
            Windows.UI.Xaml.BringIntoViewOptions args)
        {
            return FromBringIntoViewOptions(args);
        }

        /// <summary>
        /// Creates a <see cref="BringIntoViewOptions"/> from <see cref="Windows.UI.Xaml.BringIntoViewOptions"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.BringIntoViewOptions"/> instance containing the event data.</param>
        /// <returns><see cref="BringIntoViewOptions"/></returns>
        public static BringIntoViewOptions FromBringIntoViewOptions(Windows.UI.Xaml.BringIntoViewOptions args)
        {
            return new BringIntoViewOptions(args);
        }
    }
}