using System;

namespace Microsoft.Toolkit.Uwp.Services.Weibo
{
    public class WeiboException : Exception
    {
        public WeiboError Error { get; set; }
    }
}