namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Input.Inking.InkStrokeContainer"/>
    /// </summary>
    public class InkStrokeContainer
    {
        private global::Windows.UI.Input.Inking.InkStrokeContainer uwpInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkStrokeContainer"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Input.Inking.InkStrokeContainer"/>
        /// </summary>
        public InkStrokeContainer(global::Windows.UI.Input.Inking.InkStrokeContainer instance)
        {
            this.uwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Input.Inking.InkStrokeContainer.BoundingRect"/>
        /// </summary>
        public global::Windows.Foundation.Rect BoundingRect
        {
            get => uwpInstance.BoundingRect;
        }

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