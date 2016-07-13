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
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Windows.Toolkit.SampleApp
{
    public static class Samples
    {
        private static SampleCategory[] _samplesCategories;

        public static async Task<SampleCategory[]> GetCategoriesAsync()
        {
            if (_samplesCategories == null)
            {
                using (var jsonStream = await StreamHelper.GetPackagedFileStreamAsync("SamplePages/samples.json"))
                {
                    var jsonString = await jsonStream.ReadTextAsync();
                    _samplesCategories = JsonConvert.DeserializeObject<SampleCategory[]>(jsonString);
                }
            }

            return _samplesCategories;
        }
    }
}
