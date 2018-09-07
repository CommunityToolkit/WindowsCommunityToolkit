// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Toolkit.UI.Controls;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    public interface IWebViewCompatibleAdapter : IWebViewCompatible, IDisposable
    {
        FrameworkElement View { get; }

        void Initialize();
    }
}
