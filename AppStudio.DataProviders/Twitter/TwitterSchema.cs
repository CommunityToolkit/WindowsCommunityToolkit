using System;
using System.Globalization;

namespace AppStudio.DataProviders.Twitter
{
    public class TwitterSchema : SchemaBase
    {
        public string Text { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserScreenName { get; set; }

        public string UserProfileImageUrl { get; set; }

        public string Url { get; set; }
    }
}
