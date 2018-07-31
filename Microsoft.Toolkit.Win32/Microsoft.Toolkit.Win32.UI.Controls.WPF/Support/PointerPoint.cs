// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Input.PointerPoint"/>
    /// </summary>
    public class PointerPoint
    {
        internal Windows.UI.Input.PointerPoint UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerPoint"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Input.PointerPoint"/>
        /// </summary>
        public PointerPoint(Windows.UI.Input.PointerPoint instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.PointerPoint.FrameId"/>
        /// </summary>
        public uint FrameId
        {
            get => UwpInstance.FrameId;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.UI.Input.PointerPoint.IsInContact"/>
        /// </summary>
        public bool IsInContact
        {
            get => UwpInstance.IsInContact;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.PointerPoint.PointerDevice"/>
        /// </summary>
        public Windows.Devices.Input.PointerDevice PointerDevice
        {
            get => UwpInstance.PointerDevice;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.PointerPoint.PointerId"/>
        /// </summary>
        public uint PointerId
        {
            get => UwpInstance.PointerId;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.PointerPoint.Position"/>
        /// </summary>
        public Windows.Foundation.Point Position
        {
            get => UwpInstance.Position;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.PointerPoint.Properties"/>
        /// </summary>
        public Windows.UI.Input.PointerPointProperties Properties
        {
            get => UwpInstance.Properties;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.PointerPoint.RawPosition"/>
        /// </summary>
        public Windows.Foundation.Point RawPosition
        {
            get => UwpInstance.RawPosition;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.PointerPoint.Timestamp"/>
        /// </summary>
        public ulong Timestamp
        {
            get => UwpInstance.Timestamp;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Input.PointerPoint"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.PointerPoint"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Input.PointerPoint"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PointerPoint(
            Windows.UI.Input.PointerPoint args)
        {
            return FromPointerPoint(args);
        }

        /// <summary>
        /// Creates a <see cref="PointerPoint"/> from <see cref="Windows.UI.Input.PointerPoint"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Input.PointerPoint"/> instance containing the event data.</param>
        /// <returns><see cref="PointerPoint"/></returns>
        public static PointerPoint FromPointerPoint(Windows.UI.Input.PointerPoint args)
        {
            return new PointerPoint(args);
        }
    }
}