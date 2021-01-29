// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// This class reports a recognized speech
    /// </summary>
    public class SpeechRecognitionResult : ISpeechRecognitionResult
    {
        /// <summary>
        /// Gets the Text of the recognized speech
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the RawConfidence of the recognized speech
        /// </summary>
        public double RawConfidence { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechRecognitionResult"/> class.
        /// </summary>
        /// <param name="text">the speech recognized text</param>
        /// <param name="rawConfidence">the rawConfidence of recognized speech</param>
        public SpeechRecognitionResult(string text, double rawConfidence)
        {
            Text = text;
            RawConfidence = rawConfidence;
        }
    }
}
