// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView
{
    [TestClass]
    public class AssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            context.WriteLine("AssemblyInit");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }
    }
}
