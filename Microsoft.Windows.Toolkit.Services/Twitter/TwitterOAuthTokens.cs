using Microsoft.Windows.Toolkit.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    [ConnectedServiceOAuth("Twitter")]
    public class TwitterOAuthTokens
    {
        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string AccessToken { get; set; }

        public string AccessTokenSecret { get; set; }
    }
}
