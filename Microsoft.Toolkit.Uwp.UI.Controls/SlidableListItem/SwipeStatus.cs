namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Types of swipe status.
    /// </summary>
    public enum SwipeStatus
    {
        /// <summary>
        /// Swiping is not occuring.
        /// </summary>
        Idle,

        /// <summary>
        /// Swiping is going to start.
        /// </summary>
        Starting,

        /// <summary>
        /// Swiping to the left, but the command is disabled.
        /// </summary>
        DisabledSwipingToLeft,

        /// <summary>
        /// Swiping to the left below the threshold.
        /// </summary>
        SwipingToLeftThreshold,

        /// <summary>
        /// Swiping to the left above the threshold.
        /// </summary>
        SwipingPassedLeftThreshold,

        /// <summary>
        /// Swiping to the right, but the command is disabled.
        /// </summary>
        DisabledSwipingToRight,

        /// <summary>
        /// Swiping to the right below the threshold.
        /// </summary>
        SwipingToRightThreshold,

        /// <summary>
        /// Swiping to the right above the threshold.
        /// </summary>
        SwipingPassedRightThreshold
    }
}
