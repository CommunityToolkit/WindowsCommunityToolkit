// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Mvvm.ComponentModel;

namespace UnitTests.NetStandard
{
    /// <summary>
    /// See https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/4167.
    /// This model in particular is loaded from an external .NET Standard 2.0 assembly.
    /// </summary>
    [INotifyPropertyChanged]
    public partial class SampleModelWithINPCAndObservableProperties
    {
        [ObservableProperty]
        private int x;

        [ObservableProperty]
        private int y;
    }
}
