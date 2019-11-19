// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="Eyedropper"/> control can pick up a color from anywhere in your application.
    /// </summary>
    public partial class Eyedropper : Control
    {
        private const string TouchState = "Touch";
        private const string MousePenState = "MousePen";

        private const int PreviewPixelsPerRawPixel = 10;
        private const int PixelCountPerRow = 11;
        private static readonly CoreCursor DefaultCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private static readonly CoreCursor MoveCursor = new CoreCursor(CoreCursorType.Cross, 1);
        private readonly CanvasDevice _device = CanvasDevice.GetSharedDevice();
        private readonly TranslateTransform _layoutTransform = new TranslateTransform();

        private readonly Popup _popup;
        private readonly CanvasImageSource _previewImageSource;
        private readonly Grid _rootGrid;
        private readonly Grid _targetGrid;
        private CanvasBitmap _appScreenshot;
        private Action _lazyTask;
        private uint? _pointerId = null;
        private TaskCompletionSource<Color> _taskSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Eyedropper"/> class.
        /// </summary>
        public Eyedropper()
        {
            DefaultStyleKey = typeof(Eyedropper);
            _rootGrid = new Grid();
            _targetGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromArgb(0x01, 0x00, 0x00, 0x00))
            };
            _popup = new Popup
            {
                Child = _rootGrid
            };
            RenderTransform = _layoutTransform;
            _previewImageSource = new CanvasImageSource(_device, PreviewPixelsPerRawPixel * PixelCountPerRow, PreviewPixelsPerRawPixel * PixelCountPerRow, 96f);
            Preview = _previewImageSource;
            Loaded += Eyedropper_Loaded;
        }

        /// <summary>
        /// Occurs when the Color property has changed.
        /// </summary>
        public event TypedEventHandler<Eyedropper, EyedropperColorChangedEventArgs> ColorChanged;

        /// <summary>
        /// Occurs when the eyedropper begins to take color.
        /// </summary>
        public event TypedEventHandler<Eyedropper, EventArgs> PickStarted;

        /// <summary>
        /// Occurs when the eyedropper stops to take color.
        /// </summary>
        public event TypedEventHandler<Eyedropper, EventArgs> PickCompleted;

        /// <summary>
        /// Open the eyedropper.
        /// </summary>
        /// <param name="startPoint">The initial eyedropper position</param>
        /// <returns>The picked color.</returns>
        public async Task<Color> Open(Point? startPoint = null)
        {
            _taskSource = new TaskCompletionSource<Color>();
            HookUpEvents();
            Opacity = 0;
            if (startPoint.HasValue)
            {
                _lazyTask = async () =>
                {
                    await UpdateAppScreenshotAsync();
                    UpdateEyedropper(startPoint.Value);
                    Opacity = 1;
                };
            }

            _rootGrid.Children.Add(_targetGrid);
            _rootGrid.Children.Add(this);
            _rootGrid.Width = Window.Current.Bounds.Width;
            _rootGrid.Height = Window.Current.Bounds.Height;
            UpadateWorkArea();
            _popup.IsOpen = true;
            var result = await _taskSource.Task;
            _taskSource = null;
            _rootGrid.Children.Clear();
            return result;
        }

        /// <summary>
        /// Close the eyedropper.
        /// </summary>
        public void Close()
        {
            if (_taskSource != null && !_taskSource.Task.IsCanceled)
            {
                _taskSource.SetCanceled();
                _rootGrid.Children.Clear();
            }
        }

        private void HookUpEvents()
        {
            Unloaded += Eyedropper_Unloaded;
            Window.Current.SizeChanged += Window_SizeChanged;
            DisplayInformation.GetForCurrentView().DpiChanged += Eyedropper_DpiChanged;
            _targetGrid.PointerEntered += TargetGrid_PointerEntered;
            _targetGrid.PointerExited += TargetGrid_PointerExited;
            _targetGrid.PointerPressed += TargetGrid_PointerPressed;
            _targetGrid.PointerMoved += TargetGrid_PointerMoved;
            _targetGrid.PointerReleased += TargetGrid_PointerReleased;
        }

        private void UnhookEvents()
        {
            Unloaded -= Eyedropper_Unloaded;
            Window.Current.SizeChanged -= Window_SizeChanged;
            DisplayInformation.GetForCurrentView().DpiChanged -= Eyedropper_DpiChanged;
            if (_targetGrid != null)
            {
                _targetGrid.PointerEntered -= TargetGrid_PointerEntered;
                _targetGrid.PointerExited -= TargetGrid_PointerExited;
                _targetGrid.PointerPressed -= TargetGrid_PointerPressed;
                _targetGrid.PointerMoved -= TargetGrid_PointerMoved;
                _targetGrid.PointerReleased -= TargetGrid_PointerReleased;
            }
        }

        private void Eyedropper_Loaded(object sender, RoutedEventArgs e)
        {
            _lazyTask?.Invoke();
            _lazyTask = null;
        }

        private void TargetGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            if (_pointerId != null)
            {
                _pointerId = null;
                Opacity = 0;
            }
        }

        private void TargetGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = MoveCursor;
        }

        private async void Eyedropper_DpiChanged(DisplayInformation sender, object args)
        {
            await UpdateAppScreenshotAsync();
        }

        private async void TargetGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.Pointer;
            if (pointer.PointerId == _pointerId)
            {
                var point = e.GetCurrentPoint(_rootGrid);
                if (_appScreenshot == null)
                {
                    await UpdateAppScreenshotAsync();
                }

                UpdateEyedropper(point.Position);
                PickCompleted?.Invoke(this, EventArgs.Empty);
                _pointerId = null;
                if (!_taskSource.Task.IsCanceled)
                {
                    _taskSource.SetResult(Color);
                }
            }
        }

        private void TargetGrid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.Pointer;
            if (pointer.PointerId == _pointerId)
            {
                var point = e.GetCurrentPoint(_rootGrid);
                UpdateEyedropper(point.Position);
            }
        }

        private async void TargetGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pointerId = e.Pointer.PointerId;
            var point = e.GetCurrentPoint(_rootGrid);
            PickStarted?.Invoke(this, EventArgs.Empty);
            await UpdateAppScreenshotAsync();
            UpdateEyedropper(point.Position);

            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
            {
                VisualStateManager.GoToState(this, TouchState, false);
            }
            else
            {
                VisualStateManager.GoToState(this, MousePenState, false);
            }

            if (Opacity < 1)
            {
                Opacity = 1;
            }
        }

        private void Eyedropper_Unloaded(object sender, RoutedEventArgs e)
        {
            UnhookEvents();
            if (_popup != null)
            {
                _popup.IsOpen = false;
            }

            _appScreenshot?.Dispose();
            _appScreenshot = null;

            Window.Current.CoreWindow.PointerCursor = DefaultCursor;
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (_rootGrid != null)
            {
                _rootGrid.Width = Window.Current.Bounds.Width;
                _rootGrid.Height = Window.Current.Bounds.Height;
            }
        }
    }
}
