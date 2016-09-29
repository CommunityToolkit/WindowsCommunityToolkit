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

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// States of AnimationSet.
    /// </summary>
    public enum AnimationSetState
    {
        /// <summary>
        /// The animation has not been started
        /// </summary>
        NotStarted,

        /// <summary>
        /// The animation has been started and is in progress
        /// </summary>
        Running,

        /// <summary>
        /// The animation has been started and is stopped
        /// </summary>
        Stopped,

        /// <summary>
        /// The animation had completed
        /// </summary>
        Completed
    }
}
