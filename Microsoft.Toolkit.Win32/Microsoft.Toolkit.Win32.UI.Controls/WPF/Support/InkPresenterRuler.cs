namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler"/>
    /// </summary>
    public class InkPresenterRuler
    {
        internal global::Windows.UI.Input.Inking.InkPresenterRuler UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkPresenterRuler"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler"/>
        /// </summary>
        public InkPresenterRuler(global::Windows.UI.Input.Inking.InkPresenterRuler instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.Width"/>
        /// </summary>
        public double Width
        {
            get => UwpInstance.Width;
            set => UwpInstance.Width = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.Length"/>
        /// </summary>
        public double Length
        {
            get => UwpInstance.Length;
            set => UwpInstance.Length = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.IsCompassVisible"/>
        /// </summary>
        public bool IsCompassVisible
        {
            get => UwpInstance.IsCompassVisible;
            set => UwpInstance.IsCompassVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.AreTickMarksVisible"/>
        /// </summary>
        public bool AreTickMarksVisible
        {
            get => UwpInstance.AreTickMarksVisible;
            set => UwpInstance.AreTickMarksVisible = value;
        }

        /*
        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.Transform"/>
        /// </summary>
        public System.Numerics.Matrix3x2 Transform
        {
            get => UwpInstance.Transform;
            set => UwpInstance.Transform = value;
        }
        */

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.IsVisible"/>
        /// </summary>
        public bool IsVisible
        {
            get => UwpInstance.IsVisible;
            set => UwpInstance.IsVisible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.ForegroundColor"/>
        /// </summary>
        public global::Windows.UI.Color ForegroundColor
        {
            get => UwpInstance.ForegroundColor;
            set => UwpInstance.ForegroundColor = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.BackgroundColor"/>
        /// </summary>
        public global::Windows.UI.Color BackgroundColor
        {
            get => UwpInstance.BackgroundColor;
            set => UwpInstance.BackgroundColor = value;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler.Kind"/>
        /// </summary>
        public global::Windows.UI.Input.Inking.InkPresenterStencilKind Kind
        {
            get => UwpInstance.Kind;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPresenterRuler"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkPresenterRuler(
            global::Windows.UI.Input.Inking.InkPresenterRuler args)
        {
            return FromInkPresenterRuler(args);
        }

        /// <summary>
        /// Creates a <see cref="InkPresenterRuler"/> from <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkPresenterRuler"/> instance containing the event data.</param>
        /// <returns><see cref="InkPresenterRuler"/></returns>
        public static InkPresenterRuler FromInkPresenterRuler(global::Windows.UI.Input.Inking.InkPresenterRuler args)
        {
            return new InkPresenterRuler(args);
        }
    }
}