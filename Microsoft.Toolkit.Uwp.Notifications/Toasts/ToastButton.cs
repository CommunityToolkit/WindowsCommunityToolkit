// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// A button that the user can click on a Toast notification.
    /// </summary>
    public sealed class ToastButton : IToastButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToastButton"/> class.
        /// </summary>
        /// <param name="content">The text to display on the button.</param>
        /// <param name="arguments">App-defined string of arguments that the app can later retrieve once it is activated when the user clicks the button.</param>
        public ToastButton(string content, string arguments)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            Content = content;
            Arguments = arguments;
        }

        /// <summary>
        /// Gets the text to display on the button. Required
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Gets app-defined string of arguments that the app can later retrieve once it is
        /// activated when the user clicks the button. Required
        /// </summary>
        public string Arguments { get; private set; }

        /// <summary>
        /// Gets or sets what type of activation this button will use when clicked. Defaults to Foreground.
        /// </summary>
        public ToastActivationType ActivationType { get; set; } = ToastActivationType.Foreground;

        /// <summary>
        /// Gets or sets additional options relating to activation of the toast button. New in Creators Update
        /// </summary>
        public ToastActivationOptions ActivationOptions { get; set; }

        /// <summary>
        /// Gets or sets an optional image icon for the button to display (required for buttons adjacent to inputs like quick reply).
        /// </summary>
        public string ImageUri { get; set; }

        /// <summary>
        /// Gets or sets the ID of an existing <see cref="ToastTextBox"/> in order to have this button display
        /// to the right of the input, achieving a quick reply scenario.
        /// </summary>
        public string TextBoxId { get; set; }

        /// <summary>
        /// Gets or sets an identifier used in telemetry to identify your category of action. This should be something
        /// like "Delete", "Reply", or "Archive". In the upcoming toast telemetry dashboard in Dev Center, you will
        /// be able to view how frequently your actions are being clicked.
        /// </summary>
        public string HintActionId { get; set; }

        internal Element_ToastAction ConvertToElement()
        {
            var el = new Element_ToastAction()
            {
                Content = Content,
                Arguments = Arguments,
                ActivationType = Element_Toast.ConvertActivationType(ActivationType),
                ImageUri = ImageUri,
                InputId = TextBoxId,
                HintActionId = HintActionId
            };

            ActivationOptions?.PopulateElement(el);

            return el;
        }
    }
}
