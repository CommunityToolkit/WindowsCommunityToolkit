// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace UITests.App.Pages
{
    public sealed partial class ConstrainedBoxTestPage : Page
    {
        public int IntegerWidth { get; set; } = 2;

        public ConstrainedBoxTestPage()
        {
            this.InitializeComponent();
        }
    }
}