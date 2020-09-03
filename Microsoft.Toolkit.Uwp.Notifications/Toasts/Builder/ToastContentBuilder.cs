// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Notifications
{
#if !WINRT
    /// <summary>
    /// Builder class used to create <see cref="ToastContent"/>
    /// </summary>
    public partial class ToastContentBuilder
    {
        /// <summary>
        /// Gets internal instance of <see cref="ToastContent"/>. This is equivalent to the call to <see cref="ToastContentBuilder.GetToastContent"/>.
        /// </summary>
        public ToastContent Content
        {
            get; private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToastContentBuilder"/> class.
        /// </summary>
        public ToastContentBuilder()
        {
            Content = new ToastContent();
        }

        /// <summary>
        /// Add custom time stamp on the toast to override the time display on the toast.
        /// </summary>
        /// <param name="dateTime">Custom Time to be displayed on the toast</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddCustomTimeStamp(DateTime dateTime)
        {
            Content.DisplayTimestamp = dateTime;

            return this;
        }

        /// <summary>
        /// Add a header to a toast.
        /// </summary>
        /// <param name="id">A developer-created identifier that uniquely identifies this header. If two notifications have the same header id, they will be displayed underneath the same header in Action Center.</param>
        /// <param name="title">A title for the header.</param>
        /// <param name="arguments">A developer-defined string of arguments that is returned to the app when the user clicks this header.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        /// <remarks>More info about toast header: https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/toast-headers </remarks>
        public ToastContentBuilder AddHeader(string id, string title, string arguments)
        {
            Content.Header = new ToastHeader(id, title, arguments);

            return this;
        }

        /// <summary>
        /// Add info that can be used by the application when the app was activated/launched by the toast.
        /// </summary>
        /// <param name="launchArgs">Custom app-defined launch arguments to be passed along on toast activation</param>
        /// <param name="activationType">Set the activation type that will be used when the user click on this toast</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddToastActivationInfo(string launchArgs, ToastActivationType activationType = ToastActivationType.Foreground)
        {
            Content.Launch = launchArgs;
            Content.ActivationType = activationType;
            return this;
        }

        /// <summary>
        /// Sets the amount of time the Toast should display. You typically should use the
        /// Scenario attribute instead, which impacts how long a Toast stays on screen.
        /// </summary>
        /// <param name="duration">Duration of the toast</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder SetToastDuration(ToastDuration duration)
        {
            Content.Duration = duration;
            return this;
        }

        /// <summary>
        ///  Sets the scenario, to make the Toast behave like an alarm, reminder, or more.
        /// </summary>
        /// <param name="scenario">Scenario to be used for the toast's behavior</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder SetToastScenario(ToastScenario scenario)
        {
            Content.Scenario = scenario;
            return this;
        }

        /// <summary>
        /// Set custom audio to go along with the toast.
        /// </summary>
        /// <param name="src">Source to the media that will be played when the toast is pop</param>
        /// <param name="loop">Indicating whether sound should repeat as long as the Toast is shown; false to play only once (default).</param>
        /// <param name="silent">Indicating whether sound is muted; false to allow the Toast notification sound to play (default).</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddAudio(Uri src, bool? loop = null, bool? silent = null)
        {
            if (!src.IsFile)
            {
                throw new ArgumentException(nameof(src), "Audio Source has to be a file.");
            }

            Content.Audio = new ToastAudio();
            Content.Audio.Src = src;

            if (loop != null)
            {
                Content.Audio.Loop = loop.Value;
            }

            if (silent != null)
            {
                Content.Audio.Silent = silent.Value;
            }

            return this;
        }

        /// <summary>
        /// Set custom audio to go along with the toast.
        /// </summary>
        /// <param name="audio">The <see cref="ToastAudio"/> to set.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddAudio(ToastAudio audio)
        {
            Content.Audio = audio;
            return this;
        }

        /// <summary>
        /// Get the instance of <see cref="ToastContent"/> that has been built by the builder with specified configuration so far.
        /// </summary>
        /// <returns>An instance of <see cref="ToastContent"/> that can be used to create tile notification.</returns>
        public ToastContent GetToastContent()
        {
            return Content;
        }

#if WINDOWS_UWP
        /// <summary>
        /// Retrieves the notification XML content as a WinRT XmlDocument, so that it can be used with a local Toast notification's constructor on either <see cref="Windows.UI.Notifications.ToastNotification"/> or <see cref="Windows.UI.Notifications.ScheduledToastNotification"/>.
        /// </summary>
        /// <returns>The notification XML content as a WinRT XmlDocument.</returns>
        public Windows.Data.Xml.Dom.XmlDocument GetXml()
        {
            return GetToastContent().GetXml();
        }

        /// <summary>
        /// Shows a new toast notification with the current content.
        /// </summary>
        /// <param name="customize">Allows you to set additional properties on the <see cref="Windows.UI.Notifications.ToastNotification"/> object.</param>
        public void Show(Action<Windows.UI.Notifications.ToastNotification> customize = null)
        {
            var notif = new Windows.UI.Notifications.ToastNotification(GetToastContent().GetXml());
            customize?.Invoke(notif);

            ToastNotificationManagerCompat.CreateToastNotifier().Show(notif);
        }

        /// <summary>
        /// Schedules the notification.
        /// </summary>
        /// <param name="deliveryTime">The date and time that Windows should display the toast notification. This time must be in the future.</param>
        /// <param name="customize">Allows you to set additional properties on the <see cref="Windows.UI.Notifications.ScheduledToastNotification"/> object.</param>
        public void Schedule(DateTimeOffset deliveryTime, Action<Windows.UI.Notifications.ScheduledToastNotification> customize = null)
        {
            var notif = new Windows.UI.Notifications.ScheduledToastNotification(GetToastContent().GetXml(), deliveryTime);
            customize?.Invoke(notif);

            ToastNotificationManagerCompat.CreateToastNotifier().AddToSchedule(notif);
        }
#endif
    }

#endif
}
