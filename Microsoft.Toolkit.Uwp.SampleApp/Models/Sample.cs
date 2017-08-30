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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public class Sample
    {
        private static HttpClient client = new HttpClient();
        private static string _docsOnlineRoot = "https://raw.githubusercontent.com/Microsoft/UWPCommunityToolkit/";
        private string _cachedDocumentation = string.Empty;

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

        public string XamlCode { get; private set; }

        public string DocumentationUrl { get; set; }

        public string Icon { get; set; }

        public string BadgeUpdateVersionRequired { get; set; }

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
                return await codeStream.ReadTextAsync();
            }
        }

        public async Task<string> GetJavaScriptSourceAsync()
        {
            using (var codeStream = await StreamHelper.GetPackagedFileStreamAsync($"SamplePages/{Name}/{JavaScriptCodeFile}"))
            {
                return await codeStream.ReadTextAsync();
            }
        }

        public async Task<string> GetDocumentationAsync()
        {
            if (!string.IsNullOrWhiteSpace(_cachedDocumentation))
            {
                return _cachedDocumentation;
            }

            var filepath = string.Empty;
            var filename = string.Empty;
            var branch = "master";

            var docRegex = new Regex("^" + _docsOnlineRoot + "(?<branch>.+?)/docs/(?<file>.+)");
            var docMatch = docRegex.Match(DocumentationUrl);
            if (docMatch.Success)
            {
                branch = docMatch.Groups["branch"].Value;
                filepath = docMatch.Groups["file"].Value;
                filename = Path.GetFileName(filepath);
            }

            string modifiedDocumentationUrl = DocumentationUrl;

#if !DEBUG // only use the master branch for release mode
                modifiedDocumentationUrl = $"{_docsOnlineRoot}master/docs/{filepath}";
#endif

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

#if !DEBUG // don't cache for debugging perpuses so it always gets the latests
            if (string.IsNullOrWhiteSpace(_cachedDocumentation))
            {
                try
                {
                    _cachedDocumentation = await StorageFileHelper.ReadTextFromLocalCacheFileAsync(filename);
                }
                catch (Exception)
                {
                }
            }
#endif

            if (string.IsNullOrWhiteSpace(_cachedDocumentation))
            {
                try
                {
                    using (var localDocsStream = await StreamHelper.GetPackagedFileStreamAsync($"docs/{filepath}"))
                    {
                        var result = await localDocsStream.ReadTextAsync();
                        _cachedDocumentation = ProcessDocs(result);
                    }
                }
                catch (Exception)
                {
                }
            }

            return _cachedDocumentation;
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

            // Need to do some cleaning
            // Rework code tags
            var regex = new Regex("```(xaml|xml|csharp)(?<code>.+?)```", RegexOptions.Singleline);

            foreach (Match match in regex.Matches(result))
            {
                var code = match.Groups["code"].Value;
                var lines = code.Split('\n');
                var newCode = new StringBuilder();
                foreach (var line in lines)
                {
                    newCode.AppendLine("    " + line);
                }

                result = result.Replace(match.Value, newCode.ToString());
            }

            // Images
            regex = new Regex("## Example Image.+?##", RegexOptions.Singleline);
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
                    XamlCode = await codeStream.ReadTextAsync();

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
                                        var sliderOptions = new SliderPropertyOptions { DefaultValue = double.Parse(value) };
                                        var parameters = match.Groups["parameters"].Value;
                                        var split = parameters.Split('-');
                                        int minIndex = 0;
                                        int minMultiplier = 1;
                                        if (string.IsNullOrEmpty(split[0]))
                                        {
                                            minIndex = 1;
                                            minMultiplier = -1;
                                        }

                                        sliderOptions.MinValue = minMultiplier * double.Parse(split[minIndex]);
                                        sliderOptions.MaxValue = double.Parse(split[minIndex + 1]);
                                        if (split.Length > 2 + minIndex)
                                        {
                                            sliderOptions.Step = double.Parse(split[split.Length - 1]);
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
                                default:
                                    options = new PropertyOptions { DefaultValue = value };
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

            // Search in Windows
            var proxyType = VerticalAlignment.Center;
            var assembly = proxyType.GetType().GetTypeInfo().Assembly;

            foreach (var typeInfo in assembly.ExportedTypes)
            {
                if (typeInfo.Name == typeName)
                {
                    return typeInfo;
                }
            }

            // Search in Microsoft.Toolkit.Uwp.UI.Controls
            var controlsProxyType = GridSplitter.GridResizeDirection.Auto;
            assembly = controlsProxyType.GetType().GetTypeInfo().Assembly;

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
