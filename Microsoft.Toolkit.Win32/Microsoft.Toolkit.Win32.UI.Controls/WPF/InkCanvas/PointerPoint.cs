using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class PointerPoint
    {
        private global::Windows.UI.Input.PointerPoint uwpInstance;

        public PointerPoint(global::Windows.UI.Input.PointerPoint instance)
        {
            this.uwpInstance = instance;
        }

        public static PointerPoint GetCurrentPoint(uint pointerId) => global::Windows.UI.Input.PointerPoint.GetCurrentPoint(pointerId);

        public static IList<PointerPoint> GetIntermediatePoints(uint pointerId) => global::Windows.UI.Input.PointerPoint.GetIntermediatePoints(pointerId).Cast<PointerPoint>().ToList();

        public uint FrameId { get => uwpInstance.FrameId; }

        public bool IsInContact { get => uwpInstance.IsInContact; }

        // public PointerDevice PointerDevice { get; }
        public uint PointerId { get => uwpInstance.PointerId; }

        public Point Position { get => new Point(uwpInstance.Position.X, uwpInstance.Position.Y); }

        // ublic PointerPointProperties Properties { get; }
        public Point RawPosition { get => new Point(uwpInstance.RawPosition.X, uwpInstance.Position.Y); }

        public ulong Timestamp { get => uwpInstance.Timestamp; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.PointerPoint"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.PointerPoint"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.PointerPoint"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PointerPoint(
            global::Windows.UI.Input.PointerPoint args)
        {
            return FromPointerPoint(args);
        }

        /// <summary>
        /// Creates a <see cref="PointerPoint"/> from <see cref="global::Windows.UI.Input.PointerPoint"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.PointerPoint"/> instance containing the event data.</param>
        /// <returns><see cref="PointerPoint"/></returns>
        public static PointerPoint FromPointerPoint(global::Windows.UI.Input.PointerPoint args)
        {
            return new PointerPoint(args);
        }
    }
}