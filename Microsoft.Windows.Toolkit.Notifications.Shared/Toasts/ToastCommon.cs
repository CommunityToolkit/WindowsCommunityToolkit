// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved



namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// Decides the type of activation that will be used when the user interacts with the Toast notification.
    /// </summary>
    public enum ToastActivationType
    {
        /// <summary>
        /// Default value. Your foreground app is launched.
        /// </summary>
        Foreground,

        /// <summary>
        /// Your corresponding background task (assuming you set everything up) is triggered, and you can execute code in the background (like sending the user's quick reply message) without interrupting the user.
        /// </summary>
        [EnumString("background")]
        Background,

        /// <summary>
        /// Launch a different app using protocol activation.
        /// </summary>
        [EnumString("protocol")]
        Protocol
    }
}
