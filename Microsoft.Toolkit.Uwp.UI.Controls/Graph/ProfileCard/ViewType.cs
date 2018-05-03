namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The view type.
    /// </summary>
    public enum ViewType
    {
        /// <summary>
        /// Only the user photo is shown.
        /// </summary>
        PictureOnly = 0,

        /// <summary>
        /// Only the user email is shown.
        /// </summary>
        EmailOnly = 1,

        /// <summary>
        /// A basic user profile is shown, and the user photo is place on the left side.
        /// </summary>
        LargeProfilePhotoLeft = 2,

        /// <summary>
        /// A basic user profile is shown, and the user photo is place on the right side.
        /// </summary>
        LargeProfilePhotoRight = 3
    }
}
