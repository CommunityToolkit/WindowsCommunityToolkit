// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    internal class InkToolbarDesigner : ParentControlDesigner
    {
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            var toolbar = (InkToolbar)Component;
            if (toolbar != null)
            {
                // Set MinimumSize in the designer, so that the control doesn't go to 0-height
                toolbar.MinimumSize = new System.Drawing.Size(20, 60);
                toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            }
        }
    }
}