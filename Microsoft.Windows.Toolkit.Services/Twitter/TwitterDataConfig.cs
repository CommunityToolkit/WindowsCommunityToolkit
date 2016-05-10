using Microsoft.Windows.Toolkit.Services.Core;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    [ConnectedServiceDataConfig("Twitter")]
    public class TwitterDataConfig
    {
        public TwitterQueryType QueryType { get; set; }

        [ConnectedServiceDataConfigQueryStringParam("Facebook")]
        public string Query { get; set; }
    }
}
