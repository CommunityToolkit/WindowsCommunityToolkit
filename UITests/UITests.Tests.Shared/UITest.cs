// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
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
#if USING_TAEF
            var fullTestName = TestContext.TestName;
            var lastDotIndex = fullTestName.LastIndexOf('.');
            var testName = fullTestName.Substring(lastDotIndex + 1);
            var theClassName = fullTestName.Substring(0, lastDotIndex);
#else
            var testName = TestContext.TestName;
            var theClassName = TestContext.FullyQualifiedTestClassName;
#endif
            var currentlyRunningClassType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(f => f.FullName == theClassName);
            if (!(Type.GetType(theClassName) is Type type))
            {
                Verify.Fail("Type is null. TestClassName : " + theClassName);
                return;
            }

            if (!(type.GetMethod(testName) is MethodInfo method))
            {
                Verify.Fail("Mothod is null. TestClassName : " + theClassName + " Testname: " + testName);
                return;
            }

            if (!(method.GetCustomAttribute(typeof(TestPageAttribute), true) is TestPageAttribute attribute))
            {
                Verify.Fail("Attribute is null. TestClassName : " + theClassName);
                return;
            }

            var pageTextBox = FindElement.ById<Edit>("PageName");
            pageTextBox.SetValue(attribute.XamlFile);
            KeyboardHelper.PressKey(Key.Enter);
        }
    }
}
