// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Security;
using System.Windows.Forms.Design;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    class WebViewDesigner : ControlDesigner
    {
        public Uri Source
        {
            get => (Uri)ShadowProperties[nameof(WebView.Source)];
            set => ShadowProperties[nameof(WebView.Source)] = value;
        }

        public bool IsScriptNotifyAllowed
        {
            get => (bool) ShadowProperties[nameof(WebView.IsScriptNotifyAllowed)];
            set => ShadowProperties[nameof(WebView.IsScriptNotifyAllowed)] = value;
        }

        public bool IsJavaScriptEnabled
        {
            get => (bool)ShadowProperties[nameof(WebView.IsJavaScriptEnabled)];
            set => ShadowProperties[nameof(WebView.IsJavaScriptEnabled)] = value;
        }

        public bool IsIndexDBEnabled
        {
            get => (bool)ShadowProperties[nameof(WebView.IsIndexedDBEnabled)];
            set => ShadowProperties[nameof(WebView.IsIndexedDBEnabled)] = value;
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

            var webView = (WebView)Component;
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
                nameof(WebView.Source),
                nameof(WebView.IsScriptNotifyAllowed),
                nameof(WebView.IsJavaScriptEnabled),
                nameof(WebView.IsIndexedDBEnabled)
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
