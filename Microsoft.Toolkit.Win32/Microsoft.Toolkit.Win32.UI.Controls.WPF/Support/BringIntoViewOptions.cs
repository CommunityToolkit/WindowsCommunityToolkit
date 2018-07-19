namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.BringIntoViewOptions"/>
    /// </summary>
    public class BringIntoViewOptions
    {
        internal global::Windows.UI.Xaml.BringIntoViewOptions UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BringIntoViewOptions"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.BringIntoViewOptions"/>
        /// </summary>
        public BringIntoViewOptions(global::Windows.UI.Xaml.BringIntoViewOptions instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.BringIntoViewOptions.TargetRect"/>
        /// </summary>
        public global::Windows.Foundation.Rect? TargetRect
        {
            get => UwpInstance.TargetRect;
            set => UwpInstance.TargetRect = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.BringIntoViewOptions.AnimationDesired"/>
        /// </summary>
        public bool AnimationDesired
        {
            get => UwpInstance.AnimationDesired;
            set => UwpInstance.AnimationDesired = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.BringIntoViewOptions.VerticalOffset"/>
        /// </summary>
        public double VerticalOffset
        {
            get => UwpInstance.VerticalOffset;
            set => UwpInstance.VerticalOffset = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.BringIntoViewOptions.VerticalAlignmentRatio"/>
        /// </summary>
        public double VerticalAlignmentRatio
        {
            get => UwpInstance.VerticalAlignmentRatio;
            set => UwpInstance.VerticalAlignmentRatio = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.BringIntoViewOptions.HorizontalOffset"/>
        /// </summary>
        public double HorizontalOffset
        {
            get => UwpInstance.HorizontalOffset;
            set => UwpInstance.HorizontalOffset = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.BringIntoViewOptions.HorizontalAlignmentRatio"/>
        /// </summary>
        public double HorizontalAlignmentRatio
        {
            get => UwpInstance.HorizontalAlignmentRatio;
            set => UwpInstance.HorizontalAlignmentRatio = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.BringIntoViewOptions"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.BringIntoViewOptions"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.BringIntoViewOptions"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BringIntoViewOptions(
            global::Windows.UI.Xaml.BringIntoViewOptions args)
        {
            return FromBringIntoViewOptions(args);
        }

        /// <summary>
        /// Creates a <see cref="BringIntoViewOptions"/> from <see cref="global::Windows.UI.Xaml.BringIntoViewOptions"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.BringIntoViewOptions"/> instance containing the event data.</param>
        /// <returns><see cref="BringIntoViewOptions"/></returns>
        public static BringIntoViewOptions FromBringIntoViewOptions(global::Windows.UI.Xaml.BringIntoViewOptions args)
        {
            return new BringIntoViewOptions(args);
        }
    }
}