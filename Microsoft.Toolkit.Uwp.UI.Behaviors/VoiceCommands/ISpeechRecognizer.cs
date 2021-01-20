namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// Service used to recongize speech
    /// </summary>
    public interface ISpeechRecognizer
    {
        /// <summary>
        /// Occurs when a speech is recognized
        /// </summary>
        event RecognizedEventHandler Recognized;
    }
}
