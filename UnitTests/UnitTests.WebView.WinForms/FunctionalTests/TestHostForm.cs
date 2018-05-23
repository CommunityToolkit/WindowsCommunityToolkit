// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Forms;
using WindowsInput;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests
{
    public class TestHostForm : Form
    {
        public InputSimulator InputSimulator { get; }

        public TestHostForm()
        {
            InputSimulator = new InputSimulator();
        }
    }
}