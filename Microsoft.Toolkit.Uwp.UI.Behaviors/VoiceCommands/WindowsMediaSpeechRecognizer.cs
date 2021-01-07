using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    public class WindowsMediaSpeechRecognizer : ISpeechRecognizer
    {
        private readonly SpeechRecognizer _speechRecognizer;

        private WindowsMediaSpeechRecognizer(SpeechRecognizer speechRecognizer)
        {
            _speechRecognizer = speechRecognizer;
        }

        public static async Task<WindowsMediaSpeechRecognizer> CreateAsync(Windows.UI.Xaml.Window window)
        {
            SpeechRecognizer sr = null;
            foreach (var item in Windows.System.UserProfile.GlobalizationPreferences.Languages)
            {
                var language = new Language(item);
                try
                {
                    sr = new SpeechRecognizer(language);
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            if (sr is null)
            {
                sr = new SpeechRecognizer();
            }

            var d = new WindowsMediaSpeechRecognizer(sr);

            Debug.WriteLine($"SpeechRecognizer Language: { sr.CurrentLanguage.DisplayName}");

            sr.ContinuousRecognitionSession.AutoStopSilenceTimeout = TimeSpan.MaxValue;
            await sr.CompileConstraintsAsync();
            sr.ContinuousRecognitionSession.ResultGenerated += d.ContinuousRecognitionSession_ResultGenerated;
            await sr.ContinuousRecognitionSession.StartAsync();

            window.Activated += d.Window_Activated;

            return d;
        }

        private void Window_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.CodeActivated && _speechRecognizer.State == SpeechRecognizerState.Idle)
            {
                _ = _speechRecognizer.ContinuousRecognitionSession.StartAsync();
            }
        }

        private void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            OnRecognized(new RecognizedEventArgs(new SpeechRecognitionResult(args.Result.Text, args.Result.RawConfidence)));
        }

        public event RecognizedEventHandler Recognized;

        protected virtual void OnRecognized(RecognizedEventArgs args)
        {
            Recognized?.Invoke(this, args);
        }
    }
}
