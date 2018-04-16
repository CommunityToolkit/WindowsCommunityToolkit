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
    internal class SimpleImplLottieValueCallback<T> : LottieValueCallback<T>
    {
        private readonly SimpleLottieValueCallback<T> _callback;

        public SimpleImplLottieValueCallback(SimpleLottieValueCallback<T> callback)
        {
            _callback = callback;
        }

        public override T GetValue(LottieFrameInfo<T> frameInfo)
        {
            if (_callback != null)
            {
                return _callback.Invoke(frameInfo);
            }

            return default(T);
        }
    }
}