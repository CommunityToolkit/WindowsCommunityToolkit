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
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie;
using Microsoft.Toolkit.Uwp.UI.Media;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Path = System.IO.Path;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public class Sample
    {
        private const string _repoOnlineRoot = "https://raw.githubusercontent.com/Microsoft/UWPCommunityToolkit/";
        private const string _docsOnlineRoot = "https://raw.githubusercontent.com/MicrosoftDocs/UWPCommunityToolkitDocs/";

        private static HttpClient client = new HttpClient();
        private string _cachedDocumentation = string.Empty;
        private string _cachedPath = string.Empty;

        internal static async Task<Sample> FindAsync(string category, string name)
        {
            var categories = await Samples.GetCategoriesAsync();
            return categories?
                .FirstOrDefault(c => c.Name.Equals(category, StringComparison.OrdinalIgnoreCase))?
                .Samples
                .FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private PropertyDescriptor _propertyDescriptor;

        public string Name { get; set; }

        public string Type { get; set; }

        public string About { get; set; }

        private string _codeUrl;

        public string CodeUrl
        {
            get
            {
                return _codeUrl;
            }

            set
            {
#if DEBUG
                _codeUrl = value;
#else
                var regex = new Regex("^https://github.com/Microsoft/UWPCommunityToolkit/(tree|blob)/(?<branch>.+?)/(?<path>.*)");
                var docMatch = regex.Match(value);

                var branch = string.Empty;
                var path = string.Empty;
                if (docMatch.Success)
                {
                    branch = docMatch.Groups["branch"].Value;
                    path = docMatch.Groups["path"].Value;
                }

                if (string.IsNullOrWhiteSpace(branch))
                {
                    _codeUrl = value;
                }
                else
                {
                    _codeUrl = $"https://github.com/Microsoft/UWPCommunityToolkit/tree/master/{path}";
                }
#endif
            }
        }

        public string CodeFile { get; set; }

        public string JavaScriptCodeFile { get; set; }

        public string XamlCodeFile { get; set; }

        public bool DisableXamlEditorRendering { get; set; }

        public string XamlCode { get; private set; }

        public string DocumentationUrl { get; set; }

        public string Icon { get; set; }

        public string BadgeUpdateVersionRequired { get; set; }

        public string DeprecatedWarning { get; set; }

        public string ApiCheck { get; set; }

        public bool HasXAMLCode => !string.IsNullOrEmpty(XamlCodeFile);

        public bool HasCSharpCode => !string.IsNullOrEmpty(CodeFile);

        public bool HasJavaScriptCode => !string.IsNullOrEmpty(JavaScriptCodeFile);

        public bool HasDocumentation => !string.IsNullOrEmpty(DocumentationUrl);

        public bool IsSupported
        {
            get
            {
                if (ApiCheck == null)
                {
                    return true;
                }

                return ApiInformation.IsTypePresent(ApiCheck);
            }
        }

        public async Task<string> GetCSharpSourceAsync()
        {
            using (var codeStream = await StreamHelper.GetPackagedFileStreamAsync($"SamplePages/{Name}/{CodeFile}"))
            {
                using (var streamreader = new StreamReader(codeStream.AsStream()))
                {
                    return await streamreader.ReadToEndAsync();
                }
            }
        }

        public async Task<string> GetJavaScriptSourceAsync()
        {
            using (var codeStream = await StreamHelper.GetPackagedFileStreamAsync($"SamplePages/{Name}/{JavaScriptCodeFile}"))
            {
                using (var streamreader = new StreamReader(codeStream.AsStream()))
                {
                    return await streamreader.ReadToEndAsync();
                }
            }
        }

#pragma warning disable SA1009 // Doesn't like ValueTuples.
        public async Task<(string contents, string path)> GetDocumentationAsync()
#pragma warning restore SA1009 // Doesn't like ValueTuples.
        {
            if (!string.IsNullOrWhiteSpace(_cachedDocumentation))
            {
                return (_cachedDocumentation, _cachedPath);
            }

            var filepath = string.Empty;
            var filename = string.Empty;
            var localPath = string.Empty;

            var docRegex = new Regex("^" + _repoOnlineRoot + "(?<branch>.+?)/docs/(?<file>.+)");
            var docMatch = docRegex.Match(DocumentationUrl);
            if (docMatch.Success)
            {
                filepath = docMatch.Groups["file"].Value;
                filename = Path.GetFileName(filepath);
                localPath = $"ms-appx:///docs/{Path.GetDirectoryName(filepath)}/";
            }

#if !DEBUG // use the docs repo in release mode
            string modifiedDocumentationUrl = $"{_docsOnlineRoot}master/docs/{filepath}";

            _cachedPath = modifiedDocumentationUrl.Replace(filename, string.Empty);

            // Read from Cache if available.
            try
            {
                _cachedDocumentation = await StorageFileHelper.ReadTextFromLocalCacheFileAsync(filename);
            }
            catch (Exception)
            {
            }

            // Grab from docs repo if not.
            if (string.IsNullOrWhiteSpace(_cachedDocumentation))
            {
                try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(modifiedDocumentationUrl)))
                    {
                        using (var response = await client.SendAsync(request).ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                _cachedDocumentation = ProcessDocs(result);

                                if (!string.IsNullOrWhiteSpace(_cachedDocumentation))
                                {
                                    await StorageFileHelper.WriteTextToLocalCacheFileAsync(_cachedDocumentation, filename);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
#endif

            // Grab the local copy in Debug mode, allowing you to preview changes made.
            if (string.IsNullOrWhiteSpace(_cachedDocumentation))
            {
                try
                {
                    using (var localDocsStream = await StreamHelper.GetPackagedFileStreamAsync($"docs/{filepath}"))
                    {
                        var result = await localDocsStream.ReadTextAsync();
                        _cachedDocumentation = ProcessDocs(result);
                        _cachedPath = localPath;
                    }
                }
                catch (Exception)
                {
                }
            }

            return (_cachedDocumentation, _cachedPath);
        }

        /// <summary>
        /// Gets the image data from a Uri, with Caching.
        /// </summary>
        /// <param name="uri">Image Uri</param>
        /// <returns>Image Stream</returns>
        public async Task<IRandomAccessStream> GetImageStream(Uri uri)
        {
            async Task<Stream> CopyStream(HttpContent source)
            {
                var stream = new MemoryStream();
                await source.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }

            IRandomAccessStream imageStream = null;
            var localpath = $"{uri.Host}/{uri.LocalPath}";

            // Cache only in Release
#if !DEBUG
            try
            {
                imageStream = await StreamHelper.GetLocalCacheFileStreamAsync(localpath, Windows.Storage.FileAccessMode.Read);
            }
            catch
            {
            }
#endif

            if (imageStream == null)
            {
                try
                {
                    using (var response = await client.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var imageCopy = await CopyStream(response.Content);
                            imageStream = imageCopy.AsRandomAccessStream();

                            // Cache only in Release
#if !DEBUG
                            // Takes a second copy of the image stream, so that is can save the image data to cache.
                            using (var saveStream = await CopyStream(response.Content))
                            {
                                await SaveImageToCache(localpath, saveStream);
                            }
#endif
                        }
                    }
                }
                catch
                {
                }
            }

            return imageStream;
        }

        private async Task SaveImageToCache(string localpath, Stream imageStream)
        {
            var folder = ApplicationData.Current.LocalCacheFolder;
            localpath = Path.Combine(folder.Path, localpath);

            // Resort to creating using traditional methods to avoid iteration for folder creation.
            Directory.CreateDirectory(Path.GetDirectoryName(localpath));

            using (var filestream = File.Create(localpath))
            {
                await imageStream.CopyToAsync(filestream);
            }
        }

        private string ProcessDocs(string docs)
        {
            string result = docs;

            var metadataRegex = new Regex("^---(.+?)---", RegexOptions.Singleline);
            var metadataMatch = metadataRegex.Match(result);
            if (metadataMatch.Success)
            {
                result = result.Remove(metadataMatch.Index, metadataMatch.Index + metadataMatch.Length);
            }

            // Images
            var regex = new Regex("## Example Image.+?##", RegexOptions.Singleline);
            result = regex.Replace(result, "##");

            return result;
        }

        /// <summary>
        /// Gets a version of the XamlCode with the explicit values of the option controls.
        /// </summary>
        public string UpdatedXamlCode
        {
            get
            {
                if (_propertyDescriptor == null)
                {
                    return string.Empty;
                }

                var result = XamlCode;
                var proxy = (IDictionary<string, object>)_propertyDescriptor.Expando;
                foreach (var option in _propertyDescriptor.Options)
                {
                    if (proxy[option.Name] is ValueHolder value)
                    {
                        var newString = value.Value is Windows.UI.Xaml.Media.SolidColorBrush brush ?
                                            brush.Color.ToString() : value.Value.ToString();

                        result = result.Replace(option.OriginalString, newString);
                        result = result.Replace("@[" + option.Label + "]@", newString);
                        result = result.Replace("@[" + option.Label + "]", newString);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets a version of the XamlCode bound directly to the slider/option controls.
        /// </summary>
        public string BindedXamlCode
        {
            get
            {
                if (_propertyDescriptor == null)
                {
                    return string.Empty;
                }

                var result = XamlCode;
                var proxy = (IDictionary<string, object>)_propertyDescriptor.Expando;
                foreach (var option in _propertyDescriptor.Options)
                {
                    if (proxy[option.Name] is ValueHolder value)
                    {
                        result = result.Replace(
                            option.OriginalString,
                            "{Binding " + option.Name + ".Value, Mode=" + (option.IsTwoWayBinding ? "TwoWay" : "OneWay") + "}");
                        result = result.Replace(
                            "@[" + option.Label + "]@",
                            "{Binding " + option.Name + ".Value, Mode=TwoWay}");
                        result = result.Replace(
                            "@[" + option.Label + "]",
                            "{Binding " + option.Name + ".Value, Mode=OneWay}"); // Order important here.
                    }
                }

                return result;
            }
        }

        public PropertyDescriptor PropertyDescriptor => _propertyDescriptor;

        public async Task PreparePropertyDescriptorAsync()
        {
            if (string.IsNullOrEmpty(XamlCodeFile))
            {
                return;
            }

            if (_propertyDescriptor == null)
            {
                // Get Xaml code
                using (var codeStream = await StreamHelper.GetPackagedFileStreamAsync($"SamplePages/{Name}/{XamlCodeFile}"))
                {
                    XamlCode = await codeStream.ReadTextAsync(Encoding.UTF8);

                    // Look for @[] values and generate associated properties
                    var regularExpression = new Regex(@"@\[(?<name>.+?)(:(?<type>.+?):(?<value>.+?)(:(?<parameters>.+?))?(:(?<options>.*))*)?\]@?");

                    _propertyDescriptor = new PropertyDescriptor { Expando = new ExpandoObject() };
                    var proxy = (IDictionary<string, object>)_propertyDescriptor.Expando;

                    foreach (Match match in regularExpression.Matches(XamlCode))
                    {
                        var label = match.Groups["name"].Value;
                        var name = label.Replace(" ", string.Empty); // Allow us to have nicer display names, but create valid properties.
                        var type = match.Groups["type"].Value;
                        var value = match.Groups["value"].Value;

                        var existingOption = _propertyDescriptor.Options.Where(o => o.Name == name).FirstOrDefault();

                        if (existingOption == null && string.IsNullOrWhiteSpace(type))
                        {
                            throw new NotSupportedException($"Unrecognized short identifier '{name}'; Define type and parameters of property in first occurance in {XamlCodeFile}.");
                        }

                        if (Enum.TryParse(type, out PropertyKind kind))
                        {
                            if (existingOption != null)
                            {
                                if (existingOption.Kind != kind)
                                {
                                    throw new NotSupportedException($"Multiple options with same name but different type not supported: {XamlCodeFile}:{name}");
                                }

                                continue;
                            }

                            PropertyOptions options;

                            switch (kind)
                            {
                                case PropertyKind.Slider:
                                case PropertyKind.DoubleSlider:
                                    try
                                    {
                                        var sliderOptions = new SliderPropertyOptions { DefaultValue = double.Parse(value, CultureInfo.InvariantCulture) };
                                        var parameters = match.Groups["parameters"].Value;
                                        var split = parameters.Split('-');
                                        int minIndex = 0;
                                        int minMultiplier = 1;
                                        if (string.IsNullOrEmpty(split[0]))
                                        {
                                            minIndex = 1;
                                            minMultiplier = -1;
                                        }

                                        sliderOptions.MinValue = minMultiplier * double.Parse(split[minIndex], CultureInfo.InvariantCulture);
                                        sliderOptions.MaxValue = double.Parse(split[minIndex + 1], CultureInfo.InvariantCulture);
                                        if (split.Length > 2 + minIndex)
                                        {
                                            sliderOptions.Step = double.Parse(split[split.Length - 1], CultureInfo.InvariantCulture);
                                        }

                                        options = sliderOptions;
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Unable to extract slider info from {value}({ex.Message})");
                                        TrackingManager.TrackException(ex);
                                        continue;
                                    }

                                    break;

                                case PropertyKind.RangeSelectorMin:
                                case PropertyKind.RangeSelectorMax:
                                    try
                                    {
                                        var rangeSelectorOptions = new RangeSelectorPropertyOptions { DefaultValue = double.Parse(value, CultureInfo.InvariantCulture) };

                                        var parameters = match.Groups["parameters"].Value;
                                        rangeSelectorOptions.Value = double.Parse(parameters, CultureInfo.InvariantCulture);
                                        rangeSelectorOptions.Id = match.Groups["options"].Value;

                                        options = rangeSelectorOptions;
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Unable to extract range selector info from {value}({ex.Message})");
                                        TrackingManager.TrackException(ex);
                                        continue;
                                    }

                                    break;

                                case PropertyKind.TimeSpan:
                                    try
                                    {
                                        var sliderOptions = new SliderPropertyOptions { DefaultValue = TimeSpan.FromMilliseconds(double.Parse(value, CultureInfo.InvariantCulture)) };
                                        var parameters = match.Groups["parameters"].Value;
                                        var split = parameters.Split('-');
                                        int minIndex = 0;
                                        int minMultiplier = 1;
                                        if (string.IsNullOrEmpty(split[0]))
                                        {
                                            minIndex = 1;
                                            minMultiplier = -1;
                                        }

                                        sliderOptions.MinValue = minMultiplier * double.Parse(split[minIndex], CultureInfo.InvariantCulture);
                                        sliderOptions.MaxValue = double.Parse(split[minIndex + 1], CultureInfo.InvariantCulture);
                                        if (split.Length > 2 + minIndex)
                                        {
                                            sliderOptions.Step = double.Parse(split[split.Length - 1], CultureInfo.InvariantCulture);
                                        }

                                        options = sliderOptions;
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Unable to extract slider info from {value}({ex.Message})");
                                        TrackingManager.TrackException(ex);
                                        continue;
                                    }

                                    break;

                                case PropertyKind.Enum:
                                    try
                                    {
                                        options = new PropertyOptions();
                                        var split = value.Split('.');
                                        var typeName = string.Join(".", split.Take(split.Length - 1));
                                        var enumType = LookForTypeByName(typeName);
                                        options.DefaultValue = Enum.Parse(enumType, split.Last());
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Unable to parse enum from {value}({ex.Message})");
                                        TrackingManager.TrackException(ex);
                                        continue;
                                    }

                                    break;

                                case PropertyKind.Bool:
                                    try
                                    {
                                        options = new PropertyOptions { DefaultValue = bool.Parse(value) };
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Unable to parse bool from {value}({ex.Message})");
                                        continue;
                                    }

                                    break;

                                case PropertyKind.Brush:
                                    try
                                    {
                                        options = new PropertyOptions { DefaultValue = value };
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Unable to parse bool from {value}({ex.Message})");
                                        TrackingManager.TrackException(ex);
                                        continue;
                                    }

                                    break;

                                case PropertyKind.Thickness:
                                    try
                                    {
                                        var thicknessOptions = new ThicknessPropertyOptions { DefaultValue = value };
                                        options = thicknessOptions;
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Unable to extract thickness info from {value}({ex.Message})");
                                        TrackingManager.TrackException(ex);
                                        continue;
                                    }

                                    break;

                                default:
                                    try
                                    {
                                        var stringOptions = new StringPropertyOptions { DefaultValue = value };
                                        var parameters = match.Groups["parameters"]?.Value;
                                        if (string.IsNullOrEmpty(parameters) == false)
                                        {
                                            var split = parameters.Split('-');
                                            if (split.Length > 0)
                                            {
                                                stringOptions.UpdateSourceTrigger = (Windows.UI.Xaml.Data.UpdateSourceTrigger)Convert.ToInt32(split[0]);
                                            }
                                        }

                                        options = stringOptions;
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Unable to extract string info from {value}({ex.Message})");
                                        TrackingManager.TrackException(ex);
                                        continue;
                                    }

                                    break;
                            }

                            options.Label = label;
                            options.Name = name;
                            options.OriginalString = match.Value;
                            options.Kind = kind;
                            options.IsTwoWayBinding = options.OriginalString.EndsWith("@");
                            proxy[name] = new ValueHolder(options.DefaultValue);

                            _propertyDescriptor.Options.Add(options);
                        }
                    }
                }
            }
        }

        private static Type LookForTypeByName(string typeName)
        {
            // First search locally
            var result = System.Type.GetType(typeName);

            if (result != null)
            {
                return result;
            }

            var assemblies = new[]
            {
                VerticalAlignment.Center.GetType().GetTypeInfo().Assembly, // Search in Windows
                GridSplitter.GridResizeDirection.Auto.GetType().GetTypeInfo().Assembly, // Search in Microsoft.Toolkit.Uwp.UI.Controls
                RepeatMode.Restart.GetType().GetTypeInfo().Assembly, // Search in Microsoft.Toolkit.Uwp.UI.Controls.Lottie
                EasingType.Default.GetType().GetTypeInfo().Assembly, // Search in Microsoft.Toolkit.Uwp.UI.Animations
                ImageBlendMode.Multiply.GetType().GetTypeInfo().Assembly // Search in Microsoft.Toolkit.Uwp.UI
            };

            foreach (var assembly in assemblies)
            {
                var typeInfo = SearchTypeIn(assembly, typeName);
                if (typeInfo != null)
                {
                    return typeInfo;
                }
            }

            return null;
        }

        private static Type SearchTypeIn(Assembly assembly, string typeName)
        {
            foreach (var typeInfo in assembly.ExportedTypes)
            {
                if (typeInfo.Name == typeName)
                {
                    return typeInfo;
                }
            }

            return null;
        }
    }
}