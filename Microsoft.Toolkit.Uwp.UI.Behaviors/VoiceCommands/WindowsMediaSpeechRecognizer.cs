// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// This class used the Windows <see cref="SpeechRecognizer"/> to recognize speech for the <see cref="VoiceCommandTrigger"/>
    /// </summary>
    public class WindowsMediaSpeechRecognizer : ISpeechRecognizer
    {
        private readonly SpeechRecognizer _speechRecognizer;

        private WindowsMediaSpeechRecognizer(SpeechRecognizer speechRecognizer)
        {
            _speechRecognizer = speechRecognizer;
        }

        /// <summary>
        /// Creates a <see cref="WindowsMediaSpeechRecognizer"/> which can be used for a <see cref="VoiceCommandTrigger"/>
        /// </summary>
        /// <param name="window">The <see cref="Window" /> is used for triggering the Activated event. This will restart the speech recongition.</param>
        /// <returns>The created <see cref="WindowsMediaSpeechRecognizer"/></returns>
        public static async Task<WindowsMediaSpeechRecognizer> CreateAsync(Window window)
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
            Debug.WriteLine($"SpeechRecognizer Language: {sr.CurrentLanguage.DisplayName}");

            sr.ContinuousRecognitionSession.AutoStopSilenceTimeout = TimeSpan.MaxValue;
            await sr.CompileConstraintsAsync();
            sr.ContinuousRecognitionSession.ResultGenerated += d.ContinuousRecognitionSession_ResultGenerated;
            try
            {
                await sr.ContinuousRecognitionSession.StartAsync();
                window.Activated += d.Window_Activated;
                return d;
            }
            catch
            {
                return null;
            }
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

        /// <summary>
        /// Occurs when a speech is recognized
        /// </summary>
        public event RecognizedEventHandler Recognized;

        /// <summary>
        /// Called when speech is recognized
        /// </summary>
        /// <param name="args"><see cref="RecognizedEventArgs"/> used to report the <see cref="ISpeechRecognitionResult"/></param>
        protected virtual void OnRecognized(RecognizedEventArgs args)
        {
            Recognized?.Invoke(this, args);
        }
    }
}
