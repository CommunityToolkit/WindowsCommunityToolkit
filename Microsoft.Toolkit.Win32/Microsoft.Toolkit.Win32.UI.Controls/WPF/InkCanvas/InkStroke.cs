using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkStroke
    {
        private global::Windows.UI.Input.Inking.InkStroke uwpInstance;

        public InkStroke(global::Windows.UI.Input.Inking.InkStroke instance)
        {
            uwpInstance = instance;
        }

        public IReadOnlyList<InkStrokeRenderingSegment> GetRenderingSegments() => uwpInstance.GetRenderingSegments().Cast<InkStrokeRenderingSegment>().ToList();

        internal global::Windows.UI.Input.Inking.InkStroke ToUwp()
        {
            return uwpInstance;
        }

        public InkStroke Clone() => uwpInstance.Clone();

        public IReadOnlyList<InkPoint> GetInkPoints() => uwpInstance.GetInkPoints().Cast<InkPoint>().ToList();

        public bool Selected { get => uwpInstance.Selected; set => uwpInstance.Selected = value; }

        public InkDrawingAttributes DrawingAttributes { get => uwpInstance.DrawingAttributes; set => uwpInstance.DrawingAttributes = value.ToUwp(); }

        public Rect BoundingRect { get => new Rect(uwpInstance.BoundingRect.X, uwpInstance.BoundingRect.Y, uwpInstance.BoundingRect.Width, uwpInstance.BoundingRect.Height); }

        public bool Recognized { get => uwpInstance.Recognized; }

        // public Matrix3x2 PointTransform { get; set; }
        public DateTimeOffset? StrokeStartedTime { get => uwpInstance.StrokeStartedTime; set => uwpInstance.StrokeStartedTime = value; }

        public TimeSpan? StrokeDuration { get => uwpInstance.StrokeDuration; set => uwpInstance.StrokeDuration = value; }

        public uint Id { get => uwpInstance.Id; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkStroke"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStroke"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStroke"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkStroke(
            global::Windows.UI.Input.Inking.InkStroke args)
        {
            return FromInkStroke(args);
        }

        /// <summary>
        /// Creates a <see cref="InkStroke"/> from <see cref="global::Windows.UI.Input.Inking.InkStroke"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStroke"/> instance containing the event data.</param>
        /// <returns><see cref="InkStroke"/></returns>
        public static InkStroke FromInkStroke(global::Windows.UI.Input.Inking.InkStroke args)
        {
            return new InkStroke(args);
        }
    }
}