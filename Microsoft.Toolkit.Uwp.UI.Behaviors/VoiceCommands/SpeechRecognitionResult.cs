namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    public class SpeechRecognitionResult : ISpeechRecognitionResult
    {
        public string Text { get; }

        public double RawConfidence { get; }

        public SpeechRecognitionResult(string text, double rawConfidence)
        {
            Text = text;
            RawConfidence = rawConfidence;
        }
    }
}
