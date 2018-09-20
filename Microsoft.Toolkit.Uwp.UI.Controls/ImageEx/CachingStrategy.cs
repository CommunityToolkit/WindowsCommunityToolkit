namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The type of caching to be applied to <see cref="ImageEx"/>.
    /// Default is <see cref="Custom"/>
    /// </summary>
    public enum CachingStrategy
    {
        /// <summary>
        /// Caching is handled by <see cref="ImageEx"/>'s custom caching system.
        /// </summary>
        Custom,

        /// <summary>
        /// Caching is handled internally by UWP.
        /// </summary>
        Internal
    }
}
