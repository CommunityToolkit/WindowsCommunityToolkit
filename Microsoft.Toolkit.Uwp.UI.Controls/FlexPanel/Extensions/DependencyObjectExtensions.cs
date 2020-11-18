// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See the LICENSE file in the project root
// for the license information.
//
// Author(s):
//  - Laurent Sansonetti (native Xamarin flex https://github.com/xamarin/flex)
//  - Stephane Delcroix (.NET port)
//  - Ben Askren (UWP/Uno port)
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.FlexPanelExtensions
{
    internal static class DependencyObjectExtensions
    {
        public static void SetNewValue(this DependencyObject obj, DependencyProperty dp, object value)
        {
            var currentValue = obj.GetValue(dp);
            if ((currentValue is null && !(value is null)) || !currentValue.Equals(value))
            {
                obj.SetValue(dp, value);
            }
        }
    }
}