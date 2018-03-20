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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Manager;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// This can be used to show an lottie animation in any place that would normally take a drawable.
    /// If there are masks or mattes, then you MUST call <seealso cref="RecycleBitmaps()"/> when you are done
    /// or else you will leak bitmaps.
    /// <para>
    /// It is preferable to use <seealso cref="LottieAnimationView"/> when possible because it
    /// handles bitmap recycling and asynchronous loading
    /// of compositions.
    /// </para>
    /// </summary>
    public class LottieDrawable : UserControl, IAnimatable, IDisposable
    {
        private readonly LottieValueAnimator _animator = new LottieValueAnimator();
        private readonly List<Action<LottieComposition>> _lazyCompositionTasks = new List<Action<LottieComposition>>();

        private Matrix3X3 _matrix = Matrix3X3.CreateIdentity();
        private LottieComposition _composition;
        private float _scale = 1f;
        private ImageAssetManager _imageAssetManager;
        private IImageAssetDelegate _imageAssetDelegate;
        private FontAssetManager _fontAssetManager;
        private FontAssetDelegate _fontAssetDelegate;
        private TextDelegate _textDelegate;
        private bool _enableMergePaths;
        private CompositionLayer _compositionLayer;
        private byte _alpha = 255;
        private bool _performanceTrackingEnabled;
        private BitmapCanvas _bitmapCanvas;
        private CanvasAnimatedControl _canvasControl;
        private bool _forceSoftwareRenderer;

        /// <summary>
        /// This value used used with the <see cref="RepeatCount"/> property to repeat
        /// the animation indefinitely.
        /// </summary>
        public const int Infinite = -1;

        internal LottieDrawable()
        {
            _animator.Update += (sender, e) =>
            {
                if (_compositionLayer != null)
                {
                    _compositionLayer.Progress = _animator.AnimatedValueAbsolute;
                }
            };
            Loaded += UserControl_Loaded;
            Unloaded += UserControl_Unloaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _canvasControl = new CanvasAnimatedControl
            {
                ForceSoftwareRenderer = _forceSoftwareRenderer
            };

            _canvasControl.Paused = true;
            _canvasControl.Draw += CanvasControlOnDraw;
            _canvasControl.Loaded += (s, args) => InvalidateMeasure();
            Content = _canvasControl;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            lock (this)
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                if (_canvasControl != null)
                {
                    _canvasControl.Draw -= CanvasControlOnDraw;
                    _canvasControl.RemoveFromVisualTree();
                    _canvasControl = null;
                }

                if (_bitmapCanvas != null)
                {
                    _bitmapCanvas.Dispose();
                    _bitmapCanvas = null;
                }

                ClearComposition();
            }
        }

        /// <summary>
        /// Method used to force software rendering on the underlying <see cref="CanvasAnimatedControl"/>
        /// </summary>
        /// <param name="force">True will force the devices that this control creates to be forced to software rendering.</param>
        public void ForceSoftwareRenderer(bool force)
        {
            _forceSoftwareRenderer = force;
            if (_canvasControl != null)
            {
                _canvasControl.ForceSoftwareRenderer = force;
            }
        }

        /// <summary>
        /// Method used to know whether or not any layer in this composition has a masks.
        /// </summary>
        /// <returns>Returns true if any layer in this composition has a mask.</returns>
        public virtual bool HasMasks()
        {
            return _compositionLayer != null && _compositionLayer.HasMasks();
        }

        /// <summary>
        /// Method used to know whether or not any layer in this composition has a matte.
        /// </summary>
        /// <returns>Returns true if any layer in this composition has a matte.</returns>
        public virtual bool HasMatte()
        {
            return _compositionLayer != null && _compositionLayer.HasMatte();
        }

        internal virtual bool EnableMergePathsForKitKatAndAbove()
        {
            return _enableMergePaths;
        }

        /// <summary>
        /// Enable this to get merge path support for devices running KitKat (19) and above.
        ///
        /// Merge paths currently don't work if the the operand shape is entirely contained within the
        /// first shape. If you need to cut out one shape from another shape, use an even-odd fill type
        /// instead of using merge paths.
        /// </summary>
        public virtual void EnableMergePathsForKitKatAndAbove(bool enable)
        {
            _enableMergePaths = enable;
            if (_composition != null)
            {
                BuildCompositionLayer();
            }
        }

        /// <summary>
        /// Get a value that indicates if merge paths are enabled or not
        /// </summary>
        /// <returns>Returns true if merge paths are enabled</returns>
        public bool IsMergePathsEnabledForKitKatAndAbove()
        {
            return _enableMergePaths;
        }

        /// <summary>
        /// Gets or sets the folder to look for the image assets.
        /// If you use image assets, you must explicitly specify the folder in assets/ in which they are
        /// located because bodymovin uses the name filenames across all compositions (img_#).
        /// Do NOT rename the images themselves.
        ///
        /// If your images are located in src/main/assets/airbnb_loader/ then call
        /// `setImageAssetsFolder("airbnb_loader/");`.
        ///
        ///
        /// If you use LottieDrawable directly, you MUST call <seealso cref="RecycleBitmaps()"/> when you
        /// are done. Calling <seealso cref="RecycleBitmaps()"/> doesn't have to be final and <seealso cref="LottieDrawable"/>
        /// will recreate the bitmaps if needed but they will leak if you don't recycle them.
        ///
        /// Be wary if you are using many images, however. Lottie is designed to work with vector shapes
        /// from After Effects. If your images look like they could be represented with vector shapes,
        /// see if it is possible to convert them to shape layers and re-export your animation. Check
        /// the documentation at http://airbnb.io/lottie for more information about importing shapes from
        /// Sketch or Illustrator to avoid this.
        /// </summary>
        public virtual string ImageAssetsFolder { get; set; }

        /// <summary>
        /// If you have image assets and use <seealso cref="LottieDrawable"/> directly, you must call this yourself.
        ///
        /// Calling recycleBitmaps() doesn't have to be final and <seealso cref="LottieDrawable"/>
        /// will recreate the bitmaps if needed but they will leak if you don't recycle them.
        ///
        /// </summary>
        public virtual void RecycleBitmaps()
        {
            _imageAssetManager?.RecycleBitmaps();
        }

        /// <summary>
        /// Sets the current <see cref="LottieComposition"/> being used by this <see cref="LottieDrawable"/>
        /// </summary>
        /// <param name="composition">The new <see cref="LottieComposition"/> to be used by this <see cref="LottieDrawable"/></param>
        /// <returns>Returns true if the composition is different from the previously set composition, false otherwise.</returns>
        public virtual bool SetComposition(LottieComposition composition)
        {
            // if (Callback == null) // TODO: needed?
            // {
            //    throw new System.InvalidOperationException("You or your view must set a Drawable.Callback before setting the composition. This " + "gets done automatically when added to an ImageView. " + "Either call ImageView.setImageDrawable() before setComposition() or call " + "setCallback(yourView.getCallback()) first.");
            // }
            if (_composition == composition)
            {
                return false;
            }

            lock (this)
            {
                ClearComposition();
                _composition = composition;
                BuildCompositionLayer();
                _animator.Composition = composition;
                Progress = _animator.AnimatedFraction;
                Scale = _scale;
                UpdateBounds();

                // We copy the tasks to a new ArrayList so that if this method is called from multiple threads,
                // then there won't be two iterators iterating and removing at the same time.
                foreach (var t in _lazyCompositionTasks.ToList())
                {
                    t.Invoke(composition);
                }

                _lazyCompositionTasks.Clear();
                composition.PerformanceTrackingEnabled = _performanceTrackingEnabled;
            }

            return true;
        }

        /// <summary>
        /// Sets a value indicating whether the performance tracking is enabled or not.
        /// </summary>
        public virtual bool PerformanceTrackingEnabled
        {
            set
            {
                _performanceTrackingEnabled = value;
                if (_composition != null)
                {
                    _composition.PerformanceTrackingEnabled = value;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceTracker"/> object associated with this <see cref="LottieDrawable"/>
        /// </summary>
        public virtual PerformanceTracker PerformanceTracker => _composition?.PerformanceTracker;

        private void BuildCompositionLayer()
        {
            _compositionLayer = new CompositionLayer(this, LayerParser.Parse(_composition), _composition.Layers, _composition);
        }

        /// <summary>
        /// Clears the internal <see cref="LottieComposition"/> stopping the animation and recycling it's bitmaps, if any exists.
        /// </summary>
        public void ClearComposition()
        {
            RecycleBitmaps();
            if (_animator.IsRunning)
            {
                _animator.Cancel();
            }

            lock (this)
            {
                _composition = null;
            }

            _compositionLayer = null;
            _imageAssetManager = null;
            InvalidateSelf();
        }

        /// <summary>
        /// Invalidates the animation, requiring it's redrawing.
        /// </summary>
        public void InvalidateSelf()
        {
            _canvasControl?.Invalidate();
        }

        /// <summary>
        /// Gets or sets the alpha of the whole animation
        /// </summary>
        public byte Alpha
        {
            get => _alpha;
            set => _alpha = value;
        }

        private void CanvasControlOnDraw(ICanvasAnimatedControl canvasControl, CanvasAnimatedDrawEventArgs args)
        {
            lock (this)
            {
                Render(_bitmapCanvas, _compositionLayer, _composition, canvasControl.Device, _scale, _alpha, _matrix, canvasControl.Size.Width, canvasControl.Size.Height, args.DrawingSession);
            }
        }

        internal static void Render(
            BitmapCanvas bitmapCanvas,
            CompositionLayer compositionLayer,
            LottieComposition composition,
            CanvasDevice device,
            float renderScale,
            byte alpha,
            Matrix3X3 matrix,
            double width,
            double height,
            CanvasDrawingSession drawingSession)
        {
            // If there are masks or mattes, we can't scale the animation larger than the canvas or else
            // the off screen rendering for masks and mattes after saveLayer calls will get clipped.
            float GetMaxScale()
            {
                var maxScaleX = (float)bitmapCanvas.Width / (float)composition.Bounds.Width;
                var maxScaleY = (float)bitmapCanvas.Height / (float)composition.Bounds.Height;
                return Math.Min(maxScaleX, maxScaleY);
            }

            if (bitmapCanvas == null)
            {
                return;
            }

            using (bitmapCanvas.CreateSession(device, width, height, drawingSession))
            {
                bitmapCanvas.Clear(Colors.Transparent);
                LottieLog.BeginSection("Drawable.Draw");
                if (compositionLayer == null)
                {
                    return;
                }

                var scale = renderScale;
                float extraScale = 1f;

                float maxScale = GetMaxScale();
                if (scale > maxScale)
                {
                    scale = maxScale;
                    extraScale = renderScale / scale;
                }

                if (extraScale > 1)
                {
                    // This is a bit tricky...
                    // We can't draw on a canvas larger than ViewConfiguration.get(context).getScaledMaximumDrawingCacheSize()
                    // which works out to be roughly the size of the screen because Android can't generate a
                    // bitmap large enough to render to.
                    // As a result, we cap the scale such that it will never be wider/taller than the screen
                    // and then only render in the top left corner of the canvas. We then use extraScale
                    // to scale up the rest of the scale. However, since we rendered the animation to the top
                    // left corner, we need to scale up and translate the canvas to zoom in on the top left
                    // corner.
                    bitmapCanvas.Save();
                    float halfWidth = (float)composition.Bounds.Width / 2f;
                    float halfHeight = (float)composition.Bounds.Height / 2f;
                    float scaledHalfWidth = halfWidth * scale;
                    float scaledHalfHeight = halfHeight * scale;
                    bitmapCanvas.Translate(
                        (renderScale * halfWidth) - scaledHalfWidth,
                        (renderScale * halfHeight) - scaledHalfHeight);
                    bitmapCanvas.Scale(extraScale, extraScale, scaledHalfWidth, scaledHalfHeight);
                }

                matrix.Reset();
                matrix = MatrixExt.PreScale(matrix, scale, scale);
                compositionLayer.Draw(bitmapCanvas, matrix, alpha);
                LottieLog.EndSection("Drawable.Draw");

                if (extraScale > 1)
                {
                    bitmapCanvas.Restore();
                }
            }
        }

        /// <inheritdoc/>
        public void Start()
        {
            PlayAnimation();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            EndAnimation();
        }

        /// <inheritdoc/>
        public bool IsRunning => IsAnimating;

        /// <summary>
        /// Plays the animation from the beginning. If speed is &lt; 0, it will start at the end
        /// and play towards the beginning
        /// </summary>
        public virtual void PlayAnimation()
        {
            if (_compositionLayer == null)
            {
                _lazyCompositionTasks.Add(composition =>
                {
                    PlayAnimation();
                });
                return;
            }

            _animator.PlayAnimation();
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        public void EndAnimation()
        {
            _lazyCompositionTasks.Clear();
            _animator.EndAnimation();
        }

        /// <summary>
        /// Continues playing the animation from its current position. If speed &lt; 0, it will play backwards
        /// from the current position.
        /// </summary>
        public virtual void ResumeAnimation()
        {
            if (_compositionLayer == null)
            {
                _lazyCompositionTasks.Add(composition =>
                {
                    ResumeAnimation();
                });
                return;
            }

            _animator.ResumeAnimation();
        }

        /// <summary>
        /// Gets or sets the minimum frame that the animation will start from when playing or looping.
        /// </summary>
        public float MinFrame
        {
            get => _animator.MinFrame;
            set => _animator.MinFrame = value;
        }

        /// <summary>
        /// Sets the minimum progress that the animation will start from when playing or looping.
        /// </summary>
        public float MinProgress
        {
            set
            {
                if (_composition == null)
                {
                    _lazyCompositionTasks.Add(composition =>
                    {
                        MinProgress = value;
                    });
                    return;
                }

                MinFrame = value * _composition.DurationFrames;
            }
        }

        /// <summary>
        /// Gets or sets the maximum frame that the animation will end at when playing or looping.
        /// </summary>
        public float MaxFrame
        {
            get => _animator.MaxFrame;
            set => _animator.MaxFrame = value;
        }

        /// <summary>
        /// Sets the maximum progress that the animation will end at when playing or looping.
        /// </summary>
        public float MaxProgress
        {
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > 1)
                {
                    value = 1;
                }

                if (_composition == null)
                {
                    _lazyCompositionTasks.Add(composition =>
                    {
                        MaxProgress = value;
                    });
                    return;
                }

                MaxFrame = value / _composition.DurationFrames;
            }
        }

        /// <summary>
        /// <see cref="MinFrame"/>
        /// <see cref="MaxFrame"/>
        /// </summary>
        /// <param name="minFrame">The minimum frame that the animation will start from when playing or looping.</param>
        /// <param name="maxFrame">The maximum frame that the animation will end at when playing or looping.</param>
        public void SetMinAndMaxFrame(float minFrame, float maxFrame)
        {
            _animator.SetMinAndMaxFrames(minFrame, maxFrame);
        }

        /// <summary>
        /// <see cref="MinProgress"/>
        /// <see cref="MaxProgress"/>
        /// </summary>
        /// <param name="minProgress">The minimum progress that the animation will start from when playing or looping.</param>
        /// <param name="maxProgress">The maximum progress that the animation will end at when playing or looping.</param>
        public void SetMinAndMaxProgress(float minProgress, float maxProgress)
        {
            if (minProgress < 0)
            {
                minProgress = 0;
            }

            if (minProgress > 1)
            {
                minProgress = 1;
            }

            if (maxProgress < 0)
            {
                maxProgress = 0;
            }

            if (maxProgress > 1)
            {
                maxProgress = 1;
            }

            if (_composition == null)
            {
                _lazyCompositionTasks.Add(composition =>
                {
                    SetMinAndMaxProgress(minProgress, maxProgress);
                });
                return;
            }

            SetMinAndMaxFrame(minProgress * _composition.DurationFrames, maxProgress * _composition.DurationFrames);
        }

        /// <summary>
        /// Reverses the current animation speed. This does NOT play the animation.
        /// <see cref="Speed"/>
        /// <see cref="PlayAnimation"/>
        /// <see cref="ResumeAnimation"/>
        /// </summary>
        public void ReverseAnimationSpeed()
        {
            _animator.ReverseAnimationSpeed();
        }

        /// <summary>
        /// Gets or sets the current playback speed. If speed &lt; 0, the animation will play backwards.
        /// This will be &lt; 0 if the animation is playing backwards.
        /// </summary>
        public virtual float Speed
        {
            get => _animator.Speed;
            set => _animator.Speed = value;
        }

        /// <summary>
        /// This event will be invoked whenever the frame of the animation changes.
        /// </summary>
        public event EventHandler<ValueAnimator.ValueAnimatorUpdateEventArgs> AnimatorUpdate
        {
            add => _animator.Update += value;
            remove => _animator.Update -= value;
        }

        /// <summary>
        /// This event will be invoked whenever the animation ends and it is going to be repeated again.
        /// </summary>
        public event EventHandler AnimationRepeat
        {
            add => _animator.AnimationRepeat += value;
            remove => _animator.AnimationRepeat -= value;
        }

        /// <summary>
        /// Clears the <seealso cref="AnimatorUpdate"/> event handler.
        /// </summary>
        public void RemoveAllUpdateListeners()
        {
            _animator.RemoveAllUpdateListeners();
        }

        /// <summary>
        /// This event will be invoked whenever the internal animator is executed.
        /// </summary>
        public event EventHandler ValueChanged
        {
            add => _animator.ValueChanged += value;
            remove => _animator.ValueChanged -= value;
        }

        /// <summary>
        /// Clears the <seealso cref="ValueChanged"/> event handler.
        /// </summary>
        public void RemoveAllAnimatorListeners()
        {
            _animator.RemoveAllListeners();
        }

        /// <summary>
        /// Gets or sets the currently rendered frame.
        /// If the composition isn't set yet, the progress will be set to the frame when
        /// it is.
        /// </summary>
        public float Frame
        {
            get => _animator.Frame;

            set
            {
                if (_composition == null)
                {
                    _lazyCompositionTasks.Add(composition =>
                    {
                        Frame = value;
                    });
                    return;
                }

                _animator.Frame = value;
            }
        }

        /// <summary>
        /// Gets or sets the progress of the animation. This value will be a double from 0 to 1.
        /// </summary>
        public virtual float Progress
        {
            get => _animator.AnimatedValueAbsolute;
            set
            {
                if (_composition == null)
                {
                    _lazyCompositionTasks.Add(composition =>
                    {
                        Progress = value;
                    });
                    return;
                }

                Frame = MiscUtils.Lerp(_composition.StartFrame, _composition.EndFrame, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="RepeatCount"/> is Infinite (0) or not.
        /// </summary>
        [Obsolete]
        public virtual bool Looping
        {
            get => _animator.RepeatCount == Infinite;
            set => _animator.RepeatCount = value ? Infinite : 0;
        }

        /// <summary>
        /// Gets or sets what this animation should do when it reaches the end. This
        /// setting is applied only when the repeat count is either greater than
        /// 0 or <see cref="LottieDrawable.Infinite"/>. Defaults to <see cref="Lottie.RepeatMode.Restart"/>.
        /// Return either one of <see cref="Lottie.RepeatMode.Reverse"/> or <see cref="Lottie.RepeatMode.Restart"/>
        /// </summary>
        /// <param name="value"><seealso cref="RepeatMode"/></param>
        public RepeatMode RepeatMode
        {
            get => _animator.RepeatMode;
            set => _animator.RepeatMode = value;
        }

        /// <summary>
        /// Gets or sets how many times the animation should be repeated. If the repeat
        /// count is 0, the animation is never repeated. If the repeat count is
        /// greater than 0 or <see cref="LottieDrawable.Infinite"/>, the repeat mode will be taken
        /// into account. The repeat count is 0 by default.
        ///
        /// Count the number of times the animation should be repeated
        ///
        /// Return the number of times the animation should repeat, or <see cref="LottieDrawable.Infinite"/>
        /// </summary>
        public int RepeatCount
        {
            get => _animator.RepeatCount;
            set => _animator.RepeatCount = value;
        }

        /// <summary>
        /// Gets the number of times that this animation finished since the last call to <seealso cref="PlayAnimation"/>.
        /// </summary>
        public int TimesRepeated
        {
            get => _animator.TimesRepeated;
        }

        /// <summary>
        /// Gets or sets the current frame rate that this animation is being executed
        /// </summary>
        public float FrameRate
        {
            get => _animator.FrameRate;
            set => _animator.FrameRate = value;
        }

        /// <summary>
        /// Gets a value indicating whether the internal <see cref="ValueAnimator"/> is running or not
        /// </summary>
        public virtual bool IsAnimating => _animator.IsRunning;

        /// <summary>
        /// Sets the FontAssetDelegate to use. Use this to manually set fonts.
        /// </summary>
        public virtual FontAssetDelegate FontAssetDelegate
        {
            set
            {
                _fontAssetDelegate = value;
                if (_fontAssetManager != null)
                {
                    _fontAssetManager.Delegate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TextDelegate"/> associated with this <see cref="LottieDrawable"/>
        /// </summary>
        public virtual TextDelegate TextDelegate
        {
            get => _textDelegate;
            set => _textDelegate = value;
        }

        internal virtual bool UseTextGlyphs()
        {
            return _textDelegate == null && _composition.Characters.Count > 0;
        }

        /// <summary>
        /// Gets or sets the scale on the current composition. The only cost of this function is re-rendering the
        /// current frame so you may call it frequent to scale something up or down.
        ///
        /// The smaller the animation is, the better the performance will be. You may find that scaling an
        /// animation down then rendering it in a larger ImageView and letting ImageView scale it back up
        /// with a scaleType such as centerInside will yield better performance with little perceivable
        /// quality loss.
        ///
        /// You can also use a fixed view width/height in conjunction with the normal ImageView
        /// scaleTypes centerCrop and centerInside.
        /// </summary>
        public virtual float Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                lock (this)
                {
                    UpdateBounds();
                    InvalidateMeasure();
                    InvalidateSelf();
                }
            }
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            InvalidateSelf();
            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Sets the image asset delegate.
        /// Use this if you can't bundle images with your app. This may be useful if you download the
        /// animations from the network or have the images saved to an SD Card. In that case, Lottie
        /// will defer the loading of the bitmap to this delegate.
        ///
        /// Be wary if you are using many images, however. Lottie is designed to work with vector shapes
        /// from After Effects. If your images look like they could be represented with vector shapes,
        /// see if it is possible to convert them to shape layers and re-export your animation. Check
        /// the documentation at http://airbnb.io/lottie for more information about importing shapes from
        /// Sketch or Illustrator to avoid this.
        /// </summary>
        public virtual IImageAssetDelegate ImageAssetDelegate
        {
            set
            {
                _imageAssetDelegate = value;
                if (_imageAssetManager != null)
                {
                    _imageAssetManager.Delegate = value;
                }
            }
        }

        /// <summary>
        /// Gets the internal <see cref="LottieComposition"/>
        /// </summary>
        public virtual LottieComposition Composition => _composition;

        private void UpdateBounds()
        {
            if (_composition == null)
            {
                return;
            }

            Width = (int)(_composition.Bounds.Width * _scale);
            Height = (int)(_composition.Bounds.Height * _scale);
            _bitmapCanvas?.Dispose();
            _bitmapCanvas = new BitmapCanvas(Width, Height);
        }

        /// <summary>
        /// Cancels the animation that is being executed
        /// </summary>
        public virtual void CancelAnimation()
        {
            _lazyCompositionTasks.Clear();
            _animator.Cancel();
        }

        /// <summary>
        /// Pauses the animation at the current <seealso cref="Frame"/>
        /// </summary>
        public void PauseAnimation()
        {
            _lazyCompositionTasks.Clear();
            _animator.PauseAnimation();
        }

        /// <summary>
        /// Gets the actual width of the current animation
        /// </summary>
        public int IntrinsicWidth => _composition == null ? -1 : (int)(_composition.Bounds.Width * _scale);

        /// <summary>
        /// Gets the actual height of the current animation
        /// </summary>
        public int IntrinsicHeight => _composition == null ? -1 : (int)(_composition.Bounds.Height * _scale);

        /// <summary>
        /// Takes a <see cref="KeyPath"/>, potentially with wildcards or globstars and resolve it to a list of
        /// zero or more actual <see cref="KeyPath"/>s
        /// that exist in the current animation.
        ///
        /// If you want to set value callbacks for any of these values, it is recommend to use the
        /// returned <see cref="KeyPath"/> objects because they will be internally resolved to their content
        /// and won't trigger a tree walk of the animation contents when applied.
        /// </summary>
        /// <param name="keyPath">The <see cref="KeyPath"/> to be resolved.</param>
        /// <returns>Returns a list of resolved <see cref="KeyPath"/>s, based on the keySet used as the parameter.</returns>
        public List<KeyPath> ResolveKeyPath(KeyPath keyPath)
        {
            if (_compositionLayer == null)
            {
                Debug.WriteLine("Cannot resolve KeyPath. Composition is not set yet.", LottieLog.Tag);
                return new List<KeyPath>();
            }

            var keyPaths = new List<KeyPath>();
            _compositionLayer.ResolveKeyPath(keyPath, 0, keyPaths, new KeyPath());
            return keyPaths;
        }

        /// <summary>
        /// Add an property callback for the specified <see cref="KeyPath"/>. This <see cref="KeyPath"/> can resolve
        /// to multiple contents. In that case, the callbacks's value will apply to all of them.
        ///
        /// Internally, this will check if the <see cref="KeyPath"/> has already been resolved with
        /// <see cref="ResolveKeyPath"/> and will resolve it if it hasn't.
        /// </summary>
        /// <typeparam name="T">The type that the callback will work on.</typeparam>
        /// <param name="keyPath">The <see cref="KeyPath"/> to be resolved.</param>
        /// <param name="property">The <see cref="LottieProperty"/> that the callback is listening to.</param>
        /// <param name="callback">The <see cref="ILottieValueCallback{T}"/> to add to the resolved <see cref="KeyPath"/></param>
        public void AddValueCallback<T>(KeyPath keyPath, LottieProperty property, ILottieValueCallback<T> callback)
        {
            if (_compositionLayer == null)
            {
                _lazyCompositionTasks.Add(composition =>
                {
                    AddValueCallback(keyPath, property, callback);
                });
                return;
            }

            bool invalidate;
            if (keyPath.GetResolvedElement() != null)
            {
                keyPath.GetResolvedElement().AddValueCallback(property, callback);
                invalidate = true;
            }
            else
            {
                List<KeyPath> elements = ResolveKeyPath(keyPath);

                for (int i = 0; i < elements.Count; i++)
                {
                    elements[i].GetResolvedElement().AddValueCallback(property, callback);
                }

                invalidate = elements.Any();
            }

            if (invalidate)
            {
                InvalidateSelf();
                if (property == LottieProperty.TimeRemap)
                {
                    // Time remapping values are read in setProgress. In order for the new value
                    // to apply, we have to re-set the progress with the current progress so that the
                    // time remapping can be reapplied.
                    Progress = Progress;
                }
            }
        }

        /// <summary>
        /// Overload of <see cref="AddValueCallback{T}(KeyPath, LottieProperty, ILottieValueCallback{T})"/> that takes an interface. This allows you to use a single abstract
        /// method code block in Kotlin such as:
        /// drawable.AddValueCallback(yourKeyPath, LottieProperty.Color) { yourColor }
        /// </summary>
        /// <typeparam name="T">The type that the callback will work on.</typeparam>
        /// <param name="keyPath">The <see cref="KeyPath"/> to be resolved.</param>
        /// <param name="property">The <see cref="LottieProperty"/> that the callback is listening to.</param>
        /// <param name="callback">The <see cref="SimpleLottieValueCallback{T}"/> to add to the resolved <see cref="KeyPath"/></param>
        public void AddValueCallback<T>(KeyPath keyPath, LottieProperty property, SimpleLottieValueCallback<T> callback)
        {
            AddValueCallback(keyPath, property, new SimpleImplLottieValueCallback<T>(callback));
        }

        /// <summary>
        /// Allows you to modify or clear a bitmap that was loaded for an image either automatically
        ///
        /// through <seealso cref="ImageAssetsFolder"/> or with an <seealso cref="ImageAssetDelegate"/>.
        /// </summary>
        /// <returns>If the bitmap parameter is null, returns the previously set bitmap, or else returns the same bitmap send thru the parameter</returns>
        public virtual CanvasBitmap UpdateBitmap(string id, CanvasBitmap bitmap)
        {
            var bm = ImageAssetManager;
            if (bm == null)
            {
                Debug.WriteLine("Cannot update bitmap. Most likely the drawable is not added to a View " + "which prevents Lottie from getting a Context.", LottieLog.Tag);
                return null;
            }

            var ret = bm.UpdateBitmap(id, bitmap);
            InvalidateSelf();
            return ret;
        }

        internal virtual CanvasBitmap GetImageAsset(string id)
        {
            return ImageAssetManager?.BitmapForId(_canvasControl.Device, id);
        }

        private ImageAssetManager ImageAssetManager
        {
            get
            {
                if (_imageAssetManager != null && false)
                {
                    _imageAssetManager.RecycleBitmaps();
                    _imageAssetManager = null;
                }

                if (_imageAssetManager == null)
                {
                    _imageAssetManager = new ImageAssetManager(ImageAssetsFolder, _imageAssetDelegate, _composition.Images);
                }

                return _imageAssetManager;
            }
        }

        internal virtual Typeface GetTypeface(string fontFamily, string style)
        {
            var assetManager = FontAssetManager;
            return assetManager?.GetTypeface(fontFamily, style);
        }

        private FontAssetManager FontAssetManager => _fontAssetManager ??
            (_fontAssetManager = new FontAssetManager(_fontAssetDelegate));

        private void Dispose(bool disposing)
        {
            _animator.Dispose();
            _imageAssetManager?.Dispose();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LottieDrawable"/> class.
        /// </summary>
        ~LottieDrawable()
        {
            Dispose(false);
        }

        private class ColorFilterData
        {
            private readonly string _layerName;
            private readonly string _contentName;
            private readonly ColorFilter _colorFilter;

            internal ColorFilterData(string layerName, string contentName, ColorFilter colorFilter)
            {
                _layerName = layerName;
                _contentName = contentName;
                _colorFilter = colorFilter;
            }

            public override int GetHashCode()
            {
                var hashCode = 17;
                if (!string.IsNullOrEmpty(_layerName))
                {
                    hashCode = hashCode * 31 * _layerName.GetHashCode();
                }

                if (!string.IsNullOrEmpty(_contentName))
                {
                    hashCode = hashCode * 31 * _contentName.GetHashCode();
                }

                return hashCode;
            }

            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }

                if (!(obj is ColorFilterData))
                {
                    return false;
                }

                var other = (ColorFilterData)obj;

                return GetHashCode() == other.GetHashCode() && _colorFilter == other._colorFilter;
            }
        }
    }
}
