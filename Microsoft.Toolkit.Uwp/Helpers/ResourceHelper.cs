using System.Diagnostics;
using Windows.ApplicationModel.Resources.Core;

namespace Microsoft.Toolkit.Uwp
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Resource Helper to help get strings from localized Resourcefiles
        /// </summary>
        /// <param name="resourceString">Put in string Resource like this --> "nameOfTheResourceString"</param>
        /// <returns>Output is a string with the value of the resource string the user asked for</returns>
        public static string GetString(string resourceString)
        {
            // Connect to the resource-files
            ResourceContext defaultContext = ResourceContext.GetForCurrentView();
            ResourceMap srm = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");

            // Prepare an empty string to fill with the resource string
            string text = "";

            // Get back the localized string, in case of failure show an error in the output window
            try
            {
                text = srm.GetValue(resourceString, defaultContext).ValueAsString;
            }
            catch
            {
                Debug.WriteLine("ResourceHelper - Could not find resource: " + resourceString);
            }

            return text;
        }
    }
}
