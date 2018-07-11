using System.Windows;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkPoint
    {
        private global::Windows.UI.Input.Inking.InkPoint uwpInstance;

        public InkPoint(global::Windows.UI.Input.Inking.InkPoint instance)
        {
            this.uwpInstance = instance;
        }

        public Point Position { get => new Point(uwpInstance.Position.X, uwpInstance.Position.Y); }

        public float Pressure { get => uwpInstance.Pressure; }

        public float TiltX { get => uwpInstance.TiltX; }

        public float TiltY { get => uwpInstance.TiltY; }

        public ulong Timestamp { get => uwpInstance.Timestamp; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkPoint"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPoint"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkPoint"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkPoint(
            global::Windows.UI.Input.Inking.InkPoint args)
        {
            return FromInkPoint(args);
        }

        /// <summary>
        /// Creates a <see cref="InkPoint"/> from <see cref="global::Windows.UI.Input.Inking.InkPoint"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkPoint"/> instance containing the event data.</param>
        /// <returns><see cref="InkPoint"/></returns>
        public static InkPoint FromInkPoint(global::Windows.UI.Input.Inking.InkPoint args)
        {
            return new InkPoint(args);
        }
    }
}