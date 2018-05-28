// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.Rest;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PowerBI embedded control is a simple wrapper to an IFRAME for a PowerBI embed.
    /// </summary>
    public partial class PowerBiEmbedded : Control
    {
        private const string Authority = "https://login.microsoftonline.com/common";
        private const string ResourceId = "https://analysis.windows.net/powerbi/api";
        private const string ApiUrl = "https://api.powerbi.com/";
        private const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        private ComboBox _cbxReportList;
        private WebView _webViewReportFrame;
        private TaskCompletionSource<bool> _webViewInitializedTask = new TaskCompletionSource<bool>();
        private string _tokenForUser;
        private DateTimeOffset _expiration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerBiEmbedded"/> class.
        /// </summary>
        public PowerBiEmbedded()
        {
            DefaultStyleKey = typeof(PowerBiEmbedded);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            ApplyTemplate();

            _cbxReportList = GetTemplateChild("CbxReportList") as ComboBox;

            _webViewReportFrame = GetTemplateChild("WebViewReportFrame") as WebView;

            if (_webViewReportFrame != null)
            {
                _webViewReportFrame.DOMContentLoaded += (WebView sender, WebViewDOMContentLoadedEventArgs args) =>
                {
                    _webViewInitializedTask.TrySetResult(true);
                };
            }
        }

        private async Task<string> GetUserTokenAsync(string appClientId)
        {
            try
            {
                var azureAdContext = new AuthenticationContext(Authority);

                if (_tokenForUser == null)
                {
                    AuthenticationResult userAuthnResult = await azureAdContext.AcquireTokenAsync(ResourceId, appClientId, new Uri(DefaultRedirectUri), new PlatformParameters(PromptBehavior.Auto, false));
                    _tokenForUser = userAuthnResult.AccessToken;
                    _expiration = userAuthnResult.ExpiresOn;
                }

                if (_expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
                {
                    AuthenticationResult userAuthnResult = await azureAdContext.AcquireTokenSilentAsync(ResourceId, appClientId);
                    _tokenForUser = userAuthnResult.AccessToken;
                    _expiration = userAuthnResult.ExpiresOn;
                }
            }
            catch (AdalException ex)
            {
                if (ex.ErrorCode != "authentication_canceled")
                {
                    throw ex;
                }
            }

            return _tokenForUser;
        }

        private Task<bool> WaitWebviewContentLoaded()
        {
            return _webViewInitializedTask.Task;
        }

        private async void LoadReport(string embedReportId, string embedUrl)
        {
            if (!string.IsNullOrEmpty(ClientId.Trim()) && !string.IsNullOrEmpty(embedReportId) && !string.IsNullOrEmpty(embedUrl.Trim()))
            {
                await WaitWebviewContentLoaded();

                await _webViewReportFrame.InvokeScriptAsync(
                    "eval",
                    new string[]
                    {
                        $"loadPowerBiReport('{await GetUserTokenAsync(ClientId)}', '{embedUrl}', '{embedReportId}')"
                    });
            }
        }

        private async void LoadReport()
        {
            if (!string.IsNullOrEmpty(ClientId.Trim()) && !string.IsNullOrEmpty(EmbedUrl.Trim()))
            {
                if (Uri.TryCreate(EmbedUrl, UriKind.Absolute, out Uri embedUri))
                {
                    if (embedUri.Query.IndexOf("reportid", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        var decoder = new WwwFormUrlDecoder(embedUri.Query.ToLower());
                        LoadReport(
                            decoder.GetFirstValueByName("reportid"),
                            EmbedUrl);
                    }
                }
                else
                {
                    throw new ArgumentException(nameof(EmbedUrl));
                }
            }
            else
            {
                await ClearReport();
            }
        }

        private async void LoadGroup()
        {
            if (!string.IsNullOrEmpty(ClientId.Trim()) && !string.IsNullOrEmpty(GroupId.Trim()))
            {
                string token = await GetUserTokenAsync(ClientId);

                if (!string.IsNullOrEmpty(token))
                {
                    var tokenCredentials = new TokenCredentials(token, "Bearer");

                    using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
                    {
                        Reports = (await client.Reports.GetReportsAsync(GroupId))?.Value;

                        if (Reports.Count > 0 && SelectionReport == null)
                        {
                            SelectionReport = Reports.First();
                        }
                    }
                }
            }
            else
            {
                Reports = null;
                await ClearReport();
            }
        }

        private async void LoadAll()
        {
            if (!string.IsNullOrEmpty(ClientId.Trim()))
            {
                if (!string.IsNullOrEmpty(GroupId))
                {
                    LoadGroup();
                }

                if (!string.IsNullOrEmpty(EmbedUrl))
                {
                    LoadReport();
                }
            }
            else
            {
                await ClearReport();
            }
        }

        private async Task ClearReport()
        {
            await WaitWebviewContentLoaded();

            await _webViewReportFrame.InvokeScriptAsync(
                "eval",
                new string[]
                {
                        "clearReport()"
                });
        }
    }
}
