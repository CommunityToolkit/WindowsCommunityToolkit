// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

#pragma warning disable CS8618

namespace Microsoft.Toolkit.Mvvm.Messaging.Messages
{
    /// <summary>
    /// A <see langword="class"/> for request messages, which can either be used directly or through derived classes.
    /// </summary>
    /// <typeparam name="T">The type of request to make.</typeparam>
    public class RequestMessage<T>
    {
        /// <summary>
        /// An <see cref="object"/> used to synchronize access to <see cref="Result"/> and <see cref="ReportResult"/>.
        /// </summary>
        private readonly object dummy = new object();

        private T result;

        /// <summary>
        /// Gets the message response.
        /// </summary>
        public T Result
        {
            get
            {
                lock (this.dummy)
                {
                    if (!this.IsResponseReceived)
                    {
                        ThrowInvalidOperationExceptionForDuplicateReporResult();
                    }

                    return this.result;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether a result has already been assigned to this instance.
        /// </summary>
        public bool IsResponseReceived { get; private set; }

        /// <summary>
        /// Reports a result for the current request message.
        /// </summary>
        /// <param name="result">The result to report for the message.</param>
        public void ReportResult(T result)
        {
            lock (this.dummy)
            {
                if (this.IsResponseReceived)
                {
                    ThrowInvalidOperationExceptionForDuplicateReporResult();
                }

                this.IsResponseReceived = true;
                this.result = result;
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when <see cref="ReportResult"/> is called twice.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidOperationExceptionForDuplicateReporResult()
        {
            throw new InvalidOperationException("A result has already been reported for the current message");
        }
    }
}
