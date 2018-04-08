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

using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasUpdateTextColorCommand : IInfiniteCanvasCommand
    {
        private readonly Color _oldColor;
        private readonly Color _newColor;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasUpdateTextColorCommand(TextDrawable drawable, Color oldText, Color newText)
        {
            _oldColor = oldText;
            _newColor = newText;
            _drawable = drawable;
        }

        public void Execute()
        {
            _drawable.TextColor = _newColor;
        }

        public void UnExecute()
        {
            _drawable.TextColor = _oldColor;
        }
    }
}