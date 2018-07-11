using System.Windows;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkStrokeRenderingSegment
    {
        private global::Windows.UI.Input.Inking.InkStrokeRenderingSegment uwpInstance;

        public InkStrokeRenderingSegment(global::Windows.UI.Input.Inking.InkStrokeRenderingSegment args)
        {
            this.uwpInstance = args;
        }

        public Point BezierControlPoint1 { get => new Point(uwpInstance.BezierControlPoint1.X, uwpInstance.BezierControlPoint1.Y); }

        public Point BezierControlPoint2 { get => new Point(uwpInstance.BezierControlPoint2.X, uwpInstance.BezierControlPoint2.Y); }

        public Point Position { get => new Point(uwpInstance.Position.X, uwpInstance.Position.Y); }

        public float Pressure { get => uwpInstance.Pressure; }

        public float TiltX { get => uwpInstance.TiltX; }

        public float TiltY { get => uwpInstance.TiltY; }

        public float Twist { get => uwpInstance.Twist; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkStrokeRenderingSegment"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStrokeRenderingSegment"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokeRenderingSegment"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkStrokeRenderingSegment(
            global::Windows.UI.Input.Inking.InkStrokeRenderingSegment args)
        {
            return FromInkStrokeRenderingSegment(args);
        }

        /// <summary>
        /// Creates a <see cref="InkStrokeRenderingSegment"/> from <see cref="global::Windows.UI.Input.Inking.InkStrokeRenderingSegment"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokeRenderingSegment"/> instance containing the event data.</param>
        /// <returns><see cref="InkStrokeRenderingSegment"/></returns>
        public static InkStrokeRenderingSegment FromInkStrokeRenderingSegment(global::Windows.UI.Input.Inking.InkStrokeRenderingSegment args)
        {
            return new InkStrokeRenderingSegment(args);
        }
    }
}