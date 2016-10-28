namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal enum CultureTypes
    {
        NeutralCultures = 0x0001,             // Neutral cultures are cultures like "en", "de", "zh", etc, for enumeration this includes ALL neutrals regardless of other flags
        SpecificCultures = 0x0002,             // Non-netural cultuers.  Examples are "en-us", "zh-tw", etc., for enumeration this includes ALL specifics regardless of other flags
        InstalledWin32Cultures = 0x0004,             // Win32 installed cultures in the system and exists in the framework too., this is effectively all cultures

        AllCultures = NeutralCultures | SpecificCultures | InstalledWin32Cultures,

        UserCustomCulture = 0x0008,               // User defined custom culture
        ReplacementCultures = 0x0010,               // User defined replacement custom culture.
    }
}