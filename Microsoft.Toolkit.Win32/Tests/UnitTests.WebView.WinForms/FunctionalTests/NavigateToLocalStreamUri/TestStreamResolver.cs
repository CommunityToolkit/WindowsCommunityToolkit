// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System.IO;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.NavigateToLocalStreamUri
{
#pragma warning disable 618
    internal class TestStreamResolver : UriToLocalStreamResolver
#pragma warning restore 618
    {
        public TestStreamResolver()
            :base(Path.GetDirectoryName(typeof(TestStreamResolver).Assembly.Location))
        {            
        }        
    }
}
