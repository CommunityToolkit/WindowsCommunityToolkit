using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class InfiniteCanvasVirtualDrawingSurface
    {
        private readonly List<IDrawable> _visibleList = new List<IDrawable>();
        private readonly List<IDrawable> _drawableList = new List<IDrawable>();

        private readonly Stack<IInfiniteCanvasCommand> _undoCommands = new Stack<IInfiniteCanvasCommand>();
        private readonly Stack<IInfiniteCanvasCommand> _redoCommands = new Stack<IInfiniteCanvasCommand>();

        public void ReDraw(Rect viewPort)
        {
            _visibleList.Clear();
            foreach (var drawable in _drawableList)
            {
                if (drawable.IsVisible(viewPort))
                {
                    _visibleList.Add(drawable);
                }
            }

            Rect toDraw;
            var first = _visibleList.FirstOrDefault();
            if (first != null)
            {
                double top = first.Bounds.Top,
                    bottom = first.Bounds.Bottom,
                    left = first.Bounds.Left,
                    right = first.Bounds.Right;

                for (var index = 1; index < _visibleList.Count; index++)
                {
                    var stroke = _visibleList[index];
                    bottom = Math.Max(stroke.Bounds.Bottom, bottom);
                    right = Math.Max(stroke.Bounds.Right, right);
                    top = Math.Min(stroke.Bounds.Top, top);
                    left = Math.Min(stroke.Bounds.Left, left);
                }

                toDraw = new Rect(Math.Max(left, 0), Math.Max(top, 0), Math.Max(right - left, 0), Math.Max(bottom - top, 0));

                toDraw.Union(viewPort);
            }
            else
            {
                toDraw = viewPort;
            }

            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface, toDraw))
            {
                drawingSession.Clear(Background);
                foreach (var drawable in _visibleList)
                {
                    drawable.Draw(drawingSession, toDraw);
                }
            }
        }

        public void ClearAll(Rect viewPort)
        {
            _visibleList.Clear();
            _drawableList.Clear();

            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface, viewPort))
            {
                drawingSession.Clear(Background);
            }
        }

        public void Undo(Rect viewPort)
        {
            if (_undoCommands.Count != 0)
            {
                IInfiniteCanvasCommand command = _undoCommands.Pop();
                command.UnExecute();
                _redoCommands.Push(command);

                ReDraw(viewPort);
            }
        }

        public void Redo(Rect viewPort)
        {
            if (_redoCommands.Count != 0)
            {
                IInfiniteCanvasCommand command = _redoCommands.Pop();
                command.Execute();
                _undoCommands.Push(command);

                ReDraw(viewPort);
            }
        }

        public void UpdateTextBoxText(string newText)
        {
            var drawable = GetSelectedTextDrawable();
            var command = new InfiniteCanvasUpdateTextCommand(drawable, drawable.Text, newText);
            ExecuteCommand(command);
        }

        public void UpdateTextBoxColor(Color newColor)
        {
            var drawable = GetSelectedTextDrawable();
            var command = new InfiniteCanvasUpdateTextColorCommand(drawable, drawable.TextColor, newColor);
            ExecuteCommand(command);
        }

        public void UpdateTextBoxStyle(bool newValue)
        {
            var drawable = GetSelectedTextDrawable();
            var command = new InfiniteCanvasUpdateTextStyleCommand(drawable, drawable.IsItalic, newValue);
            ExecuteCommand(command);
        }

        public void UpdateTextBoxWeight(bool newValue)
        {
            var drawable = GetSelectedTextDrawable();
            var command = new InfiniteCanvasUpdateTextWeightCommand(drawable, drawable.IsBold, newValue);
            ExecuteCommand(command);
        }

        public void UpdateTextBoxFontSize(float newValue)
        {
            var drawable = GetSelectedTextDrawable();
            var command = new InfiniteCanvasUpdateTextFontSizeCommand(drawable, drawable.FontSize, newValue);
            ExecuteCommand(command);
        }

        public void CreateTextBox(double x, double y, double width, double height, int textFontSize, string text, Color color, bool isBold, bool isItalic)
        {
            var command = new InfiniteCanvasCreateTextBoxCommand(_drawableList, x, y, width, height, textFontSize, text, color, isBold, isItalic);
            ExecuteCommand(command);
        }

        public void RemoveTextBox()
        {
            var drawable = GetSelectedTextDrawable();
            var command = new InfiniteCanvasRemoveTextBoxCommand(_drawableList, drawable);
            ExecuteCommand(command);
        }

        public void CreateInk(IReadOnlyList<InkStroke> beginDry)
        {
            var command = new InfiniteCanvasCreateInkCommand(_drawableList, beginDry);
            ExecuteCommand(command);
        }

        internal void EraseInk(IDrawable drawable)
        {
            var command = new InfiniteCanvasEraseInkCommand(_drawableList, drawable);
            ExecuteCommand(command);
        }

        private void ExecuteCommand(IInfiniteCanvasCommand command)
        {
            _undoCommands.Push(command);
            _redoCommands.Clear();
            command.Execute();
        }
    }
}
