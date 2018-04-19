namespace Microsoft.Toolkit.Uwp.PlatformSpecific
{
    /// <summary>
    /// Enum
    /// </summary>
    public enum PlatformKind
    {
        /// <summary>
        /// .NET and Pre-UWP WinRT
        /// </summary>
        Unchecked,

        /// <summary>
        /// Core UWP platform
        /// </summary>
        Uwp,

        /// <summary>
        /// Desktop, Mobile, IOT, Xbox extension SDK
        /// </summary>
        ExtensionSDK,

        /// <summary>
        /// User specified *Specific attribute on something
        /// </summary>
        User,
    }
}
