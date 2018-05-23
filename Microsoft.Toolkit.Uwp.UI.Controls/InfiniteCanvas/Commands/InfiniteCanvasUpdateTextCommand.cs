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
    internal class InfiniteCanvasUpdateTextCommand : IInfiniteCanvasCommand
    {
        private readonly string _oldText;
        private readonly string _newText;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasUpdateTextCommand(TextDrawable drawable, string oldText, string newText)
        {
            _oldText = oldText;
            _newText = newText;
            _drawable = drawable;
        }

        public void Execute()
        {
            _drawable.Text = _newText;
        }

        public void Undo()
        {
            _drawable.Text = _oldText;
        }
    }
}