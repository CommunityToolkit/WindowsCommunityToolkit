namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Input.Inking.InkInputConfiguration"/>
    /// </summary>
    public class InkInputConfiguration
    {
        private global::Windows.UI.Input.Inking.InkInputConfiguration uwpInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkInputConfiguration"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Input.Inking.InkInputConfiguration"/>
        /// </summary>
        public InkInputConfiguration(global::Windows.UI.Input.Inking.InkInputConfiguration instance)
        {
            this.uwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Input.Inking.InkInputConfiguration.IsPrimaryBarrelButtonInputEnabled"/>
        /// </summary>
        public bool IsPrimaryBarrelButtonInputEnabled
        {
            get => uwpInstance.IsPrimaryBarrelButtonInputEnabled;
            set => uwpInstance.IsPrimaryBarrelButtonInputEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Input.Inking.InkInputConfiguration.IsEraserInputEnabled"/>
        /// </summary>
        public bool IsEraserInputEnabled
        {
            get => uwpInstance.IsEraserInputEnabled;
            set => uwpInstance.IsEraserInputEnabled = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkInputConfiguration"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkInputConfiguration"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkInputConfiguration"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkInputConfiguration(
            global::Windows.UI.Input.Inking.InkInputConfiguration args)
        {
            return FromInkInputConfiguration(args);
        }

        /// <summary>
        /// Creates a <see cref="InkInputConfiguration"/> from <see cref="global::Windows.UI.Input.Inking.InkInputConfiguration"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkInputConfiguration"/> instance containing the event data.</param>
        /// <returns><see cref="InkInputConfiguration"/></returns>
        public static InkInputConfiguration FromInkInputConfiguration(global::Windows.UI.Input.Inking.InkInputConfiguration args)
        {
            return new InkInputConfiguration(args);
        }
    }
}