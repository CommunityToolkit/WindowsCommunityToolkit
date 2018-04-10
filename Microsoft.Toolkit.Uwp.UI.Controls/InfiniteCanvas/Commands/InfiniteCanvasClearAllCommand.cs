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

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasClearAllCommand : IInfiniteCanvasCommand
    {
        private readonly List<IDrawable> _drawableList;
        private IDrawable[] _storeList;

        public InfiniteCanvasClearAllCommand(List<IDrawable> drawableList)
        {
            _drawableList = drawableList;
        }

        public void Execute()
        {
            _storeList = new IDrawable[_drawableList.Count];
            for (int i = 0; i < _drawableList.Count; i++)
            {
                _storeList[i] = _drawableList[i];
            }

            _drawableList.Clear();
        }

        public void Undo()
        {
            foreach (var drawable in _storeList)
            {
                _drawableList.Add(drawable);
            }
        }
    }
}
