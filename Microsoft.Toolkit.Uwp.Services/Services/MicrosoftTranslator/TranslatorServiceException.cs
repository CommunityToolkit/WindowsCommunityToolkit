using System;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator
{
    /// <summary>
    /// The <strong>TranslatorServiceException</strong> class holds information about Exception related to <see cref="TranslatorService"/>.
    /// </summary>
    /// <seealso cref="TranslatorService"/>
    public class TranslatorServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorServiceException"/> class using the specified error message.
        /// <param name="message">The message that describes the error.</param>
        /// </summary>
        public TranslatorServiceException(string message)
            : base(message)
        {
        }
    }
}
