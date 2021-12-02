// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using Windows.UI.Input.Inking;

namespace CommunityToolkit.WinUI.UI.Controls
{
    /// <summary>
    /// InfiniteCanvas is a canvas that supports Ink, Text, Format Text, Zoom in/out, Redo, Undo, Export canvas data, Import canvas data.
    /// </summary>
    public partial class InfiniteCanvas
    {
        private static void CanvasWidthHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var infiniteCanvas = (InfiniteCanvas)d;
            infiniteCanvas.SetCanvasWidthHeight();
        }

        private static void IsToolbarVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var infiniteCanvas = (InfiniteCanvas)d;
            if (infiniteCanvas._canvasToolbarContainer != null)
            {
                infiniteCanvas._canvasToolbarContainer.Visibility = infiniteCanvas.IsToolbarVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static void MinMaxZoomChangedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var infiniteCanvas = (InfiniteCanvas)d;
            infiniteCanvas.SetZoomFactor();
        }

        /// <inheritdoc />
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            var isCtrlDown = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

            if (!isCtrlDown)
            {
                return;
            }

            if (e.Key == VirtualKey.Z)
            {
                Undo();
            }

            if (e.Key == VirtualKey.Y)
            {
                Redo();
            }

            base.OnKeyDown(e);
        }

        private void InfiniteCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            // Application.Current.LeavingBackground -= Current_LeavingBackground;
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            Redo();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            Undo();
        }

        private void EnableTouchInkingButton_Unchecked(object sender, RoutedEventArgs e)
        {
            // _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Pen;
        }

        private void EnableTouchInkingButton_Checked(object sender, RoutedEventArgs e)
        {
            // _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;
        }

        private void EnableTextButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _canvasTextBox.Visibility = Visibility.Collapsed;

            // _inkCanvas.Visibility = Visibility.Visible;
            _canvasTextBoxTools.Visibility = Visibility.Collapsed;
        }

        private void EnableTextButton_Checked(object sender, RoutedEventArgs e)
        {
            // _inkCanvas.Visibility = Visibility.Collapsed;
            _canvasTextBoxTools.Visibility = Visibility.Visible;
        }

        private void EraseAllButton_Click(object sender, RoutedEventArgs e)
        {
            _canvasTextBox.Visibility = Visibility.Collapsed;
            ClearTextBoxValue();

            _drawingSurfaceRenderer.ClearAll(ViewPort);
        }

        private async void Current_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            // work around to virtual drawing surface bug.
            await Task.Delay(1000);

            _drawingSurfaceRenderer.ReDraw(ViewPort, _infiniteCanvasScrollViewer.ZoomFactor);
        }

        private void InkScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReDrawCanvas();
        }

        private void OnStrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            /*
            _drawingSurfaceRenderer.ExecuteCreateInk(_inkSync.BeginDry());
            _inkSync.EndDry();
            */
            ReDrawCanvas();
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            // TODO: WinUI3: still not supported
            /*
            if (_inkCanvasToolBar.ActiveTool == _inkCanvasToolBar.GetToolButton(Microsoft.UI.Xaml.Controls.InkToolbarTool.Eraser) || args.CurrentPoint.Properties.IsEraser)
            {
                _drawingSurfaceRenderer.Erase(args.CurrentPoint.Position, ViewPort, _infiniteCanvasScrollViewer.ZoomFactor);
            }
            */
        }

        private void InkScrollViewer_ViewChanged(object sender, Microsoft.UI.Xaml.Controls.ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                _drawingSurfaceRenderer.SetScale(_infiniteCanvasScrollViewer.ZoomFactor);

                ReDrawCanvas();
            }
        }

        private void DrawingSurfaceRenderer_CommandExecuted(object sender, EventArgs e)
        {
            ReRenderCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}