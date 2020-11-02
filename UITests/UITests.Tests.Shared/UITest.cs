// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Common;

#if USING_TAEF
using WEX.Logging.Interop;
using WEX.TestExecution;
using WEX.TestExecution.Markup;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace UITests.Tests
{
    public abstract class UITest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitalize()
        {
            Log.Comment("Running Test: " + TestContext.TestName);

            if (GetType().GetMethod(TestContext.TestName) is MethodInfo method
                && method.GetCustomAttribute(typeof(TestPageAttribute), true) is TestPageAttribute xamlFileAttribute)
            {
                OpenTest(xamlFileAttribute.XamlFile);
            }
        }

        private static void OpenTest(string name)
        {
            Log.Comment("Opening: " + name);
            var btn = new Button(FindElement.ByName(name));
            Verify.IsNotNull(btn);
            btn.Click();
            Wait.ForIdle();
        }
    }
}
