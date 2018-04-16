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
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model
{
    internal sealed class AsyncCompositionLoader
    {
        private readonly CancellationToken _cancellationToken;
        private TaskCompletionSource<LottieComposition> _tcs;

        internal AsyncCompositionLoader(CancellationToken cancellationToken)
        {
            Utils.Utils.DpScale();
            _cancellationToken = cancellationToken;
        }

        internal async Task<LottieComposition> Execute(params JsonReader[] @params)
        {
            _tcs = new TaskCompletionSource<LottieComposition>();
            await Task.Run(
                () =>
            {
                try
                {
                    _tcs.SetResult(LottieComposition.Factory.FromJsonSync(@params[0]));
                }
                catch (IOException e)
                {
                    throw new InvalidOperationException(e.Message);
                }
            }, _cancellationToken);
            return await _tcs.Task;
        }

        public void Cancel()
        {
            _tcs.SetCanceled();
        }
    }
}