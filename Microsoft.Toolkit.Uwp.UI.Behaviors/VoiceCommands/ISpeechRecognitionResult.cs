namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// Service used to report a recognized speech result
    /// </summary>
    public interface ISpeechRecognitionResult
    {
        /// <summary>
        /// Gets the Text of the recognized speech
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets the RawConfidence of the recognized speech
        /// </summary>
        double RawConfidence { get; }
    }
}
