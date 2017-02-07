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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.Web.Http;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public class Sample
    {
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

        public string CodeUrl { get; set; }

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
            using (var request = new HttpHelperRequest(new Uri(DocumentationUrl), HttpMethod.Get))
            {
                using (var response = await HttpHelper.Instance.SendRequestAsync(request).ConfigureAwait(false))
                {
                    if (response.Success)
                    {
                        var result = await response.Content.ReadAsStringAsync();

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
                }
            }

            return string.Empty;
        }

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
                    var value = proxy[option.Name] as ValueHolder;
                    if (value != null)
                    {
                        result = result.Replace(option.OriginalString, value.Value.ToString());
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
                    var regularExpression = new Regex(@"@\[(?<name>.+?):(?<type>.+?):(?<value>.+?)(:(?<parameters>.+?))?(:(?<options>.*))*\]");

                    _propertyDescriptor = new PropertyDescriptor { Expando = new ExpandoObject() };
                    var proxy = (IDictionary<string, object>)_propertyDescriptor.Expando;

                    foreach (Match match in regularExpression.Matches(XamlCode))
                    {
                        var name = match.Groups["name"].Value;
                        var type = match.Groups["type"].Value;
                        var value = match.Groups["value"].Value;

                        PropertyKind kind;

                        if (Enum.TryParse(type, out kind))
                        {
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

                            options.Name = name;
                            options.OriginalString = match.Value;
                            options.Kind = kind;
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
