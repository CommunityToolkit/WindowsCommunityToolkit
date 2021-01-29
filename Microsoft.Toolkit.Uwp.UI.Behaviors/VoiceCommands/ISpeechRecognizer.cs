// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
