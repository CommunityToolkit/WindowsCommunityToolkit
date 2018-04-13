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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Types of swipe status.
    /// </summary>
    [Obsolete("The SwipeStatus will be removed alongside SlidableListItem in a future major release. Please use the SwipeControl available in the Fall Creators Update")]
    public enum SwipeStatus
    {
        /// <summary>
        /// Swiping is not occurring.
        /// </summary>
        Idle,

        /// <summary>
        /// Swiping is going to start.
        /// </summary>
        Starting,

        /// <summary>
        /// Swiping to the left, but the command is disabled.
        /// </summary>
        DisabledSwipingToLeft,

        /// <summary>
        /// Swiping to the left below the threshold.
        /// </summary>
        SwipingToLeftThreshold,

        /// <summary>
        /// Swiping to the left above the threshold.
        /// </summary>
        SwipingPassedLeftThreshold,

        /// <summary>
        /// Swiping to the right, but the command is disabled.
        /// </summary>
        DisabledSwipingToRight,

        /// <summary>
        /// Swiping to the right below the threshold.
        /// </summary>
        SwipingToRightThreshold,

        /// <summary>
        /// Swiping to the right above the threshold.
        /// </summary>
        SwipingPassedRightThreshold
    }
}
