using System;

namespace AppStudio.DataProviders.Bing
{
    /// <summary>
    /// Implementation of the BingSchema class.
    /// </summary>
    public class BingSchema : SchemaBase
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Link { get; set; }

        public DateTime Published { get; set; }
    }
}
