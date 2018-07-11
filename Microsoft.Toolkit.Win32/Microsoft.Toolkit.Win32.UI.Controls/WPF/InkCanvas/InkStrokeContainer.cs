using System.Collections.Generic;
using System.Windows;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkStrokeContainer
    {
        private global::Windows.UI.Input.Inking.InkStrokeContainer uwpInstance;

        public InkStrokeContainer(global::Windows.UI.Input.Inking.InkStrokeContainer args)
        {
            this.uwpInstance = args;
        }

        public void AddStroke(InkStroke stroke) => uwpInstance.AddStroke(stroke.ToUwp());

        public Rect DeleteSelected();

        public Rect MoveSelected(Point translation);

        public Rect SelectWithPolyLine(IEnumerable<Point> polyline);

        public Rect SelectWithLine(Point from, Point to);

        public void CopySelectedToClipboard();

        public Rect PasteFromClipboard(Point position);

        public bool CanPasteFromClipboard();

        public IAsyncActionWithProgress<ulong> LoadAsync(IInputStream inputStream);

        public IAsyncOperationWithProgress<uint, uint> SaveAsync(IOutputStream outputStream);

        public void UpdateRecognitionResults(IReadOnlyList<InkRecognitionResult> recognitionResults);

        public IReadOnlyList<InkStroke> GetStrokes();

        public IReadOnlyList<InkRecognitionResult> GetRecognitionResults();

        public void AddStrokes(IEnumerable<InkStroke> strokes);

        public void Clear();

        public IAsyncOperationWithProgress<uint, uint> SaveAsync(IOutputStream outputStream, InkPersistenceFormat inkPersistenceFormat);

        public InkStroke GetStrokeById(uint id);

        public Rect BoundingRect { get; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkStrokeContainer"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStrokeContainer"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokeContainer"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkStrokeContainer(
            global::Windows.UI.Input.Inking.InkStrokeContainer args)
        {
            return FromInkStrokeContainer(args);
        }

        /// <summary>
        /// Creates a <see cref="InkStrokeContainer"/> from <see cref="global::Windows.UI.Input.Inking.InkStrokeContainer"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokeContainer"/> instance containing the event data.</param>
        /// <returns><see cref="InkStrokeContainer"/></returns>
        public static InkStrokeContainer FromInkStrokeContainer(global::Windows.UI.Input.Inking.InkStrokeContainer args)
        {
            return new InkStrokeContainer(args);
        }
    }
}