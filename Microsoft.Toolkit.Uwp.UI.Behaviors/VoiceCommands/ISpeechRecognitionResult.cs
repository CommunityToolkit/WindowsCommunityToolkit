// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
