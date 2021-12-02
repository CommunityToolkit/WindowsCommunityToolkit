// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml;

namespace CommunityToolkit.WinUI.SampleApp.SamplePages
{
    public sealed partial class AttachedDropShadowPage : IXamlRenderListener
    {
        public AttachedDropShadowPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            // This is done as we don't have x:Bind in live xaml, so we find and attach after.
            var castToTarget = control.FindChild("ShadowTarget");
            if (castToTarget != null)
            {
                if (control.Resources.TryGetValue("CommonShadow", out var resource) && resource is AttachedDropShadow shadow)
                {
                    shadow.CastTo = castToTarget;
                }
            }
        }
    }
}