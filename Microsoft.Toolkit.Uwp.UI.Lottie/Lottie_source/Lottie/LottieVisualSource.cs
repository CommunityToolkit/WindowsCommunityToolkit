// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

#if DEBUG
// Uncomment this to slow down async awaits for testing.
//#define SlowAwaits
#endif

using Microsoft.Toolkit.Uwp.UI.Lottie.LottieData;
using Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Serialization;
using Microsoft.Toolkit.Uwp.UI.Lottie.LottieToWinComp;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Lottie
{
    /// <summary>
    /// A <see cref="CompositionSource"/> for a Lottie composition. This allows
    /// a Lottie to be specified as the source of a <see cref="Composition"/>.
    /// </summary>
    public sealed class LottieVisualSource : DependencyObject, IDynamicAnimatedVisualSource
    {
        readonly StorageFile _storageFile;
        EventRegistrationTokenTable<TypedEventHandler<IDynamicAnimatedVisualSource, object>> _compositionInvalidatedEventTokenTable;
        int _loadVersion;
        Uri _uriSource;
        ContentFactory _contentFactory;

        // Optimize Lotties by default. Optimization takes a little longer but usually produces much
        // more efficient translations. The only reason someone would turn optimization off is if
        // the time to translate is too high, but in that case the Lottie is probably going to perform
        // so badly on the machine that it won't really be usable with our without optimization.
        public static DependencyProperty OptionsProperty { get; } =
            RegisterDP(nameof(Options), LottieVisualOptions.Optimize);

        public static DependencyProperty UriSourceProperty { get; } =
            RegisterDP<Uri>(nameof(UriSource), null,
            (owner, oldValue, newValue) => owner.HandleUriSourcePropertyChanged(oldValue, newValue));

        #region DependencyProperty helpers

        static DependencyProperty RegisterDP<T>(string propertyName, T defaultValue) =>
            DependencyProperty.Register(propertyName, typeof(T), typeof(LottieVisualSource), new PropertyMetadata(defaultValue));

        static DependencyProperty RegisterDP<T>(string propertyName, T defaultValue, Action<LottieVisualSource, T, T> callback) =>
            DependencyProperty.Register(propertyName, typeof(T), typeof(LottieVisualSource),
                new PropertyMetadata(defaultValue, (d, e) => callback(((LottieVisualSource)d), (T)e.OldValue, (T)e.NewValue)));

        #endregion DependencyProperty helpers

        /// <summary>
        /// Constructor to allow a <see cref="LottieVisualSource"/> to be used in markup.
        /// </summary>
        public LottieVisualSource() { }

        /// <summary>
        /// Creates a <see cref="CompositionSource"/> from a <see cref="StorageFile"/>.
        /// </summary>
        public LottieVisualSource(StorageFile storageFile)
        {
            _storageFile = storageFile;
        }

        /// <summary>
        /// Sets options for how the Lottie is loaded.
        /// </summary>
        public LottieVisualOptions Options
        {
            get => (LottieVisualOptions)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of the JSON source file for this <see cref="LottieVisualSource"/>.
        /// </summary>
        public Uri UriSource
        {
            get => (Uri)GetValue(UriSourceProperty);
            set => SetValue(UriSourceProperty, value);
        }

        /// <summary>
        /// Called by XAML to convert a string to an <see cref="IAnimatedVisualSource"/>.
        /// </summary>
        public static LottieVisualSource CreateFromString(string uri)
        {
            var uriUri = StringToUri(uri);
            if (uriUri == null)
            {
                // TODO - throw?
                return null;
            }
            return new LottieVisualSource { UriSource = uriUri };
        }

        // TODO: accept IRandomAccessStream
        [DefaultOverload]
        public IAsyncAction SetSourceAsync(StorageFile file)
        {
            _uriSource = null;
            return LoadAsync(file == null ? null : new Loader(this, file)).AsAsyncAction();
        }

        public IAsyncAction SetSourceAsync(Uri sourceUri)
        {
            _uriSource = sourceUri;
            // Update the dependency property to keep it in sync with _uriSource.
            // This will not trigger loading because it will be seen as no change
            // from the current (just set) _uriSource value.
            UriSource = sourceUri;
            return LoadAsync(sourceUri == null ? null : new Loader(this, sourceUri)).AsAsyncAction();
        }

        // TODO: currently explicitly implemented interfaces are causing a problem with .NET Native. Make them implicit for now.
        public event TypedEventHandler<IDynamicAnimatedVisualSource, object> AnimatedVisualInvalidated
        {
            add
            {
                return EventRegistrationTokenTable<TypedEventHandler<IDynamicAnimatedVisualSource, object>>
                   .GetOrCreateEventRegistrationTokenTable(ref _compositionInvalidatedEventTokenTable)
                   .AddEventHandler(value);
            }

            remove
            {
                EventRegistrationTokenTable<TypedEventHandler<IDynamicAnimatedVisualSource, object>>
                   .GetOrCreateEventRegistrationTokenTable(ref _compositionInvalidatedEventTokenTable)
                    .RemoveEventHandler(value);
            }
        }

        // TODO: currently explicitly implemented interfaces are causing a problem with .NET Native. Make them implicit for now.
        public IAnimatedVisual TryCreateAnimatedVisual(
        //bool IAnimatedVisualSource.TryCreateAnimatedVisual(
            Compositor compositor,
            out object diagnostics)
        {

            if (_contentFactory == null)
            {
                diagnostics = null;
                return new Comp();
            }
            else
            {
                return _contentFactory.TryCreateAnimatedVisual(compositor, out diagnostics);
            }
        }

        void NotifyListenersThatCompositionChanged()
        {
            EventRegistrationTokenTable<TypedEventHandler<IDynamicAnimatedVisualSource, object>>
                .GetOrCreateEventRegistrationTokenTable(ref _compositionInvalidatedEventTokenTable)
                .InvocationList?.Invoke(this, null);
        }

        // Called when the UriSource property is updated.
        void HandleUriSourcePropertyChanged(Uri oldValue, Uri newValue)
        {
            if (newValue == _uriSource)
            {
                // Ignore if setting to the current value. This can't happen if the value
                // is being set via the DependencyProperty, but it will happen if the value
                // is set via SetSourceAsync, as _uriSource will have been set before this
                // is called.
                return;
            }
            _uriSource = newValue;
            StartLoading();
        }


        // Starts a LoadAsync and returns immediately.
        async void StartLoading() => await LoadAsync(new Loader(this, UriSource));

        // Starts loading. Completes the returned task when the load completes or is replaced by another
        // load.
        async Task LoadAsync(Loader loader)
        {
            var loadVersion = ++_loadVersion;

            var oldContentFactory = _contentFactory;
            _contentFactory = null;

            if (oldContentFactory != null)
            {
                // Notify all listeners that their existing content is no longer valid.
                // They should stop showing the content. We will notify them again when the
                // content changes.
                NotifyListenersThatCompositionChanged();
            }

            if (loader == null)
            {
                return;
            }

            var contentFactory = await loader.Load(Options);
            if (loadVersion != _loadVersion)
            {
                // Another load request came in before this one completed.
                return;
            }

            if (contentFactory == null)
            {
                // Load didn't produce anything.
                return;
            }

            // We are the the most recent load. Save the result.
            _contentFactory = contentFactory;

            // Notify all listeners that they should try to create their instance of the content again.
            NotifyListenersThatCompositionChanged();

            if (!contentFactory.CanInstantiate)
            {
                // The load did not produce any content. Throw an exception so the caller knows.
                throw new ArgumentException("Failed to load composition.");
            }
        }

        static Issue[] ToIssues(IEnumerable<(string Code, string Description)> issues)
            => issues.Select(issue => new Issue { Code = issue.Code, Description = issue.Description }).ToArray();

        // Handles loading a composition from a Lottie file.
        sealed class Loader
        {
            readonly LottieVisualSource _owner;
            readonly Uri _uri;
            readonly StorageFile _storageFile;

            internal Loader(LottieVisualSource owner, Uri uri)
            {
                _owner = owner;
                _uri = uri;
            }

            internal Loader(LottieVisualSource owner, StorageFile storageFile)
            {
                _owner = owner;
                _storageFile = storageFile;
            }

            // Null loader.
            internal Loader() { }

            // Asynchronously loads WinCompData from a Lottie file.
            public async Task<ContentFactory> Load(LottieVisualOptions Options)
            {
                if (_uri == null && _storageFile == null)
                {
                    // Request to load null. Return a null ContentFactory.
                    return null;
                }

                LottieVisualDiagnostics diagnostics = null;
                Stopwatch sw = null;
                if (Options.HasFlag(LottieVisualOptions.IncludeDiagnostics))
                {
                    sw = Stopwatch.StartNew();
                    diagnostics = new LottieVisualDiagnostics
                    {
                        Options = Options
                    };
                }

                var result = new ContentFactory(diagnostics);

                // Get the file name and contents.
                (var fileName, var jsonStream) = await GetFileStreamAsync();

                if (diagnostics != null)
                {
                    diagnostics.FileName = fileName;
                    diagnostics.ReadTime = sw.Elapsed;
                    sw.Restart();
                }

                if (jsonStream == null)
                {
                    // Failed to load ...
                    return result;
                }


                // Parsing large Lottie files can take significant time. Do it on
                // another thread.
                LottieData.LottieComposition lottieComposition = null;
                await CheckedAwait(Task.Run(() =>
                {
                    lottieComposition =
                        LottieCompositionReader.ReadLottieCompositionFromJsonStream(
                            jsonStream,
                            LottieCompositionReader.Options.IgnoreMatchNames,
                            out var readerIssues);

                    if (diagnostics != null)
                    {
                        diagnostics.JsonParsingIssues = ToIssues(readerIssues);
                    }
                }));

                if (diagnostics != null)
                {
                    diagnostics.ParseTime = sw.Elapsed;
                    sw.Restart();
                }

                if (lottieComposition == null)
                {
                    // Failed to load...
                    return result;
                }

                if (diagnostics != null)
                {
                    // Save the LottieComposition in the diagnostics so that the xml and codegen
                    // code can be derived from it.
                    diagnostics.LottieComposition = lottieComposition;

                    // For each marker, normalize to a progress value by subtracting the InPoint (so it is relative to the start of the animation)
                    // and dividing by OutPoint - InPoint
                    diagnostics.Markers = lottieComposition.Markers.Select(m =>
                    {
                        return new KeyValuePair<string, double>(m.Name, (m.Progress * lottieComposition.FramesPerSecond) / lottieComposition.Duration.TotalSeconds);
                    }).ToArray();

                    // Validate the composition and report if issues are found.
                    diagnostics.LottieValidationIssues = ToIssues(LottieCompositionValidator.Validate(lottieComposition));
                    diagnostics.ValidationTime = sw.Elapsed;
                    sw.Restart();
                }

                result.SetDimensions(width: lottieComposition.Width,
                                     height: lottieComposition.Height,
                                     duration: lottieComposition.Duration);


                // Translating large Lotties can take significant time. Do it on another thread.
                bool translateSucceeded = false;
                WinCompData.Visual wincompDataRootVisual = null;
                var optimizationEnabled = _owner.Options.HasFlag(LottieVisualOptions.Optimize);

                await CheckedAwait(Task.Run(() =>
                {
                    translateSucceeded = LottieToWinCompTranslator.TryTranslateLottieComposition(
                        lottieComposition,
                        false, // strictTranslation
                        true,  // Add descriptions for codegen comments
                        out wincompDataRootVisual,
                        out var translationIssues);

                    if (diagnostics != null)
                    {
                        diagnostics.TranslationIssues = ToIssues(translationIssues);
                        diagnostics.TranslationTime = sw.Elapsed;
                        sw.Restart();
                    }

                    // Optimize the resulting translation. This will usually significantly reduce the size of
                    // the Composition code, however it might slow down loading too much on complex Lotties.
                    if (translateSucceeded && optimizationEnabled)
                    {
                        // Optimize.
                        wincompDataRootVisual = WinCompData.CodeGen.Optimizer.Optimize(wincompDataRootVisual, ignoreCommentProperties:true);

                        if (diagnostics != null)
                        {
                            diagnostics.OptimizationTime = sw.Elapsed;
                            sw.Restart();
                        }
                    }

                }));


                if (!translateSucceeded)
                {
                    // Failed.
                    return result;
                }
                else
                {
                    if (diagnostics != null)
                    {
                        // Save the root visual so diagnostics can generate XML and codegen.
                        diagnostics.RootVisual = wincompDataRootVisual;
                    }
                    result.SetRootVisual(wincompDataRootVisual);
                    return result;
                }
            }

            Task<(string, Stream)> GetFileStreamAsync()
                => _storageFile != null
                    ? GetStorageFileStreamAsync(_storageFile)
                    : GetUriStreamAsync(_uri);


            Task<(string, string)> ReadFileAsync()
                    => _storageFile != null
                        ? ReadStorageFileAsync(_storageFile)
                        : ReadUriAsync(_uri);

            async Task<(string, string)> ReadUriAsync(Uri uri)
            {
                var absoluteUri = GetAbsoluteUri(uri);
                if (absoluteUri != null)
                {
                    if (absoluteUri.Scheme.StartsWith("ms-"))
                    {
                        return await ReadStorageFileAsync(await StorageFile.GetFileFromApplicationUriAsync(absoluteUri));
                    }
                    else
                    {
                        var winrtClient = new Windows.Web.Http.HttpClient();
                        var response = await winrtClient.GetAsync(absoluteUri);
                        var result = await response.Content.ReadAsStringAsync();
                        return (absoluteUri.LocalPath, result);
                    }
                }
                return (null, null);
            }

            async Task<(string, Stream)> GetUriStreamAsync(Uri uri)
            {
                var absoluteUri = GetAbsoluteUri(uri);
                if (absoluteUri != null)
                {
                    if (absoluteUri.Scheme.StartsWith("ms-"))
                    {
                        return await GetStorageFileStreamAsync(await StorageFile.GetFileFromApplicationUriAsync(absoluteUri));
                    }
                    else
                    {
                        var winrtClient = new Windows.Web.Http.HttpClient();
                        var response = await winrtClient.GetAsync(absoluteUri);

                        var result = await response.Content.ReadAsInputStreamAsync();
                        return (absoluteUri.LocalPath, result.AsStreamForRead());
                    }
                }
                return (null, null);
            }

            async Task<(string, string)> ReadStorageFileAsync(StorageFile storageFile)
            {
                Debug.Assert(storageFile != null);
                var result = await FileIO.ReadTextAsync(storageFile);
                return (storageFile.Name, result);
            }

            async Task<(string, Stream)> GetStorageFileStreamAsync(StorageFile storageFile)
            {
                var randomAccessStream = await storageFile.OpenReadAsync();
                return (storageFile.Name, randomAccessStream.AsStreamForRead());
            }

        }

        // Parses a string into an absolute URI, or null if the string is malformed.
        static Uri StringToUri(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
            {
                return null;
            }

            return GetAbsoluteUri(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        // Returns an absolute URI. Relative URIs are made relative to ms-appx:///
        static Uri GetAbsoluteUri(Uri uri)
        {
            if (uri == null)
            {
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                return uri;
            }

            return new Uri($"ms-appx:///{uri}", UriKind.Absolute);
        }

        // Information from which a composition's content can be instantiated. Contains the WinCompData
        // translation of a composition and some metadata.
        sealed class ContentFactory : IAnimatedVisualSource
        {
            readonly LottieVisualDiagnostics _diagnostics;
            WinCompData.Visual _wincompDataRootVisual;
            double _width;
            double _height;
            TimeSpan _duration;

            internal ContentFactory(LottieVisualDiagnostics diagnostics)
            {
                _diagnostics = diagnostics;
            }

            internal void SetDimensions(double width, double height, TimeSpan duration)
            {
                _width = width;
                _height = height;
                _duration = duration;
            }

            internal void SetRootVisual(WinCompData.Visual rootVisual)
            {
                // Ensure the visual is compatible with the current OS.
                // If it's not compatible, do not save the visual.
                var versionCompatiblity = ApiCompatibility.Analyze(rootVisual);
                if (IsRuntimeCompatible(versionCompatiblity))
                {
                    if (_diagnostics != null)
                    {
                        _diagnostics.IsCompatibleWithCurrentOS = true;
                    }
                    _wincompDataRootVisual = rootVisual;
                }
            }

            internal bool CanInstantiate => _wincompDataRootVisual != null;

            // Clones a new diagnostics object. Will return null if the factory
            // has no diagnostics object.
            LottieVisualDiagnostics GetDiagnosticsClone()
            {
                return _diagnostics != null ? _diagnostics.Clone() : null;
            }

            public IAnimatedVisual TryCreateAnimatedVisual(Compositor compositor, out object diagnostics)
            {
                var diags = GetDiagnosticsClone();
                diagnostics = diags;

                if (!CanInstantiate)
                {
                    return null;
                }
                else
                {
                    var sw = Stopwatch.StartNew();
                    var result = new Comp()
                    {
                        RootVisual = Instantiator.CreateVisual(compositor, _wincompDataRootVisual),
                        Size = new System.Numerics.Vector2((float)_width, (float)_height),
                        Duration = _duration
                    };

                    if (diags != null)
                    {
                        diags.InstantiationTime = sw.Elapsed;
                    }
                    return result;
                }
            }
        }

        public override string ToString()
        {
            // TODO - if there's a _contentFactory, it should store the identity and report here
            var identity = (_storageFile != null) ? _storageFile.Name : _uriSource?.ToString() ?? "";
            return $"LottieVisualSource({identity})";
        }

        /// <summary>
        /// Returns true if the given compatibility describes compatibility with the current operating system.
        /// </summary>
        static bool IsRuntimeCompatible(ApiCompatibility compatibility)
        {
            if (compatibility.RequiresCompositionGeometricClip &&
                !ApiInformation.IsTypePresent("Windows.UI.Composition.CompositionGeometricClip"))
            {
                return false;
            }
            return true;
        }

        sealed class Comp : IAnimatedVisual, IDisposable
        {
            public Visual RootVisual { get; set; }
            public TimeSpan Duration { get; set; }
            public System.Numerics.Vector2 Size { get; set; }
            public void Dispose()
            {
                RootVisual?.Dispose();
            }
        }


        #region DEBUG
        // For testing purposes, slows down a task.
#if SlowAwaits
        const int _checkedDelayMs = 5;
        async
#endif
        static Task CheckedAwait(Task task)
        {
#if SlowAwaits
            await Task.Delay(_checkedDelayMs);
            await task;
            await Task.Delay(_checkedDelayMs);
#else
            return task;
#endif
        }

        #endregion DEBUG
    }
}

