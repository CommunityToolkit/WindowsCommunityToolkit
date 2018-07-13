using System;
using System.Drawing;
using System.Numerics;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkDrawingAttributes
    {
        private global::Windows.UI.Input.Inking.InkDrawingAttributes uwpInstance;

        public InkDrawingAttributes(global::Windows.UI.Input.Inking.InkDrawingAttributes args)
        {
            uwpInstance = args;
        }

        public static InkDrawingAttributes CreateForPencil() => global::Windows.UI.Input.Inking.InkDrawingAttributes.CreateForPencil();

        public Size Size { get => new Size((int)uwpInstance.Size.Width, (int)uwpInstance.Size.Height); set => uwpInstance.Size = new global::Windows.Foundation.Size(value.Width, value.Height); }

        public PenTipShape PenTip { get => (PenTipShape)(int)uwpInstance.PenTip; set => uwpInstance.PenTip = (global::Windows.UI.Input.Inking.PenTipShape)(int)value; }

        public bool IgnorePressure { get => uwpInstance.IgnorePressure; set => uwpInstance.IgnorePressure = value; }

        public bool FitToCurve { get => uwpInstance.FitToCurve; set => uwpInstance.FitToCurve = value; }

        public Color Color { get => Color.FromArgb(uwpInstance.Color.A, uwpInstance.Color.R, uwpInstance.Color.G, uwpInstance.Color.B); set => uwpInstance.Color = global::Windows.UI.Color.FromArgb(value.A, value.R, value.G, value.B); }

        internal global::Windows.UI.Input.Inking.InkDrawingAttributes ToUwp()
        {
            var result = new global::Windows.UI.Input.Inking.InkDrawingAttributes()
            {
                Color = global::Windows.UI.Color.FromArgb(Color.A, Color.R, Color.G, Color.B),
                DrawAsHighlighter = DrawAsHighlighter,
                FitToCurve = FitToCurve,
                IgnorePressure = IgnorePressure,
                IgnoreTilt = IgnoreTilt,
                PenTip = (global::Windows.UI.Input.Inking.PenTipShape)(int)PenTip,
                Size = new global::Windows.Foundation.Size(Size.Width, Size.Height)
            };
            return result;
        }

        // System.Numerics.Vector not available
        // public Matrix3x2 PenTipTransform { get => uwpInstance.PenTipTransform; set => uwpInstance.PenTipTransform; }
        public bool DrawAsHighlighter { get => uwpInstance.DrawAsHighlighter; set => uwpInstance.DrawAsHighlighter = value; }

        public InkDrawingAttributesKind Kind { get => (InkDrawingAttributesKind)(int)uwpInstance.Kind; }

        public double PencilOpacity { get => uwpInstance.PencilProperties.Opacity; }

        public bool IgnoreTilt { get => uwpInstance.IgnoreTilt; set => uwpInstance.IgnoreTilt = value; }

        public float ModelerScalingFactor { get => uwpInstance.ModelerAttributes.ScalingFactor; }

        public TimeSpan ModelerPredictionTime { get => uwpInstance.ModelerAttributes.PredictionTime; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkDrawingAttributes"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkDrawingAttributes"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkDrawingAttributes"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkDrawingAttributes(
            global::Windows.UI.Input.Inking.InkDrawingAttributes args)
        {
            return FromInkDrawingAttributes(args);
        }

        /// <summary>
        /// Creates a <see cref="InkDrawingAttributes"/> from <see cref="global::Windows.UI.Input.Inking.InkDrawingAttributes"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkDrawingAttributes"/> instance containing the event data.</param>
        /// <returns><see cref="InkDrawingAttributes"/></returns>
        public static InkDrawingAttributes FromInkDrawingAttributes(global::Windows.UI.Input.Inking.InkDrawingAttributes args)
        {
            return new InkDrawingAttributes(args);
        }
    }
}