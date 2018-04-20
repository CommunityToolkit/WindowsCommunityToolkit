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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The Stack mode of an in-app notification.
    /// </summary>
    public enum StackMode
    {
        /// <summary>
        /// Each notification will replace the previous one
        /// </summary>
        Replace,

        /// <summary>
        /// Opening a notification will display it immediately, remaining notifications will appear when a notification is dismissed
        /// </summary>
        StackInFront,

        /// <summary>
        /// Dismissing a notification will show the next one in the queue
        /// </summary>
        QueueBehind
    }
}
