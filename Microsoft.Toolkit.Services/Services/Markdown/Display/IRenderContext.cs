namespace Microsoft.Toolkit.Services.Markdown.Display
{
    /// <summary>
    /// Helper for holding persistent state of Renderer.
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Gets or sets a value indicating whether to trim whitespace.
        /// </summary>
        bool TrimLeadingWhitespace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Current Element is being rendered inside a Link.
        /// </summary>
        bool WithinHyperlink { get; set; }

        /// <summary>
        /// Clones the Context.
        /// </summary>
        /// <returns>Clone</returns>
        IRenderContext Clone();
    }
}