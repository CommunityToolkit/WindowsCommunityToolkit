﻿using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public delegate void ImageExFailedEventHandler(object sender, ImageExFailedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="ImageEx"/> ImageFailed event.
    /// </summary>
    public class ImageExFailedEventArgs : EventArgs
    {
        public ImageExFailedEventArgs(Exception errorException)
        {
            ErrorException = errorException;
            ErrorMessage = ErrorException?.Message;
        }

        /// <summary>
        /// Gets the exception that caused the error condition.
        /// </summary>
        public Exception ErrorException { get; private set; }

        /// <summary>
        /// Gets the reason for the error condition.
        /// </summary>
        public string ErrorMessage { get; private set; }
    }
}
