// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Notifications
{
#if !WINRT
    public partial class ToastContentBuilder
    {
        public ToastContent Content
        {
            get; private set;
        }

        public ToastContentBuilder()
        {
            Content = new ToastContent();
        }

        public ToastContentBuilder AddCustomTimeStamp(DateTime dateTime)
        {
            Content.DisplayTimestamp = dateTime;

            return this;
        }

        public ToastContentBuilder AddHeader(string id, string title, string arguments)
        {
            Content.Header = new ToastHeader(id, title, arguments);

            return this;
        }

        public ToastContentBuilder AddToastActivationInfo(string launchArgs, ToastActivationType activationType)
        {
            Content.Launch = launchArgs;
            Content.ActivationType = activationType;
            return this;
        }

        public ToastContentBuilder SetToastDuration(ToastDuration duration)
        {
            Content.Duration = duration;
            return this;
        }

        public ToastContentBuilder SetToastScenario(ToastScenario scenario)
        {
            Content.Scenario = scenario;
            return this;
        }

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

        public ToastContent GetToastContent()
        {
            return Content;
        }
    }

#endif
}
