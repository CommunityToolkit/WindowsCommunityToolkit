// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    internal static class Verify
    {
        // Conditional to use more aggressive fail-fast behaviors when debugging
#if DEV_DEBUG
        private const bool AggressiveFailFast = true;
#else
        private const bool AggressiveFailFast = false;
#endif

        /// <summary>
        /// Verifies that two generic type data are equal.  The assertion fails if they are not.
        /// </summary>
        /// <typeparam name="T">The generic type to compare for equality.</typeparam>
        /// <param name="expected">The first generic type data to compare.  This is is the expected value.</param>
        /// <param name="actual">The second generic type data to compare.  This is the actual value.</param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void AreEqual<T>(T expected, T actual)
        {
#pragma warning disable RECS0017 // Possible compare of value type with 'null'
            if (expected == null)
#pragma warning restore RECS0017 // Possible compare of value type with 'null'
            {
                // Two nulls are considered equal, regardless of type semantics.
#pragma warning disable RECS0017 // Possible compare of value type with 'null'
                if (actual != null && !actual.Equals(expected))
#pragma warning restore RECS0017 // Possible compare of value type with 'null'
                {
                    Break();
                }
            }
            else if (!expected.Equals(actual))
            {
                Break();
            }
        }

        /// <summary>
        /// This line should never be executed.  The assertion always fails.
        /// </summary>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void Fail() => Fail(null);

        /// <summary>
        /// This line should never be executed.  The assertion always fails.
        /// </summary>
        /// <param name="message">The message to display if this function is executed.</param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void Fail(string message) => Break(message);

        /// <summary>
        /// Verifies that if the specified condition is true, then so is the result.
        /// The assertion fails if the condition is true but the result is false.
        /// </summary>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="result">
        /// A second Boolean statement.  If the first was true then so must this be.
        /// If the first statement was false then the value of this is ignored.
        /// </param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void Implies(bool condition, bool result)
        {
            if (condition && !result)
            {
                Break();
            }
        }

        /// <summary>
        /// Verify the current thread's apartment state is what's expected.  The assertion fails if it isn't
        /// </summary>
        /// <param name="expectedState">
        /// The expected apartment state for the current thread.
        /// </param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void IsApartmentState(ApartmentState expectedState)
        {
            if (Thread.CurrentThread.GetApartmentState() != expectedState)
            {
                Break();
            }
        }

        /// <summary>
        /// Verifies that the specified condition is false.  The assertion fails if it is true.
        /// </summary>
        /// <param name="condition">The expression that should be <c>false</c>.</param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void IsFalse(bool condition)
        {
            IsFalse(condition, null);
        }

        /// <summary>
        /// Verifies that the specified condition is false.  The assertion fails if it is true.
        /// </summary>
        /// <param name="condition">The expression that should be <c>false</c>.</param>
        /// <param name="message">The message to display if the condition is <c>true</c>.</param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                Break(message);
            }
        }

        /// <summary>
        /// Verifies that a string has content.  I.e. it is not null and it is not empty.
        /// </summary>
        /// <param name="value">The string to verify.</param>
        [Conditional("DEBUG")]
        internal static void IsNeitherNullNorEmpty(string value) => IsFalse(string.IsNullOrEmpty(value));

        /// <summary>
        /// Verifies that a string has content.  I.e. it is not null and it is not purely whitespace.
        /// </summary>
        /// <param name="value">The string to verify.</param>
        [Conditional("DEBUG")]
        internal static void IsNeitherNullNorWhitespace(string value) => IsFalse(string.IsNullOrWhiteSpace(value));

        /// <summary>
        /// Verifies the specified value is not null.  The assertion fails if it is.
        /// </summary>
        /// <typeparam name="T">The generic reference type.</typeparam>
        /// <param name="value">The value to check for nullness.</param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void IsNotNull<T>(T value)
            where T : class
        {
            if (value == null)
            {
                Break();
            }
        }

        [Conditional("DEBUG")]
        internal static void IsNotOnMainThread()
        {
            if (System.Windows.Application.Current.Dispatcher.CheckAccess())
            {
                Break("Is not on WPF main thread");
            }
        }

        /// <summary>
        /// Verifies that the specified object is null.  The assertion fails if it is not.
        /// </summary>
        /// <typeparam name="T">The type of item to verify.</typeparam>
        /// <param name="item">The item to verify is null.</param>
        [Conditional("DEBUG")]
        internal static void IsNull<T>(T item)
            where T : class
        {
            if (item != null)
            {
                Break();
            }
        }

        /// <summary>
        /// Verifies that the specified condition is true.  The assertion fails if it is not.
        /// </summary>
        /// <param name="condition">A condition that is expected to be <c>true</c>.</param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void IsTrue(bool condition) => IsTrue(condition, null);

        /// <summary>
        /// Verifies that the specified condition is true.  The assertion fails if it is not.
        /// </summary>
        /// <param name="condition">A condition that is expected to be <c>true</c>.</param>
        /// <param name="message">The message to write in case the condition is <c>false</c>.</param>
        /// <remarks>This breaks into the debugger in the case of a failed assertion.</remarks>
        [Conditional("DEBUG")]
        internal static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                Break(message);
            }
        }

        [Conditional("DEBUG")]
        private static void Break(string message = null)
        {
            // If we're running under a unit test, do some additional checking
            // Since this method can only be called in DEBUG builds, the additional
            // cost of enumerating assemblies or walking the stack is forgivable
            if (IsRunningInUnitTest)
            {
                if (Debugger.IsAttached)
                {
                    // Since a debugger is attached use the existing behavior
                    _Break(message);
                }
                else
                {
                    throw new InvalidOperationException(message ?? "Code encountered a BREAK condition and cannot continue. Review stack for more information.");
                }
            }
            else
            {
                _Break(message);
            }
        }

        [Conditional("DEBUG")]
#pragma warning disable SA1300 // Element must begin with upper-case letter
        private static void _Break(string message)
#pragma warning restore SA1300 // Element must begin with upper-case letter
        {
#pragma warning disable RECS0110 // Condition is set by DEV_DEBUG compile constant
            if (AggressiveFailFast)
#pragma warning restore RECS0110 // Condition is set by DEV_DEBUG compile constant
            {
#pragma warning disable 162
                if (!string.IsNullOrEmpty(message))
                {
                    Debug.WriteLine(message);
                }

                Debugger.Break();
#pragma warning restore 162
            }
            else
#pragma warning disable 162
            {
                Debug.Assert(false, message);
            }
#pragma warning restore 162
        }

        private static readonly HashSet<string> UnitTestAttributes = new HashSet<string>
        {
            "Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute"
        };

        private static readonly HashSet<string> UnitTestFrameworks = new HashSet<string>
        {
            "Microsoft.VisualStudio.QualityTools.UnitTestFramework",
            "Microsoft.VisualStudio.TestPlatform.TestFramework"
        };

        private static bool IsRunningInUnitTest
        {
            get
            {
                // Check if the current AppDomain has loaded a unit test assembly
                bool AppDomainHasUnitTestAssembly()
                {
                    return AppDomain
                           .CurrentDomain
                           .GetAssemblies()
                           .Any(AssemblyIsUnitTestFramework);
                }

                bool AssemblyIsUnitTestFramework(Assembly a)
                {
                    foreach (var f in UnitTestFrameworks)
                    {
                        if (a.FullName.StartsWith(f, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                // Walk the stack and determine if any type has a known attribute for unit tests
                bool TypeHasUnitTestAttribute()
                {
                    return new StackTrace()
                           .GetFrames()
                           .Any(f => f
                                     .GetMethod()
                                     .DeclaringType
                                     .GetCustomAttributes(false)
                                     .Any(x => UnitTestAttributes.Contains(x.GetType().FullName)));
                }

                return AppDomainHasUnitTestAssembly() || TypeHasUnitTestAttribute();
            }
        }
    }
}