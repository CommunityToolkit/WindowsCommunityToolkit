using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkStrokeContainer
    {
        internal global::Windows.UI.Input.Inking.InkStrokeContainer UwpInstance { get; }

        public InkStrokeContainer(global::Windows.UI.Input.Inking.InkStrokeContainer args)
        {
            this.UwpInstance = args;
        }

        public void AddStroke(InkStroke stroke) => UwpInstance.AddStroke(stroke.UwpInstance);

        public Rect DeleteSelected() => UwpInstance.DeleteSelected().ToWpf();

        public Rect MoveSelected(Point translation) => UwpInstance.MoveSelected(translation.ToUwp()).ToWpf();

        public Rect SelectWithPolyLine(IEnumerable<Point> polyline) => UwpInstance.SelectWithPolyLine(polyline.Select(p => p.ToUwp())).ToWpf();

        public Rect SelectWithLine(Point from, Point to) => UwpInstance.SelectWithLine(from.ToUwp(), to.ToUwp()).ToWpf();

        public void CopySelectedToClipboard() => UwpInstance.CopySelectedToClipboard();

        public Rect PasteFromClipboard(Point position) => UwpInstance.PasteFromClipboard(position.ToUwp()).ToWpf();

        public bool CanPasteFromClipboard() => UwpInstance.CanPasteFromClipboard();

        // public WrappedIAsyncOperationWithProgress<uint, uint> SaveAsync(WrappedIOutputStream outputStream, InkPersistenceFormat inkPersistenceFormat) => uwpInstance.SaveAsync(outputStream, inkPersistenceFormat).ToWpf();

        // public WrappedIAsyncActionWithProgress<ulong> LoadAsync(IInputStream inputStream) => uwpInstance.LoadAsync(inputStream.Instance).ToWpf();

        // public WrappedIAsyncOperationWithProgress<uint, uint> SaveAsync(WrappedIOutputStream outputStream) => uwpInstance.SaveAsync(outputStream.Instance).ToWpf();
        public void UpdateRecognitionResults(IReadOnlyList<InkRecognitionResult> recognitionResults) => UwpInstance.UpdateRecognitionResults(recognitionResults.ToUwp());

        public IReadOnlyList<InkStroke> GetStrokes() => UwpInstance.GetStrokes().Cast<InkStroke>().ToList();

        public IReadOnlyList<InkRecognitionResult> GetRecognitionResults() => UwpInstance.GetRecognitionResults().Cast<InkRecognitionResult>().ToList();

        public void AddStrokes(IEnumerable<InkStroke> strokes) => UwpInstance.AddStrokes(strokes.Select(s => s.UwpInstance));

        public void Clear() => UwpInstance.Clear();

        public InkStroke GetStrokeById(uint id) => UwpInstance.GetStrokeById(id);

        public Rect BoundingRect { get => UwpInstance.BoundingRect.ToWpf(); }

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