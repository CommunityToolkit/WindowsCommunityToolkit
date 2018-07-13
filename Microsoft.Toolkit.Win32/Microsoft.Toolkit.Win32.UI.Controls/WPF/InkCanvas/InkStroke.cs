using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkStroke
    {
        public global::Windows.UI.Input.Inking.InkStroke UwpInstance { get; }

        public InkStroke(global::Windows.UI.Input.Inking.InkStroke instance)
        {
            UwpInstance = instance;
        }

        public IReadOnlyList<InkStrokeRenderingSegment> GetRenderingSegments() => UwpInstance.GetRenderingSegments().Cast<InkStrokeRenderingSegment>().ToList();

        public InkStroke Clone() => UwpInstance.Clone();

        public IReadOnlyList<InkPoint> GetInkPoints() => UwpInstance.GetInkPoints().Cast<InkPoint>().ToList();

        public bool Selected { get => UwpInstance.Selected; set => UwpInstance.Selected = value; }

        public InkDrawingAttributes DrawingAttributes { get => UwpInstance.DrawingAttributes; set => UwpInstance.DrawingAttributes = value.ToUwp(); }

        public Rect BoundingRect { get => new Rect(UwpInstance.BoundingRect.X, UwpInstance.BoundingRect.Y, UwpInstance.BoundingRect.Width, UwpInstance.BoundingRect.Height); }

        public bool Recognized { get => UwpInstance.Recognized; }

        // public Matrix3x2 PointTransform { get; set; }
        public DateTimeOffset? StrokeStartedTime { get => UwpInstance.StrokeStartedTime; set => UwpInstance.StrokeStartedTime = value; }

        public TimeSpan? StrokeDuration { get => UwpInstance.StrokeDuration; set => UwpInstance.StrokeDuration = value; }

        public uint Id { get => UwpInstance.Id; }

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