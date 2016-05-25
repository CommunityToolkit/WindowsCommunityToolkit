using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                using (var jsonStream = await Helpers.GetPackagedFileStreamAsync("SamplePages/samples.json"))
                {
                    var jsonString = await jsonStream.ReadTextAsync();
                    _samplesCategories = JsonConvert.DeserializeObject<SampleCategory[]>(jsonString);
                }
            }

            return _samplesCategories;
        }
    }
}
