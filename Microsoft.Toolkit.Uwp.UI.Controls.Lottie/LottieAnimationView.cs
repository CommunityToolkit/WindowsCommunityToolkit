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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Newtonsoft.Json;
using Windows.Data.Json;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// This view will load, deserialize, and display an After Effects animation exported with
    /// bodymovin (https://github.com/bodymovin/bodymovin).
    /// <para>
    /// You may set the animation in one of two ways:
    /// 1) Attrs: <seealso cref="FileNameProperty"/>
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
    public class LottieAnimationView : UserControl, IDisposable
    {
        private static new readonly string Tag = typeof(LottieAnimationView).Name;

        /// <summary>
        /// Caching strategy for compositions that will be reused frequently.
        /// Weak or Strong indicates the GC reference strength of the composition in the cache.
        /// </summary>
        public enum CacheStrategy
        {
            /// <summary>
            /// Does not cache the <see cref="LottieComposition"/>
            /// </summary>
            None,

            /// <summary>
            /// Holds a weak reference to the <see cref="LottieComposition"/> once it is loaded and deserialized
            /// </summary>
            Weak,

            /// <summary>
            /// Holds a strong reference to the <see cref="LottieComposition"/> once it is loaded and deserialized
            /// </summary>
            Strong
        }

        private static readonly Dictionary<string, LottieComposition> AssetStrongRefCache = new Dictionary<string, LottieComposition>();
        private static readonly Dictionary<string, WeakReference<LottieComposition>> AssetWeakRefCache = new Dictionary<string, WeakReference<LottieComposition>>();

        private readonly LottieDrawable _lottieDrawable;

        /// <summary>
        /// Gets or sets the default cache strategy for this <see cref="LottieAnimationView"/>
        /// </summary>
        public CacheStrategy DefaultCacheStrategy
        {
            get => (CacheStrategy)GetValue(DefaultCacheStrategyProperty);
            set => SetValue(DefaultCacheStrategyProperty, value);
        }

        /// <summary>
        /// Dependency property associated with the <seealso cref="DefaultCacheStrategy"/> property
        /// </summary>
        public static readonly DependencyProperty DefaultCacheStrategyProperty =
            DependencyProperty.Register("DefaultCacheStrategy", typeof(CacheStrategy), typeof(LottieAnimationView), new PropertyMetadata(CacheStrategy.Weak));

        private string _animationName;

        // private bool wasAnimatingWhenDetached = false;
        private bool _useHardwareLayer;

        private CancellationTokenSource _compositionLoader;

        /// <summary>
        /// Can be null because it is created async
        /// </summary>
        private LottieComposition _composition;

        /// <summary>
        /// Gets or sets the current animation being executed. Sets has the same effect as <seealso cref="SetAnimationAsync(string)"/>
        /// </summary>
        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }

        /// <summary>
        /// Dependency property associated with the <seealso cref="FileNameProperty"/> property
        /// </summary>
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(LottieAnimationView), new PropertyMetadata(null, FileNamePropertyChangedCallback));

        private static async void FileNamePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
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

        /// <summary>
        /// Dependency property associated with the <seealso cref="AutoPlay"/> property
        /// </summary>
        public static readonly DependencyProperty AutoPlayProperty =
            DependencyProperty.Register("AutoPlay", typeof(bool), typeof(LottieAnimationView), new PropertyMetadata(false, AutoPlayPropertyChangedCallback));

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

        /// <summary>
        /// Dependency property associated with the <seealso cref="ImageAssetsFolder"/> property
        /// </summary>
        public static readonly DependencyProperty ImageAssetsFolderProperty =
            DependencyProperty.Register("ImageAssetsFolder", typeof(string), typeof(LottieAnimationView), new PropertyMetadata(null, ImageAssetsFolderPropertyChangedCallback));

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

        /// <summary>
        /// Dependency property associated with the <seealso cref="ColorFilter"/> property
        /// </summary>
        public static readonly DependencyProperty ColorFilterProperty =
            DependencyProperty.Register("ColorFilter", typeof(Color), typeof(LottieAnimationView), new PropertyMetadata(Colors.Transparent, ColorFilterPropertyChangedCallback));

        private static void ColorFilterPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView.UpdateColorFilter();
            }
        }

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
        /// Takes a <see cref="KeyPath"/>, potentially with wildcards or globstars and resolve it to a list of
        /// zero or more actual <see cref="KeyPath"/>s that exist in the current animation.
        ///
        /// If you want to set value callbacks for any of these values, it is recommend to use the
        /// returned <see cref="KeyPath"/> objects because they will be internally resolved to their content
        /// and won't trigger a tree walk of the animation contents when applied.
        /// </summary>
        /// <param name="keyPath">The <see cref="KeyPath"/> to be resolved.</param>
        /// <returns>Returns a list of resolved <see cref="KeyPath"/>s, based on the keySet used as the parameter.</returns>
        public List<KeyPath> ResolveKeyPath(KeyPath keyPath)
        {
            return _lottieDrawable.ResolveKeyPath(keyPath);
        }

        /// <summary>
        /// Add an property callback for the specified <see cref="KeyPath"/>. This KeyPath can resolve
        /// to multiple contents. In that case, the callbacks's value will apply to all of them.
        ///
        /// Internally, this will check if the KeyPath has already been resolved with
        /// <see cref="ResolveKeyPath"/> and will resolve it if it hasn't.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ILottieValueCallback{T}"/> that will be returned</typeparam>
        /// <param name="keyPath">The keyPath that will be used to resolve to the contents that it matches, to then apply the callback.</param>
        /// <param name="property">Which property that this callback will act on.</param>
        /// <param name="callback">The actual callback implementation.</param>
        public void AddValueCallback<T>(KeyPath keyPath, LottieProperty property, ILottieValueCallback<T> callback)
        {
            _lottieDrawable.AddValueCallback(keyPath, property, callback);
        }

        /// <summary>
        /// Overload of <see cref="AddValueCallback{T}(KeyPath, LottieProperty, ILottieValueCallback{T})"/> that takes an interface. This allows you to use a single abstract
        /// method code block in Kotlin such as:
        /// animationView.AddValueCallback(yourKeyPath, LottieProperty.Color) { yourColor }
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="SimpleLottieValueCallback{T}"/> that will be returned</typeparam>
        /// <param name="keyPath">The keyPath that will be used to resolve to the contents that it matches, to then apply the callback.</param>
        /// <param name="property">Which property that this callback will act on.</param>
        /// <param name="callback">The actual callback implementation.</param>
        public void AddValueCallback<T>(KeyPath keyPath, LottieProperty property, SimpleLottieValueCallback<T> callback)
        {
            _lottieDrawable.AddValueCallback(keyPath, property, new SimpleImplLottieValueCallback<T>(callback));
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

        /// <summary>
        /// Dependency property associated with the <seealso cref="Scale"/> property
        /// </summary>
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(1.0, ScalePropertyChangedCallback));

        private static void ScalePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.Scale = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LottieAnimationView"/> class.
        /// </summary>
        public LottieAnimationView()
        {
            _lottieDrawable = new LottieDrawable();
            _lottieDrawable.AnimatorUpdate += LottieDrawable_AnimatorUpdate;
            _lottieDrawable.AnimationRepeat += LottieDrawable_AnimationRepeat;

            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled && !string.IsNullOrEmpty(FileName))
            {
                SetAnimationAsync(FileName).RunSynchronously();
            }

            if (AutoPlay)
            {
                _lottieDrawable.PlayAnimation();
            }

            _lottieDrawable.RepeatCount = RepeatCount;

            EnableMergePathsForKitKatAndAbove(false);
            UpdateColorFilter();

            EnableOrDisableHardwareLayer();
        }

        private void UpdateColorFilter()
        {
            SimpleColorFilter filter = new SimpleColorFilter(ColorFilter);
            KeyPath keyPath = new KeyPath("**");
            var callback = new LottieValueCallback<ColorFilter>(filter);
            AddValueCallback(keyPath, LottieProperty.ColorFilter, callback);
        }

        private Viewbox _viewbox;

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

        internal virtual void RecycleBitmaps()
        {
            // AppCompatImageView constructor will set the image when set from xml
            // before LottieDrawable has been initialized
            _lottieDrawable?.RecycleBitmaps();
        }

        /// <summary>
        /// Enable this to get merge path support for devices running KitKat (19) and above.
        ///
        /// Merge paths currently don't work if the the operand shape is entirely contained within the
        /// first shape. If you need to cut out one shape from another shape, use an even-odd fill type
        /// instead of using merge paths.
        /// </summary>
        public void EnableMergePathsForKitKatAndAbove(bool enable)
        {
            _lottieDrawable.EnableMergePathsForKitKatAndAbove(enable);
        }

        /// <summary>
        /// Returns whether merge paths are enabled for KitKat and above.
        /// </summary>
        /// <returns>Returns true if merge paths are enabled</returns>
        public bool IsMergePathsEnabledForKitKatAndAbove()
        {
            return _lottieDrawable.IsMergePathsEnabledForKitKatAndAbove();
        }

        /// <summary>
        /// This method enables or disables hardware acceletation
        /// </summary>
        /// <param name="use">True will enable hardware acceleration</param>
        public virtual void UseExperimentalHardwareAcceleration(bool use = true)
        {
            HardwareAcceleration = use;
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
        /// Sets the animation from a file in the assets directory.
        /// This will load and deserialize the file asynchronously.
        /// <para>
        /// Will not cache the composition once loaded.
        /// </para>
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task SetAnimationAsync(string animationName)
        {
            await SetAnimationAsync(animationName, DefaultCacheStrategy);
        }

        /// <summary>
        /// Sets the animation from a file in the assets directory.
        /// This will load and deserialize the file asynchronously.
        /// <para>
        /// You may also specify a cache strategy. Specifying <seealso cref="CacheStrategy.Strong"/> will hold a
        /// strong reference to the composition once it is loaded
        /// and deserialized. <seealso cref="CacheStrategy.Weak"/> will hold a weak reference to said composition.
        /// </para>
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task SetAnimationAsync(string animationName, CacheStrategy cacheStrategy)
        {
            _animationName = animationName;
            if (AssetWeakRefCache.ContainsKey(animationName))
            {
                var compRef = AssetWeakRefCache[animationName];
                if (compRef.TryGetTarget(out LottieComposition lottieComposition))
                {
                    Composition = lottieComposition;
                    return;
                }
            }
            else if (AssetStrongRefCache.ContainsKey(animationName))
            {
                Composition = AssetStrongRefCache[animationName];
                return;
            }

            ClearComposition();
            CancelLoaderTask();

            var cancellationTokenSource = new CancellationTokenSource();

            _compositionLoader = cancellationTokenSource;

            var composition = await LottieComposition.Factory.FromAssetFileNameAsync(animationName, cancellationTokenSource.Token);

            if (cacheStrategy == CacheStrategy.Strong)
            {
                AssetStrongRefCache[animationName] = composition;
            }
            else if (cacheStrategy == CacheStrategy.Weak)
            {
                AssetWeakRefCache[animationName] = new WeakReference<LottieComposition>(composition);
            }

            Composition = composition;
        }

        /// <summary>
        /// <see cref="SetAnimationAsync(TextReader)"/> which is more efficient than using a <see cref="JsonObject"/>.
        /// For animations loaded from the network, use <see cref="SetAnimationFromJsonAsync(string)"/>
        ///
        /// If you must use a JsonObject, you can convert it to a StreamReader with:
        /// <code>new JsonReader(new StringReader(json.ToString()));</code>
        /// </summary>
        /// <param name="json">The <see cref="JsonObject"/> that will to be loaded.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Obsolete]
        public async Task SetAnimationAsync(JsonObject json)
        {
            await SetAnimationAsync(new StringReader(json.ToString()));
        }

        /// <summary>
        /// Sets the animation from json string. This is the ideal API to use when loading an animation
        /// over the network because you can use the raw response body here and a converstion to a
        /// JsonObject never has to be done.
        /// </summary>
        /// <param name="jsonString">The Json string that will to be loaded</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SetAnimationFromJsonAsync(string jsonString)
        {
            await SetAnimationAsync(new StringReader(jsonString));
        }

        /// <summary>
        /// Sets the animation from a TextReader.
        /// This will load and deserialize the file asynchronously.
        /// <para>
        /// This is particularly useful for animations loaded from the network. You can fetch the
        /// bodymovin json from the network and pass it directly here.
        /// </para>
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task SetAnimationAsync(TextReader reader)
        {
            ClearComposition();
            CancelLoaderTask();
            var cancellationTokenSource = new CancellationTokenSource();

            _compositionLoader = cancellationTokenSource;

            var composition = await LottieComposition.Factory.FromJsonReaderAsync(new JsonReader(reader), cancellationTokenSource.Token);

            if (composition != null)
            {
                Composition = composition;
            }

            _compositionLoader = null;
        }

        private void CancelLoaderTask()
        {
            if (_compositionLoader != null)
            {
                _compositionLoader.Cancel();
                _compositionLoader = null;
            }
        }

        /// <summary>
        /// Gets or sets a composition.
        /// You can set a default cache strategy if this view was inflated with xml by
        /// using <seealso cref="LottieAnimationView.CacheStrategy"/>.
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
        /// Returns whether or not any layers in this composition has masks.
        /// </summary>
        /// <returns>Returns true if any layer in this composition has a mask.</returns>
        public virtual bool HasMasks()
        {
            return _lottieDrawable.HasMasks();
        }

        /// <summary>
        /// Returns whether or not any layers in this composition has a matte layer.
        /// </summary>
        /// <returns>Returns true if any layer in this composition has a matte.</returns>
        public virtual bool HasMatte()
        {
            return _lottieDrawable.HasMatte();
        }

        /// <summary>
        /// Plays the animation from the beginning.If speed is &lt; 0, it will start at the end
        /// and play towards the beginning
        /// </summary>
        public virtual void PlayAnimation()
        {
            _lottieDrawable.PlayAnimation();
            EnableOrDisableHardwareLayer();
        }

        /// <summary>
        /// Continues playing the animation from its current position. If speed &lt; 0, it will play backwards
        /// from the current position.
        /// </summary>
        public virtual void ResumeAnimation()
        {
            _lottieDrawable.ResumeAnimation();
            EnableOrDisableHardwareLayer();
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
        /// Gets or sets the minimum frame that the animation will start from when playing or looping.
        /// </summary>
        public double MinFrame
        {
            get { return (double)GetValue(MinFrameProperty); }
            set { SetValue(MinFrameProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with the <seealso cref="MinFrame"/> property
        /// </summary>
        public static readonly DependencyProperty MinFrameProperty = DependencyProperty.Register("MinFrame", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(0.0, MinFramePropertyChangedCallback));

        private static void MinFramePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.MinFrame = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Sets the minimum progress that the animation will start from when playing or looping.
        /// </summary>
        public float MinProgress
        {
            set => _lottieDrawable.MinProgress = value;
        }

        /// <summary>
        /// Gets or sets the maximum frame that the animation will end at when playing or looping.
        /// </summary>
        public double MaxFrame
        {
            get { return (double)GetValue(MaxFrameProperty); }
            set { SetValue(MaxFrameProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with the <seealso cref="MaxFrame"/> property
        /// </summary>
        public static readonly DependencyProperty MaxFrameProperty =
            DependencyProperty.Register("MaxFrame", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(0.0, MaxFramePropertyChangedCallback));

        private static void MaxFramePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.MaxFrame = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Sets the maximum progress that the animation will end at when playing or looping.
        /// </summary>
        public float MaxProgress
        {
            set => _lottieDrawable.MaxProgress = value;
        }

        /// <summary>
        /// <see cref="MinFrame"/>
        /// <see cref="MaxFrame"/>
        /// </summary>
        /// <param name="minFrame">The minimum frame that the animation will start from when playing or looping.</param>
        /// <param name="maxFrame">The maximum frame that the animation will end at when playing or looping.</param>
        public void SetMinAndMaxFrame(float minFrame, float maxFrame)
        {
            _lottieDrawable.SetMinAndMaxFrame(minFrame, maxFrame);
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

            _lottieDrawable.SetMinAndMaxProgress(minProgress, maxProgress);
        }

        /// <summary>
        /// Reverses the current animation speed. This does NOT play the animation.
        /// <see cref="Speed"/>
        /// <see cref="PlayAnimation"/>
        /// <see cref="ResumeAnimation"/>
        /// </summary>
        public void ReverseAnimationSpeed()
        {
            _lottieDrawable.ReverseAnimationSpeed();
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

        /// <summary>
        /// Dependency property associated with the <seealso cref="Speed"/> property
        /// </summary>
        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(1.0, SpeedProperyChangedCallback));

        private static void SpeedProperyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.Speed = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// This event will be invoked whenever the frame of the animation changes.
        /// </summary>
        public event EventHandler<ValueAnimator.ValueAnimatorUpdateEventArgs> AnimatorUpdate
        {
            add => _lottieDrawable.AnimatorUpdate += value;
            remove => _lottieDrawable.AnimatorUpdate -= value;
        }

        /// <summary>
        /// Clears the <seealso cref="AnimatorUpdate"/> event handler.
        /// </summary>
        public void RemoveAllUpdateListeners()
        {
            _lottieDrawable.RemoveAllUpdateListeners();
        }

        /// <summary>
        /// This event will be invoked whenever the internal animator is executed.
        /// </summary>
        public event EventHandler ValueChanged
        {
            add => _lottieDrawable.ValueChanged += value;
            remove => _lottieDrawable.ValueChanged -= value;
        }

        /// <summary>
        /// Clears the <seealso cref="ValueChanged"/> event handler.
        /// </summary>
        public void RemoveAllAnimatorListeners()
        {
            _lottieDrawable.RemoveAllAnimatorListeners();
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

        /// <summary>
        /// Dependency property associated with the <seealso cref="Loop"/> property. Deprecated! Please refer to <seealso cref="RepeatCount"/>.
        /// </summary>
        [Obsolete]
        public static readonly DependencyProperty LoopProperty =
            DependencyProperty.Register("Loop", typeof(bool), typeof(LottieAnimationView), new PropertyMetadata(false, LoopPropertyChangedCallback));

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

        /// <summary>
        /// Dependency property associated with the <seealso cref="RepeatMode"/> property
        /// </summary>
        public static readonly DependencyProperty RepeatModeProperty =
            DependencyProperty.Register("RepeatMode", typeof(RepeatMode), typeof(LottieAnimationView), new PropertyMetadata(RepeatMode.Restart, RepeatModePropertyChangedCallback));

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

        /// <summary>
        /// Dependency property associated with the <seealso cref="RepeatCount"/> property
        /// </summary>
        public static readonly DependencyProperty RepeatCountProperty =
            DependencyProperty.Register("RepeatCount", typeof(int), typeof(LottieAnimationView), new PropertyMetadata(0, RepeatCountPropertyChangedCallback));

        private static void RepeatCountPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.RepeatCount = (int)e.NewValue;
            }
        }

        /// <summary>
        /// Gets the number of times that this animation finished since the last call to <seealso cref="PlayAnimation"/>.
        /// </summary>
        public int TimesRepeated
        {
            get { return (int)GetValue(TimesRepeatedProperty); }
            private set { SetValue(TimesRepeatedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimesRepeated.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty TimesRepeatedProperty =
            DependencyProperty.Register("TimesRepeated", typeof(int), typeof(LottieAnimationView), new PropertyMetadata(0));

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
        /// Gets or sets the current frame rate that this animation is being executed
        /// </summary>
        public double FrameRate
        {
            get { return (double)GetValue(FrameRateProperty); }
            set { SetValue(FrameRateProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with the <seealso cref="FrameRate"/> property
        /// </summary>
        public static readonly DependencyProperty FrameRateProperty =
            DependencyProperty.Register("FrameRate", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(60.0, FrameRatePropertyChangedCallback));

        private static void FrameRatePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is LottieAnimationView lottieAnimationView)
            {
                lottieAnimationView._lottieDrawable.FrameRate = (float)Convert.ToDouble(e.NewValue);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the internal <see cref="ValueAnimator"/> is running or not
        /// </summary>
        public virtual bool IsAnimating => _lottieDrawable.IsAnimating;

        /// <summary>
        /// Allows you to modify or clear a bitmap that was loaded for an image either automatically
        /// through ImageAssetsFolder or with an ImageAssetDelegate.
        /// Return the previous Bitmap or null.
        /// </summary>
        /// <returns>If the bitmap parameter is null, returns the previously set bitmap, or else returns the same bitmap send thru the parameter</returns>
        public CanvasBitmap UpdateBitmap(string id, CanvasBitmap bitmap)
        {
            return _lottieDrawable.UpdateBitmap(id, bitmap);
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
        /// Cancels the animations completely.
        /// </summary>
        public virtual void CancelAnimation()
        {
            _lottieDrawable.CancelAnimation();
            EnableOrDisableHardwareLayer();
        }

        /// <summary>
        /// Pauses the animation at the current <seealso cref="Frame"/>
        /// </summary>
        public virtual void PauseAnimation()
        {
            _lottieDrawable.PauseAnimation();
            EnableOrDisableHardwareLayer();
        }

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
        /// Gets or sets the progress of the animation. This value will be a double from 0 to 1.
        /// </summary>
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with the <seealso cref="Progress"/> property
        /// </summary>
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(LottieAnimationView), new PropertyMetadata(0.0, ProgressPropertyChangedCallback));

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
        /// Gets the animation duration, in milliseconds
        /// </summary>
        public virtual long Duration => _composition != null ? (long)_composition.Duration : 0;

        /// <summary>
        /// Sets a value indicating whether the performance tracking is enabled or not.
        /// </summary>
        public virtual bool PerformanceTrackingEnabled
        {
            set => _lottieDrawable.PerformanceTrackingEnabled = value;
        }

        /// <summary>
        /// Gets the <see cref="PerformanceTracker"/> object associated with this <see cref="LottieAnimationView"/>
        /// </summary>
        public virtual PerformanceTracker PerformanceTracker => _lottieDrawable.PerformanceTracker;

        private void ClearComposition()
        {
            _composition = null;
            _lottieDrawable.ClearComposition();
        }

        private void EnableOrDisableHardwareLayer()
        {
            var useHardwareLayer = _useHardwareLayer && _lottieDrawable.IsAnimating;
            _lottieDrawable.ForceSoftwareRenderer(!useHardwareLayer);
        }

        private void Dispose(bool disposing)
        {
            _compositionLoader?.Dispose();
            _lottieDrawable.Dispose();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LottieAnimationView"/> class.
        /// </summary>
        ~LottieAnimationView()
        {
            Dispose(false);
        }

        /// <inheritdoc/>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FrameworkElementAutomationPeer(this);
        }
    }
}