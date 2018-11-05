// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Serialization
{
    /// <summary>
    /// Exception thrown to indicate a problem reading a Lottie composition.
    /// </summary>
    sealed class LottieCompositionReaderException : Exception
    {
        public LottieCompositionReaderException()
        {
        }

        public LottieCompositionReaderException(string message) : base(message)
        {
        }

        public LottieCompositionReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
