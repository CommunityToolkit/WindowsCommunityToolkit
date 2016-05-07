using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.VisualStudio
{
    public static class Constants
    {
        public static readonly string SERVICE_FOLDER_NAME = "UWP Toolkit";

        public static readonly string APP_ID_COLUMN_ID = "AppId";
        public static readonly string APP_ID_COLUMN_DISPLAY_NAME = "App Id / Key";
        public static readonly string APP_ID_COLUMN_PLACEHOLDER_VALUE = "<required>";
        
        public static readonly string APP_SECRET_COLUMN_ID = "AppSecret";
        public static readonly string APP_SECRET_COLUMN_DISPLAY_NAME = "App Secret";
        public static readonly string APP_SECRET_COLUMN_PLACEHOLDER_VALUE = "<required>";

        public static readonly string ACCESS_TOKEN_COLUMN_ID = "AccessToken";
        public static readonly string ACCESS_TOKEN_COLUMN_DISPLAY_NAME = "Access Token";
        public static readonly string ACCESS_TOKEN_COLUMN_PLACEHOLDER_VALUE = "<not set>";

        public static readonly string ACCESS_TOKEN_SECRET_COLUMN_ID = "AccessTokenSecret";
        public static readonly string ACCESS_TOKEN_SECRET_DISPLAY_NAME = "Access Token Secret";
        public static readonly string ACCESS_TOKEN_SECRET_PLACEHOLDER_VALUE = "<not set>";
    }
}
