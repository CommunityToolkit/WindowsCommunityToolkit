using System;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    public class RecognizedEventArgs : EventArgs
    {
        public ISpeechRecognitionResult Result { get; }

        public RecognizedEventArgs(ISpeechRecognitionResult propertyParameter)
        {
            this.Result = propertyParameter;
        }
    }

    public delegate void RecognizedEventHandler(ISpeechRecognizer sender, RecognizedEventArgs e);
}
