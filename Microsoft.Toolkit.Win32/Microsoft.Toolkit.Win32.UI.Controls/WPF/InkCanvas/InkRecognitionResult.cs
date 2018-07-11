using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkRecognitionResult
    {
        private readonly global::Windows.UI.Input.Inking.InkRecognitionResult uwpInstance;

        public InkRecognitionResult(global::Windows.UI.Input.Inking.InkRecognitionResult instance)
        {
            this.uwpInstance = instance;
        }

        public IReadOnlyList<string> GetTextCandidates() => uwpInstance.GetTextCandidates();

        public IReadOnlyList<InkStroke> GetStrokes() => uwpInstance.GetStrokes().Cast<InkStroke>().ToList();

        public Rect BoundingRect { get => uwpInstance.BoundingRect.ToWpf(); }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkRecognitionResult"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkRecognitionResult"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkRecognitionResult"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkRecognitionResult(
            global::Windows.UI.Input.Inking.InkRecognitionResult args)
        {
            return FromInkRecognitionResult(args);
        }

        /// <summary>
        /// Creates a <see cref="InkRecognitionResult"/> from <see cref="global::Windows.UI.Input.Inking.InkRecognitionResult"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkRecognitionResult"/> instance containing the event data.</param>
        /// <returns><see cref="InkRecognitionResult"/></returns>
        public static InkRecognitionResult FromInkRecognitionResult(global::Windows.UI.Input.Inking.InkRecognitionResult args)
        {
            return new InkRecognitionResult(args);
        }
    }
}