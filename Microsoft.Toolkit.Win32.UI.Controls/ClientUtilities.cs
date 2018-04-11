// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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