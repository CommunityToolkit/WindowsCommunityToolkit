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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Windows.Data.Json;
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
    public partial class LottieAnimationView : UserControl, IDisposable
    {
        private static new readonly string Tag = typeof(LottieAnimationView).Name;

        private static readonly Dictionary<string, LottieComposition> AssetStrongRefCache = new Dictionary<string, LottieComposition>();
        private static readonly Dictionary<string, WeakReference<LottieComposition>> AssetWeakRefCache = new Dictionary<string, WeakReference<LottieComposition>>();

        private readonly LottieDrawable _lottieDrawable;

        private string _animationName;

        private bool _useHardwareLayer;

        private CancellationTokenSource _compositionLoader;

        /// <summary>
        /// Can be null because it is created async
        /// </summary>
        private LottieComposition _composition;

        private Viewbox _viewbox;

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
        /// Initializes a new instance of the <see cref="LottieAnimationView"/> class.
        /// </summary>
        public LottieAnimationView()
        {
            _lottieDrawable = new LottieDrawable();
            _lottieDrawable.AnimatorUpdate += LottieDrawable_AnimatorUpdate;
            _lottieDrawable.AnimationRepeat += LottieDrawable_AnimationRepeat;

            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled && !string.IsNullOrEmpty(Source))
            {
                SetAnimationAsync(Source).RunSynchronously();
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