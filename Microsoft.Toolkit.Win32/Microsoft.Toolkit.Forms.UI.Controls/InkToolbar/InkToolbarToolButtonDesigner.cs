// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Windows.Forms.Design;
using Microsoft.Toolkit.Forms.UI.XamlHost;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// Designer for most of the WinForms-wrapped WPF InkToolbar sub-controls such as InkToolbarToolButton
    /// </summary>
    internal class InkToolbarToolButtonDesigner : ControlDesigner
    {
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            if (Component is WindowsXamlHostBase toolbarbutton)
            {
                // Set MinimumSize in the designer, so that the control doesn't go to 0-height
                toolbarbutton.MinimumSize = new System.Drawing.Size(20, 20);
                toolbarbutton.Dock = System.Windows.Forms.DockStyle.Right;
            }
        }
    }
}