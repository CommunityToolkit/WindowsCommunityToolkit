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

namespace Microsoft.Toolkit.Uwp.UI.Helpers
{
    /// <summary>
    /// This interface represents a task, which exposes properties for binding.
    /// </summary>
    /// <seealso cref="TaskWithNotification{TResult}"/>
    public interface ITaskWithNotification
    {
        /// <summary>
        /// Gets a value indicating whether task was canceled
        /// </summary>
        bool IsCanceled { get; }

        /// <summary>
        /// Gets a value indicating whether task is completed
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets a value indicating whether task ended with exception
        /// </summary>
        bool IsFaulted { get; }

        /// <summary>
        /// Gets a value indicating whether task is still running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gets a value indicating whether task has been started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Gets a value indicating whether task has successfully ended
        /// </summary>
        bool IsSuccessfull { get; }

        /// <summary>
        /// Gets a value representing result of the task
        /// </summary>
        object Result { get; }
    }
}