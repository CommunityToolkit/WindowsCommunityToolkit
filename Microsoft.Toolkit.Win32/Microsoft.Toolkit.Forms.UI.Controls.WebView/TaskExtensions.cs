// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Forms;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Contains extensions to process a task within a nested Windows Forms message loop
    /// </summary>
    internal static partial class TaskExtensions
    {
        public static T WaitWithNestedMessageLoop<T>(this Task<T> task)
        {
            while (!task.IsCompleted)
            {
                Application.DoEvents();
            }

            return task.Result;
        }
    }
}