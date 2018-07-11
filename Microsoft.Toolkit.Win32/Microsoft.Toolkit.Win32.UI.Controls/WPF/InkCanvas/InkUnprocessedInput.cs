using System;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkUnprocessedInput : IDisposable
    {
        private global::Windows.UI.Input.Inking.InkUnprocessedInput uwpInstance;

        public InkUnprocessedInput(global::Windows.UI.Input.Inking.InkUnprocessedInput instance)
        {
            this.uwpInstance = instance;

            uwpInstance.PointerEntered += OnPointerEntered;
            uwpInstance.PointerExited += OnPointerExited;
            uwpInstance.PointerHovered += OnPointerHovered;
            uwpInstance.PointerLost += OnPointerLost;
            uwpInstance.PointerMoved += OnPointerMoved;
            uwpInstance.PointerPressed += OnPointerPressed;
            uwpInstance.PointerReleased += OnPointerReleased;
        }

        public InkPresenter InkPresenter { get => uwpInstance.InkPresenter; }

        // public event TypedEventHandler<InkUnprocessedInput, PointerEventArgs> PointerEntered;
        public event EventHandler<PointerEventArgs> PointerEntered = (sender, args) => { };

        private void OnPointerEntered(global::Windows.UI.Input.Inking.InkUnprocessedInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.PointerEntered?.Invoke(this, args);
        }

        // public event TypedEventHandler<InkUnprocessedInput, PointerEventArgs> PointerExited;
        public event EventHandler<PointerEventArgs> PointerExited = (sender, args) => { };

        private void OnPointerExited(global::Windows.UI.Input.Inking.InkUnprocessedInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.PointerExited?.Invoke(this, args);
        }

        // public event TypedEventHandler<InkUnprocessedInput, PointerEventArgs> PointerHovered;
        public event EventHandler<PointerEventArgs> PointerHovered = (sender, args) => { };

        private void OnPointerHovered(global::Windows.UI.Input.Inking.InkUnprocessedInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.PointerHovered?.Invoke(this, args);
        }

        // public event TypedEventHandler<InkUnprocessedInput, PointerEventArgs> PointerLost;
        public event EventHandler<PointerEventArgs> PointerLost = (sender, args) => { };

        private void OnPointerLost(global::Windows.UI.Input.Inking.InkUnprocessedInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.PointerLost?.Invoke(this, args);
        }

        // public event TypedEventHandler<InkUnprocessedInput, PointerEventArgs> PointerMoved;
        public event EventHandler<PointerEventArgs> PointerMoved = (sender, args) => { };

        private void OnPointerMoved(global::Windows.UI.Input.Inking.InkUnprocessedInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.PointerMoved?.Invoke(this, args);
        }

        // public event TypedEventHandler<InkUnprocessedInput, PointerEventArgs> PointerPressed;
        public event EventHandler<PointerEventArgs> PointerPressed = (sender, args) => { };

        private void OnPointerPressed(global::Windows.UI.Input.Inking.InkUnprocessedInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.PointerPressed?.Invoke(this, args);
        }

        // public event TypedEventHandler<InkUnprocessedInput, PointerEventArgs> PointerReleased;
        public event EventHandler<PointerEventArgs> PointerReleased = (sender, args) => { };

        private void OnPointerReleased(global::Windows.UI.Input.Inking.InkUnprocessedInput sender, global::Windows.UI.Core.PointerEventArgs args)
        {
            this.PointerReleased?.Invoke(this, args);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkUnprocessedInput"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkUnprocessedInput(
            global::Windows.UI.Input.Inking.InkUnprocessedInput args)
        {
            return FromInkStroke(args);
        }

        /// <summary>
        /// Creates a <see cref="InkUnprocessedInput"/> from <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/> instance containing the event data.</param>
        /// <returns><see cref="InkUnprocessedInput"/></returns>
        public static InkUnprocessedInput FromInkStroke(global::Windows.UI.Input.Inking.InkUnprocessedInput args)
        {
            return new InkUnprocessedInput(args);
        }

        public void Dispose()
        {
            uwpInstance.PointerEntered -= OnPointerEntered;
            uwpInstance.PointerExited -= OnPointerExited;
            uwpInstance.PointerHovered -= OnPointerHovered;
            uwpInstance.PointerLost -= OnPointerLost;
            uwpInstance.PointerMoved -= OnPointerMoved;
            uwpInstance.PointerPressed -= OnPointerPressed;
            uwpInstance.PointerReleased -= OnPointerReleased;
        }
    }
}