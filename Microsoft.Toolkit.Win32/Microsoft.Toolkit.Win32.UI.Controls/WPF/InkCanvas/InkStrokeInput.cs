using System;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkStrokeInput : IDisposable
    {
        private global::Windows.UI.Input.Inking.InkStrokeInput uwpInstance;

        public InkStrokeInput(global::Windows.UI.Input.Inking.InkStrokeInput instance)
        {
            this.uwpInstance = instance;

            uwpInstance.StrokeCanceled += OnStrokeCanceled;
            uwpInstance.StrokeContinued += OnStrokeContinued;
            uwpInstance.StrokeEnded += OnStrokeEnded;
            uwpInstance.StrokeStarted += OnStrokeStarted;
        }

        public InkPresenter InkPresenter { get => uwpInstance.InkPresenter; }

        public event EventHandler<PointerEventArgs> StrokeCanceled = (sender, args) => { };

        private void OnStrokeCanceled(global::Windows.UI.Input.Inking.InkStrokeInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.StrokeCanceled?.Invoke(this, args);
        }

        public event EventHandler<PointerEventArgs> StrokeContinued = (sender, args) => { };

        private void OnStrokeContinued(global::Windows.UI.Input.Inking.InkStrokeInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.StrokeContinued?.Invoke(this, args);
        }

        public event EventHandler<PointerEventArgs> StrokeEnded = (sender, args) => { };

        private void OnStrokeEnded(global::Windows.UI.Input.Inking.InkStrokeInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.StrokeEnded?.Invoke(this, args);
        }

        public event EventHandler<PointerEventArgs> StrokeStarted = (sender, args) => { };

        private void OnStrokeStarted(global::Windows.UI.Input.Inking.InkStrokeInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.StrokeStarted?.Invoke(this, args);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStrokeInput"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkStrokeInput(
            global::Windows.UI.Input.Inking.InkStrokeInput args)
        {
            return FromInkStroke(args);
        }

        /// <summary>
        /// Creates a <see cref="InkStrokeInput"/> from <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/> instance containing the event data.</param>
        /// <returns><see cref="InkStrokeInput"/></returns>
        public static InkStrokeInput FromInkStroke(global::Windows.UI.Input.Inking.InkStrokeInput args)
        {
            return new InkStrokeInput(args);
        }

        public void Dispose()
        {
            uwpInstance.StrokeCanceled -= OnStrokeCanceled;
            uwpInstance.StrokeContinued -= OnStrokeContinued;
            uwpInstance.StrokeEnded -= OnStrokeEnded;
            uwpInstance.StrokeStarted -= OnStrokeStarted;
        }
    }
}