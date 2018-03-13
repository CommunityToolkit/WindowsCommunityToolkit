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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value
{
    /// <summary>
    /// Delegate interface for <see cref="LottieValueCallback{T}"/>. This is helpful for the Kotlin API because you can use a SAM conversion to write the
    /// callback as a single abstract method block like this:
    /// animationView.AddValueCallback(keyPath, LottieProperty.TransformOpacity) { 50 }
    /// </summary>
    /// <typeparam name="T">The type that the callback will act on.</typeparam>
    /// <param name="frameInfo">The information of this frame, which this callback wants to change</param>
    /// <returns>Return the appropriate value that it wants to change.</returns>
    public delegate T SimpleLottieValueCallback<T>(LottieFrameInfo<T> frameInfo);
}
