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

            // Put in string Resource like this --> "nameOfTheResourceString"
            // Get back the localized string
            string text = srm.GetValue(resourceString, defaultContext).ValueAsString;
            return text;
        }
    }
}
