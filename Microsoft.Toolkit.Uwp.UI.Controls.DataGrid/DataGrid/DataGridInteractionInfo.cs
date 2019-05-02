// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls.DataGridInternals
{
    internal class DataGridInteractionInfo
    {
        internal uint CapturedPointerId
        {
            get;
            set;
        }

        internal bool IsPointerOver
        {
            get;
            set;
        }
    }
}