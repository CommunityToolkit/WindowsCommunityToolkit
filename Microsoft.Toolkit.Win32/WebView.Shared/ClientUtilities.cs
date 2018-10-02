// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;
using System.Threading;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    internal static class ClientUtilities
    {
        internal static bool IsCriticalException(this Exception ex)
        {
            return ex is NullReferenceException
                   || ex is StackOverflowException
                   || ex is OutOfMemoryException
                   || ex is ThreadAbortException
                   || ex is IndexOutOfRangeException
                   || ex is AccessViolationException;
        }

        internal static bool IsSecurityOrCriticalException(this Exception exception)
        {
            return exception is SecurityException || IsCriticalException(exception);
        }
    }
}