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

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class Samples
    {
        private static SampleCategory[] _samplesCategories;

        public static async Task<SampleCategory[]> GetCategoriesAsync()
        {
            SampleCategory[] allCategories;

            if (_samplesCategories == null)
            {
                using (var jsonStream = await StreamHelper.GetPackagedFileStreamAsync("SamplePages/samples.json"))
                {
                    var jsonString = await jsonStream.ReadTextAsync();
                    allCategories = JsonConvert.DeserializeObject<SampleCategory[]>(jsonString);
                }

                // Check API
                var supportedCategories = new List<SampleCategory>();
                foreach (var category in allCategories)
                {
                    var finalSamples = new List<Sample>();

                    foreach (var sample in category.Samples)
                    {
                        if (sample.IsSupported)
                        {
                            finalSamples.Add(sample);
                        }
                    }

                    if (finalSamples.Count > 0)
                    {
                        supportedCategories.Add(category);
                        category.Samples = finalSamples.ToArray();
                    }
                }

                _samplesCategories = supportedCategories.ToArray();
            }

            return _samplesCategories;
        }
    }
}
