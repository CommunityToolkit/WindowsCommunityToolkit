using Microsoft.Windows.Toolkit.Services.Core;

namespace Microsoft.Windows.Toolkit.Services.Facebook
{
    [ConnectedServiceDataConfig("Facebook")]
    public class FacebookDataConfig
    {
        [ConnectedServiceDataConfigQueryStringParam("Facebook")]
        public string UserId { get; set; }
    }
}
