// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// <see cref="EventArgs"/> used to report the recognized speech result.
    /// </summary>
    public class RecognizedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the result of a recognized speech
        /// </summary>
        public ISpeechRecognitionResult Result { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecognizedEventArgs"/> class.
        /// </summary>
        /// <param name="result">The result which is speech recognized</param>
        public RecognizedEventArgs(ISpeechRecognitionResult result)
        {
            this.Result = result;
        }
    }

    /// <summary>
    /// The Delegate for a Recognized Event.
    /// </summary>
    /// <param name="sender">Sender ThemeListener</param>
    /// <param name="e">The event arguments.</param>
    public delegate void RecognizedEventHandler(ISpeechRecognizer sender, RecognizedEventArgs e);
}
