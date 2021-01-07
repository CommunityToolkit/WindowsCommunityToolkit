namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    public interface ISpeechRecognitionResult
    {
        string Text { get; }
        double RawConfidence { get; }
    }

}
