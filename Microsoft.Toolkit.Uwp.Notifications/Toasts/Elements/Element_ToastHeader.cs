// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal sealed class Element_ToastHeader : IElement_ToastActivatable, IHaveXmlName, IHaveXmlNamedProperties
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Arguments { get; set; }

        public Element_ToastActivationType ActivationType { get; set; } = Element_ToastActivationType.Foreground;

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
                    throw new InvalidOperationException("AfterActivationBehavior on ToastHeader only supports the Default value.");
                }
            }
        }

        /// <inheritdoc/>
        string IHaveXmlName.Name => "header";

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            yield return new("id", Id);
            yield return new("title", Title);
            yield return new("arguments", Arguments);

            if (ActivationType != Element_ToastActivationType.Foreground)
            {
                yield return new("activationType", ActivationType.ToPascalCaseString());
            }

            yield return new("protocolActivationTargetApplicationPfn", ProtocolActivationTargetApplicationPfn);

            if (AfterActivationBehavior != ToastAfterActivationBehavior.Default)
            {
                yield return new("afterActivationBehavior", AfterActivationBehavior.ToPascalCaseString());
            }
        }
    }
}