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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Indicated if the animation should be restarted from the begining or if it should reverse, when reaching it's end.
    /// </summary>
    public enum RepeatMode
    {
        /// <summary>
        /// When the animation reaches the end and <see cref="LottieDrawable.RepeatCount"/> is INFINITE
        /// or a positive value, the animation restarts from the beginning.
        /// </summary>
        Restart = 1,

        /// <summary>
        /// When the animation reaches the end and <see cref="LottieDrawable.RepeatCount"/> is INFINITE
        /// or a positive value, the animation reverses direction on every iteration.
        /// </summary>
        Reverse = 2
    }
}
