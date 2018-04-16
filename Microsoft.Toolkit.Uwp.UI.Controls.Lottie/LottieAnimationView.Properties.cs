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
using System.Diagnostics;
using System.IO;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// This view will load, deserialize, and display an After Effects animation exported with
    /// bodymovin (https://github.com/bodymovin/bodymovin).
    /// <para>
    /// You may set the animation in one of two ways:
    /// 1) Attrs: <seealso cref="SourceProperty"/>
    /// 2) Programatically: <seealso cref="SetAnimationAsync(string)"/>, <seealso cref="Composition"/>,
    /// or <seealso cref="SetAnimationAsync(TextReader)"/>.
    /// </para>
    /// <para>
    /// You can set a default cache strategy with <seealso cref="CacheStrategy.None"/>.
    /// </para>
    /// <para>
    /// You can manually set the progress of the animation with <seealso cref="Progress"/> or
    /// <seealso cref="Progress"/>
    /// </para>
    /// </summary>
    public partial class LottieAnimationView
    {
        /// <summary>
        /// Dependency property associated with the <seealso cref="DefaultCacheStrategy"/> property
        /// </summary>
        public static readonly DependencyProperty DefaultCacheStrategyProperty = DependencyProperty.Register("DefaultCacheStrategy", typeof(CacheStrategy), typeof(LottieAnimationView), PropertyMetadata.Create(() => GlobalDefaultCacheStrategy));

        /// <summary>
        /// Dependency property associated with the <seealso cref="SourceProperty"/> property
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(LottieAnimationView), new PropertyMetadata(null, SourcePropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="AutoPlay"/> property
        /// </summary>
        public static readonly DependencyProperty AutoPlayProperty = DependencyProperty.Register("AutoPlay", typeof(bool), typeof(LottieAnimationView), new PropertyMetadata(false, AutoPlayPropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="ImageAssetsFolder"/> property
        /// </summary>
        public static readonly DependencyProperty ImageAssetsFolderProperty = DependencyProperty.Register("ImageAssetsFolder", typeof(string), typeof(LottieAnimationView), new PropertyMetadata(null, ImageAssetsFolderPropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="ColorFilter"/> property
        /// </summary>
        public static readonly DependencyProperty ColorFilterProperty = DependencyProperty.Register("ColorFilter", typeof(Color), typeof(LottieAnimationView), new PropertyMetadata(Colors.Transparent, ColorFilterPropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="Scale"/> property
        /// </summary>
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(1.0, ScalePropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="MinFrame"/> property
        /// </summary>
        public static readonly DependencyProperty MinFrameProperty = DependencyProperty.Register("MinFrame", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(0.0, MinFramePropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="MaxFrame"/> property
        /// </summary>
        public static readonly DependencyProperty MaxFrameProperty = DependencyProperty.Register("MaxFrame", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(0.0, MaxFramePropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="Speed"/> property
        /// </summary>
        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register("Speed", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(1.0, SpeedProperyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="Loop"/> property. Deprecated! Please refer to <seealso cref="RepeatCount"/>.
        /// </summary>
        [Obsolete]
        public static readonly DependencyProperty LoopProperty = DependencyProperty.Register("Loop", typeof(bool), typeof(LottieAnimationView), new PropertyMetadata(false, LoopPropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="RepeatMode"/> property
        /// </summary>
        public static readonly DependencyProperty RepeatModeProperty = DependencyProperty.Register("RepeatMode", typeof(RepeatMode), typeof(LottieAnimationView), new PropertyMetadata(RepeatMode.Restart, RepeatModePropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="RepeatCount"/> property
        /// </summary>
        public static readonly DependencyProperty RepeatCountProperty = DependencyProperty.Register("RepeatCount", typeof(int), typeof(LottieAnimationView), new PropertyMetadata(0, RepeatCountPropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="FrameRate"/> property
        /// </summary>
        public static readonly DependencyProperty FrameRateProperty = DependencyProperty.Register("FrameRate", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(60.0, FrameRatePropertyChangedCallback));

        /// <summary>
        /// Dependency property associated with the <seealso cref="Progress"/> property
        /// </summary>
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(0.0, ProgressPropertyChangedCallback));

        private static readonly DependencyProperty TimesRepeatedProperty = DependencyProperty.Register("TimesRepeated", typeof(int), typeof(LottieAnimationView), new PropertyMetadata(0));

        /// <summary>
        /// Gets or sets the static default cache strategy for all <see cref="LottieAnimationView"/>. This is used only as the default, which is Weak.
        /// You can also set it instance using the non-static property <see cref="DefaultCacheStrategy"/>.
        /// </summary>
        public static CacheStrategy GlobalDefaultCacheStrategy { get; set; } = CacheStrategy.Weak;

        /// <summary>
        /// Gets or sets the default cache strategy for this <see cref="LottieAnimationView"/>
        /// </summary>
        public CacheStrategy DefaultCacheStrategy
        {
            get => (CacheStrategy)GetValue(DefaultCacheStrategyProperty);
            set => SetValue(DefaultCacheStrategyProperty, value);
        }

        /// <summary>
        /// Gets or sets the current animation being executed. Sets has the same effect as <seealso cref="SetAnimationAsync(string)"/>
        /// </summary>
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        private static async void SourcePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                await lottieAnimationView.SetAnimationAsync((string)e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this animation should automatically plays whenever it is loaded
        /// </summary>
        public bool AutoPlay
        {
            get => (bool)GetValue(AutoPlayProperty);
            set => SetValue(AutoPlayProperty, value);
        }

        private static void AutoPlayPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var lottieAnimationView = dependencyObject as LottieAnimationView;
            if ((bool)e.NewValue)
            {
                lottieAnimationView?._lottieDrawable.PlayAnimation();
            }
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
        /// Be wary if you are using many images, however. Lottie is designed to work with vector shapes
        /// from After Effects.If your images look like they could be represented with vector shapes,
        /// see if it is possible to convert them to shape layers and re-export your animation.Check
        /// the documentation at http://airbnb.io/lottie for more information about importing shapes from
        /// Sketch or Illustrator to avoid this.
        /// </summary>
        public string ImageAssetsFolder
        {
            get => (string)GetValue(ImageAssetsFolderProperty);
            set => SetValue(ImageAssetsFolderProperty, value);
        }

        private static void ImageAssetsFolderPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.ImageAssetsFolder = (string)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets the default Color filter for this <see cref="LottieAnimationView"/>
        /// </summary>
        public Color ColorFilter
        {
            get => (Color)GetValue(ColorFilterProperty);
            set => SetValue(ColorFilterProperty, value);
        }

        private static void ColorFilterPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView.UpdateColorFilter();
            }
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
        public virtual double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        private static void ScalePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.Scale = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the minimum frame that the animation will start from when playing or looping.
        /// </summary>
        public double MinFrame
        {
            get { return (double)GetValue(MinFrameProperty); }
            set { SetValue(MinFrameProperty, value); }
        }

        private static void MinFramePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.MinFrame = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the maximum frame that the animation will end at when playing or looping.
        /// </summary>
        public double MaxFrame
        {
            get { return (double)GetValue(MaxFrameProperty); }
            set { SetValue(MaxFrameProperty, value); }
        }

        private static void MaxFramePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.MaxFrame = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the playback speed. If speed &lt; 0, the animation will play backwards.
        /// Returns the current playback speed. This will be &lt; 0 if the animation is playing backwards.
        /// </summary>
        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        private static void SpeedProperyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.Speed = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="RepeatCount"/> is Infinite (0) or not. Deprecated! Please refer to <seealso cref="RepeatCount"/>.
        /// </summary>
        [Obsolete]
        public bool Loop
        {
            get => (bool)GetValue(LoopProperty);
            set => SetValue(LoopProperty, value);
        }

        private static void LoopPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView && (bool)e.NewValue)
            {
                lottieAnimationView._lottieDrawable.RepeatCount = LottieDrawable.Infinite;
            }
        }

        /// <summary>
        /// Gets or sets what this animation should do when it reaches the end. This
        /// setting is applied only when the repeat count is either greater than
        /// 0 or <see cref="LottieDrawable.Infinite"/>. Defaults to <see cref="Lottie.RepeatMode.Restart"/>.
        /// Return either one of <see cref="Lottie.RepeatMode.Reverse"/> or <see cref="Lottie.RepeatMode.Restart"/>
        /// </summary>
        public RepeatMode RepeatMode
        {
            get { return (RepeatMode)GetValue(RepeatModeProperty); }
            set { SetValue(RepeatModeProperty, value); }
        }

        private static void RepeatModePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.RepeatMode = (RepeatMode)e.NewValue;
            }
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
            get { return (int)GetValue(RepeatCountProperty); }
            set { SetValue(RepeatCountProperty, value); }
        }

        private static void RepeatCountPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.RepeatCount = (int)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets the current frame rate that this animation is being executed
        /// </summary>
        public double FrameRate
        {
            get { return (double)GetValue(FrameRateProperty); }
            set { SetValue(FrameRateProperty, value); }
        }

        private static void FrameRatePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.FrameRate = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the progress of the animation. This value will be a double from 0 to 1.
        /// </summary>
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        private static void ProgressPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                var newValue = (float)Convert.ToDouble(e.NewValue);
                if (lottieAnimationView._lottieDrawable.Progress != newValue)
                {
                    lottieAnimationView._lottieDrawable.Progress = newValue;
                }
            }
        }

        private async void LottieDrawable_AnimatorUpdate(object sender, ValueAnimator.ValueAnimatorUpdateEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                Progress = _lottieDrawable.Progress;
            });
        }

        /// <summary>
        /// Gets the number of times that this animation finished since the last call to <seealso cref="PlayAnimation"/>.
        /// </summary>
        public int TimesRepeated
        {
            get { return (int)GetValue(TimesRepeatedProperty); }
            private set { SetValue(TimesRepeatedProperty, value); }
        }

        private async void LottieDrawable_AnimationRepeat(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                if (TimesRepeated != _lottieDrawable.TimesRepeated)
                {
                    TimesRepeated = _lottieDrawable.TimesRepeated;
                }
            });
        }

        /// <summary>
        /// Gets or sets a value indicating whether hardware acceleration is enabled for this view.
        /// READ THIS BEFORE ENABLING HARDWARE ACCELERATION:
        /// 1) Test your animation on the minimum API level you support. Some drawing features such as
        ///    dashes and stroke caps have min api levels
        ///    (https://developer.android.com/guide/topics/graphics/hardware-accel.html#unsupported)
        /// 2) Enabling hardware acceleration is not always more performant. Check it with your specific
        ///    animation only if you are having performance issues with software rendering.
        /// 3) Software rendering is safer and will be consistent across devices. Manufacturers can
        ///    potentially break hardware rendering with bugs in their SKIA engine. Lottie cannot do
        ///    anything about that.
        /// </summary>
        public bool HardwareAcceleration
        {
            get
            {
                return _useHardwareLayer;
            }

            set
            {
                _useHardwareLayer = value;
                EnableOrDisableHardwareLayer();
            }
        }

        /// <summary>
        /// Gets or sets a composition.
        /// You can set a default cache strategy if this view was inflated with xml by
        /// using <seealso cref="CacheStrategy"/>.
        /// </summary>
        public virtual LottieComposition Composition
        {
            get
            {
                return _composition;
            }

            set
            {
                Debug.WriteLine("Set Composition \n" + value, Tag);

                // lottieDrawable.Callback = this;
                _composition = value;
                var isNewComposition = _lottieDrawable.SetComposition(value);
                EnableOrDisableHardwareLayer();
                if (_viewbox?.Child == _lottieDrawable && !isNewComposition)
                {
                    // We can avoid re-setting the drawable, and invalidating the view, since the value
                    // hasn't changed.
                    return;
                }

                ImageDrawable = _lottieDrawable;

                FrameRate = _composition.FrameRate;
                MinFrame = _lottieDrawable.MinFrame;
                MaxFrame = _lottieDrawable.MaxFrame;
                Progress = _lottieDrawable.Progress;
                Speed = _lottieDrawable.Speed;
                TimesRepeated = _lottieDrawable.TimesRepeated;

                InvalidateArrange();
                InvalidateMeasure();
                _lottieDrawable.InvalidateSelf();
            }
        }

        /// <summary>
        /// Gets the starting frame of the <see cref="LottieComposition"/> associated with this <see cref="LottieAnimationView"/>
        /// </summary>
        public double StartFrame => _composition?.StartFrame ?? 0;

        /// <summary>
        /// Gets the ending frame of the <see cref="LottieComposition"/> associated with this <see cref="LottieAnimationView"/>
        /// </summary>
        public double EndFrame => _composition?.EndFrame ?? 0;

        /// <summary>
        /// Sets the minimum progress that the animation will start from when playing or looping.
        /// </summary>
        public float MinProgress
        {
            set => _lottieDrawable.MinProgress = value;
        }

        /// <summary>
        /// Sets the maximum progress that the animation will end at when playing or looping.
        /// </summary>
        public float MaxProgress
        {
            set => _lottieDrawable.MaxProgress = value;
        }

        /// <summary>
        /// Gets a value indicating whether the internal <see cref="ValueAnimator"/> is running or not
        /// </summary>
        public virtual bool IsAnimating => _lottieDrawable.IsAnimating;

        /// <summary>
        /// Gets or sets the currently rendered frame.
        /// Sets the progress to the specified frame.
        /// If the composition isn't set yet, the progress will be set to the frame when
        /// it is.
        /// </summary>
        public float Frame
        {
            get => _lottieDrawable.Frame;
            set => _lottieDrawable.Frame = value;
        }

        /// <summary>
        /// Gets the animation duration, in milliseconds
        /// </summary>
        public virtual long Duration => _composition != null ? (long)_composition.Duration : 0;

        /// <summary>
        /// Gets the <see cref="PerformanceTracker"/> object associated with this <see cref="LottieAnimationView"/>
        /// </summary>
        public virtual PerformanceTracker PerformanceTracker => _lottieDrawable.PerformanceTracker;

        /// <summary>
        /// Sets the <see cref="FontAssetDelegate"/> to be used on this <see cref="LottieAnimationView"/>. Use this to manually set fonts.
        /// </summary>
        public FontAssetDelegate FontAssetDelegate
        {
            set => _lottieDrawable.FontAssetDelegate = value;
        }

        /// <summary>
        /// Sets the <see cref="TextDelegate"/> to be used on this <see cref="LottieAnimationView"/>. Set this to replace animation text with custom text at runtime
        /// </summary>
        public TextDelegate TextDelegate
        {
            set => _lottieDrawable.TextDelegate = value;
        }

        /// <summary>
        /// Sets the current <see cref="LottieDrawable"/> associated with this <see cref="LottieAnimationView"/>
        /// </summary>
        public LottieDrawable ImageDrawable
        {
            set
            {
                if (_viewbox?.Child == value)
                {
                    return;
                }

                if (value != _lottieDrawable)
                {
                    RecycleBitmaps();
                }

                CancelLoaderTask();

                if (_viewbox == null)
                {
                    _viewbox = new Viewbox
                    {
                        Stretch = Stretch.Uniform,
                        StretchDirection = StretchDirection.DownOnly
                    };
                    Content = _viewbox;
                }

                _viewbox.Child = value;
            }
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
            set => _lottieDrawable.ImageAssetDelegate = value;
        }

        /// <summary>
        /// Sets a value indicating whether the performance tracking is enabled or not.
        /// </summary>
        public virtual bool PerformanceTrackingEnabled
        {
            set => _lottieDrawable.PerformanceTrackingEnabled = value;
        }
    }
}
