namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Enum defining inclusion of special characters.
    /// </summary>
    public enum MaskFormat
    {
        IncludePrompt = 0x0001,
        IncludeLiterals = 0x0002,

        // both of the above
        IncludePromptAndLiterals = 0x0003,

        // Never include special characters.
        ExcludePromptAndLiterals = 0x000
    }
}