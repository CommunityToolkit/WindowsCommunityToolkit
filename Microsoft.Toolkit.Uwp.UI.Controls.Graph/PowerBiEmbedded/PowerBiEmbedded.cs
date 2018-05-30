// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PowerBI embedded control is a simple wrapper to an IFRAME for a PowerBI embed.
    /// </summary>
    [TemplatePart(Name = "RootGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "WebViewReportFrame", Type = typeof(WebView))]
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
                    AuthenticationResult userAuthnResult = await azureAdContext.AcquireTokenAsync(
                        ResourceId,
                        appClientId,
                        new Uri(DefaultRedirectUri),
                        new PlatformParameters(PromptBehavior.Auto, false));

                    _tokenForUser = userAuthnResult.AccessToken;
                    _expiration = userAuthnResult.ExpiresOn;
                }

                if (_expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
                {
                    AuthenticationResult userAuthnResult = await azureAdContext.AcquireTokenSilentAsync(
                        ResourceId,
                        appClientId);

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

        private async void LoadAll()
        {
            if (!string.IsNullOrEmpty(ClientId))
            {
                if (!string.IsNullOrEmpty(GroupId))
                {
                    await LoadGroup();
                    return;
                }

                if (!string.IsNullOrEmpty(EmbedUrl))
                {
                    await LoadReport();
                    return;
                }
            }

            ClearReport();
        }

        private async Task LoadReport()
        {
            if (Uri.TryCreate(EmbedUrl, UriKind.Absolute, out Uri embedUri))
            {
                string token = await GetUserTokenAsync(ClientId);

                if (!string.IsNullOrEmpty(token)
                    && embedUri.Query.IndexOf("reportid", StringComparison.OrdinalIgnoreCase) != -1
                    && embedUri.Query.IndexOf("groupid", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    var tokenCredentials = new TokenCredentials(token, "Bearer");

                    using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
                    {
                        var decoder = new WwwFormUrlDecoder(embedUri.Query.ToLower());
                        string groupId = decoder.GetFirstValueByName("groupid");
                        string reportId = decoder.GetFirstValueByName("reportid");

                        Report report = await client.Reports.GetReportAsync(groupId, reportId);

                        if (report != null)
                        {
                            InvokeScript($"loadGroups(" +
                                $"'{token}', " +
                                $"{JsonConvert.SerializeObject(new Report[] { report })}, " +
                                $"{JsonConvert.SerializeObject(report)})");

                            return;
                        }
                    }
                }

                ClearReport();
            }
            else
            {
                throw new ArgumentException(nameof(EmbedUrl));
            }
        }

        private async Task LoadGroup()
        {
            string token = await GetUserTokenAsync(ClientId);

            if (!string.IsNullOrEmpty(token))
            {
                var tokenCredentials = new TokenCredentials(token, "Bearer");

                using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
                {
                    IList<Report> reports = (await client.Reports.GetReportsAsync(GroupId))?.Value;
                    Report selectionReport = null;

                    if (reports.Count > 0)
                    {
                        Report findReport = reports.Where(
                            i => i.EmbedUrl.Equals(
                                EmbedUrl,
                                StringComparison.OrdinalIgnoreCase))
                            .FirstOrDefault();

                        selectionReport = findReport ?? reports.First();
                    }

                    InvokeScript($"loadGroups(" +
                        $"'{token}', " +
                        $"{JsonConvert.SerializeObject(reports)}, " +
                        $"{JsonConvert.SerializeObject(selectionReport)})");
                }
            }
            else
            {
                ClearReport();
            }
        }

        private Task<bool> WaitWebviewContentLoaded()
        {
            return _webViewInitializedTask.Task;
        }

        private void ClearReport()
        {
            InvokeScript("clearReport()");
        }

        private async void InvokeScript(string script)
        {
            await WaitWebviewContentLoaded();

            await _webViewReportFrame.InvokeScriptAsync(
                "eval",
                new string[] { script });
        }
    }
}
