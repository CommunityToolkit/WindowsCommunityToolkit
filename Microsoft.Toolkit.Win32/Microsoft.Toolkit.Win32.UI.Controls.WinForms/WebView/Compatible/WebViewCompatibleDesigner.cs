// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.ComponentModel;
using System.Security;
using System.Windows.Forms.Design;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <inheritdoc />
    internal class WebViewCompatibleDesigner : ControlDesigner
    {
        public string Source
        {
            get => ((Uri)ShadowProperties[nameof(WebViewCompatible.Source)]).OriginalString;
            set => ShadowProperties[nameof(WebViewCompatible.Source)] = new Uri(value);
        }

        protected override InheritanceAttribute InheritanceAttribute
        {
            [SecurityCritical]
            get
            {
                if (base.InheritanceAttribute == InheritanceAttribute.Inherited)
                {
                    return InheritanceAttribute.InheritedReadOnly;
                }

                return base.InheritanceAttribute;
            }
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            var webView = (WebViewCompatible)Component;
            if (webView != null)
            {
                // Set MinimumSize in the designer, so that the control doesn't go to 0-height
                webView.MinimumSize = new System.Drawing.Size(20, 20);
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            // Handle shadowed properties
            var shadowProps = new[]
            {
                nameof(WebViewCompatible.Source),
            };
            PropertyDescriptor prop;
            Attribute[] empty = new Attribute[0];

            for (var i = 0; i < shadowProps.Length; i++)
            {
                prop = (PropertyDescriptor)properties[shadowProps[i]];
                if (prop != null)
                {
                    properties[shadowProps[i]] = TypeDescriptor.CreateProperty(typeof(WebViewDesigner), prop, empty);
                }
            }
        }
    }
}