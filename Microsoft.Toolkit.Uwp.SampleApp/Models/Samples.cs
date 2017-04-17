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
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class Samples
    {
        private static List<SampleCategory> _samplesCategories;

        public static async Task<SampleCategory> GetCategoryBySample(Sample sample)
        {
            return (await GetCategoriesAsync()).FirstOrDefault(c => c.Samples.Contains(sample));
        }

        public static async Task<SampleCategory> GetCategoryByName(string name)
        {
            return (await GetCategoriesAsync()).FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<Sample> GetSampleByName(string name)
        {
            return (await GetCategoriesAsync()).SelectMany(c => c.Samples).FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<Sample[]> FindSamplesByName(string name)
        {
            var query = name.ToLower();
            return (await GetCategoriesAsync()).SelectMany(c => c.Samples).Where(s => s.Name.ToLower().Contains(query)).ToArray();
        }

        public static async Task<List<SampleCategory>> GetCategoriesAsync()
        {
            if (_samplesCategories == null)
            {
                List<SampleCategory> allCategories;
                using (var jsonStream = await StreamHelper.GetPackagedFileStreamAsync("SamplePages/samples.json"))
                {
                    var jsonString = await jsonStream.ReadTextAsync();
                    allCategories = JsonConvert.DeserializeObject<List<SampleCategory>>(jsonString);
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
                            await sample.PreparePropertyDescriptorAsync();
                        }
                    }

                    if (finalSamples.Count > 0)
                    {
                        supportedCategories.Add(category);
                        category.Samples = finalSamples.OrderBy(s => s.Name).ToArray();
                    }
                }

                _samplesCategories = supportedCategories.ToList();
            }

            return _samplesCategories;
        }
    }
}
