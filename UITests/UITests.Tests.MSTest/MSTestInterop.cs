// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace UITests.Tests
{
    // TAEF has a different terms for the same concepts as compared with MSTest.
    // In order to allow both to use the same test files, we'll define these helper classes
    // to translate TAEF into MSTest.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Shim helpers")]
    public static class Log
    {
        public static void Comment(string format, params object[] args)
        {
            LogMessage(format, args);
        }

        public static void Warning(string format, params object[] args)
        {
            LogMessage("[Warning] " + format, args);
        }

        public static void Error(string format, params object[] args)
        {
            LogMessage("[Error] " + format, args);
        }

        private static void LogMessage(string format, object[] args)
        {
            // string.Format() complains if we pass it something with braces, even if we have no arguments.
            // To account for that, we'll escape braces if we have no arguments.
            if (args.Length == 0)
            {
                format = format.Replace("{", "{{").Replace("}", "}}");
            }

            Logger.LogMessage(format, args);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Shim helpers.")]
    public static class LogController
    {
        public static void InitializeLogging()
        {
            // TODO: implement
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Shim helpers.")]
    public static class Verify
    {
        // TODO: implement
        // NOTE: At the time of this comment, MUXControlsTestApp is hitting a code path where
        //       running tests locally will set this flag to true. Hence an implementation of this flag
        //       will only log errors and not fail the tests.
        public static bool DisableVerifyFailureExceptions
        {
            get;
            set;
        }

        public static void AreEqual(object expected, object actual, string message = null)
        {
            Assert.AreEqual(expected, actual, message);
        }

        public static void AreEqual<T>(T expected, T actual, string message = null)
        {
            Assert.AreEqual(expected, actual, message);
        }

        public static void AreNotEqual(object notExpected, object actual, string message = null)
        {
            Assert.AreNotEqual(notExpected, actual, message);
        }

        public static void AreNotEqual<T>(T notExpected, T actual, string message = null)
        {
            Assert.AreNotEqual(notExpected, actual, message);
        }

        public static void AreSame(object expected, object actual, string message = null)
        {
            Assert.AreSame(expected, actual, message);
        }

        public static void AreNotSame(object notExpected, object actual, string message = null)
        {
            Assert.AreNotSame(notExpected, actual, message);
        }

        public static void IsLessThan(IComparable expectedLess, IComparable expectedGreater, string message = null)
        {
            Assert.IsTrue(expectedLess.CompareTo(expectedGreater) < 0, message);
        }

        public static void IsLessThanOrEqual(IComparable expectedLess, IComparable expectedGreater, string message = null)
        {
            Assert.IsTrue(expectedLess.CompareTo(expectedGreater) <= 0, message);
        }

        public static void IsGreaterThan(IComparable expectedGreater, IComparable expectedLess, string message = null)
        {
            Assert.IsTrue(expectedGreater.CompareTo(expectedLess) > 0, message);
        }

        public static void IsGreaterThanOrEqual(IComparable expectedGreater, IComparable expectedLess, string message = null)
        {
            Assert.IsTrue(expectedGreater.CompareTo(expectedLess) >= 0, message);
        }

        public static void IsNull(object value, string message = null)
        {
            Assert.IsNull(value, message);
        }

        public static void IsNotNull(object value, string message = null)
        {
            Assert.IsNotNull(value, message);
        }

        public static void IsTrue(bool condition, string message = null)
        {
            Assert.IsTrue(condition, message);
        }

        public static void IsFalse(bool condition, string message = null)
        {
            Assert.IsFalse(condition, message);
        }

        public static void Fail()
        {
            Assert.Fail();
        }

        public static void Fail(string message, params object[] args)
        {
            Assert.Fail(message, args);
        }

        public static void Throws<T>(Action action, string message)
            where T : Exception
        {
            Assert.ThrowsException<T>(action, message);
        }

        public static void Throws<T>(Action action)
            where T : Exception
        {
            Assert.ThrowsException<T>(action);
        }
    }
}