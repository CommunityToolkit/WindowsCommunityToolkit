// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal sealed class Element_Toast : BaseElement, IElement_ToastActivatable, IHaveXmlAdditionalProperties, IHaveXmlName, IHaveXmlNamedProperties, IHaveXmlChildren
    {
        internal const ToastScenario DEFAULT_SCENARIO = ToastScenario.Default;
        internal const Element_ToastActivationType DEFAULT_ACTIVATION_TYPE = Element_ToastActivationType.Foreground;
        internal const ToastDuration DEFAULT_DURATION = ToastDuration.Short;

        public Element_ToastActivationType ActivationType { get; set; } = DEFAULT_ACTIVATION_TYPE;

        public string ProtocolActivationTargetApplicationPfn { get; set; }

        public ToastAfterActivationBehavior AfterActivationBehavior
        {
            get
            {
                return ToastAfterActivationBehavior.Default;
            }

            set
            {
                if (value != ToastAfterActivationBehavior.Default)
                {
                    throw new InvalidOperationException("AfterActivationBehavior on ToastContent only supports the Default value.");
                }
            }
        }

        public ToastDuration Duration { get; set; } = DEFAULT_DURATION;

        public string Launch { get; set; }

        public ToastScenario Scenario { get; set; } = DEFAULT_SCENARIO;

        public DateTimeOffset? DisplayTimestamp { get; set; }

        public Element_ToastVisual Visual { get; set; }

        public Element_ToastAudio Audio { get; set; }

        public Element_ToastActions Actions { get; set; }

        public Element_ToastHeader Header { get; set; }

        public string HintToastId { get; set; }

        public string HintPeople { get; set; }

        public IReadOnlyDictionary<string, string> AdditionalProperties { get; set; }

        public static Element_ToastActivationType ConvertActivationType(ToastActivationType publicType)
        {
            switch (publicType)
            {
                case ToastActivationType.Foreground:
                    return Element_ToastActivationType.Foreground;

                case ToastActivationType.Background:
                    return Element_ToastActivationType.Background;

                case ToastActivationType.Protocol:
                    return Element_ToastActivationType.Protocol;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        string IHaveXmlName.Name => "toast";

        /// <inheritdoc/>
        IEnumerable<object> IHaveXmlChildren.Children => new object[] { Visual, Audio, Actions, Header };

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            if (ActivationType != DEFAULT_ACTIVATION_TYPE)
            {
                yield return new("activationType", ActivationType.ToPascalCaseString());
            }

            yield return new("protocolActivationTargetApplicationPfn", ProtocolActivationTargetApplicationPfn);

            if (AfterActivationBehavior != ToastAfterActivationBehavior.Default)
            {
                yield return new("afterActivationBehavior", AfterActivationBehavior.ToPascalCaseString());
            }

            if (Duration != DEFAULT_DURATION)
            {
                yield return new("duration", Duration.ToPascalCaseString());
            }

            yield return new("launch", Launch);

            if (Scenario != DEFAULT_SCENARIO)
            {
                yield return new("scenario", Scenario.ToPascalCaseString());
            }

            yield return new("displayTimestamp", DisplayTimestamp);
            yield return new("hint-toastId", HintToastId);
            yield return new("hint-people", HintPeople);
        }
    }

    /// <summary>
    /// The amount of time the Toast should display.
    /// </summary>
    public enum ToastDuration
    {
        /// <summary>
        /// Default value. Toast appears for a short while and then goes into Action Center.
        /// </summary>
        Short,

        /// <summary>
        /// Toast stays on-screen for longer, and then goes into Action Center.
        /// </summary>
        Long
    }

    /// <summary>
    /// Specifies the scenario, controlling behaviors about the Toast.
    /// </summary>
    public enum ToastScenario
    {
        /// <summary>
        /// The normal Toast behavior. The Toast appears for a short duration, and then automatically dismisses into Action Center.
        /// </summary>
        Default,

        /// <summary>
        /// Causes the Toast to stay on-screen and expanded until the user takes action. Also causes a looping alarm sound to be selected by default.
        /// </summary>
        Alarm,

        /// <summary>
        /// Causes the Toast to stay on-screen and expanded until the user takes action.
        /// </summary>
        Reminder,

        /// <summary>
        /// Causes the Toast to stay on-screen and expanded until the user takes action (on Mobile this expands to full screen). Also causes a looping incoming call sound to be selected by default.
        /// </summary>
        IncomingCall
    }
}