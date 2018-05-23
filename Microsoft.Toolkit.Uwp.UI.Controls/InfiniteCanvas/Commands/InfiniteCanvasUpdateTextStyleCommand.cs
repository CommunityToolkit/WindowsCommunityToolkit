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
    internal class InfiniteCanvasUpdateTextStyleCommand : IInfiniteCanvasCommand
    {
        private readonly bool _oldValue;
        private readonly bool _newValue;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasUpdateTextStyleCommand(TextDrawable drawable, bool oldValue, bool newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _drawable = drawable;
        }

        public void Execute()
        {
            _drawable.IsItalic = _newValue;
        }

        public void Undo()
        {
            _drawable.IsItalic = _oldValue;
        }
    }
}