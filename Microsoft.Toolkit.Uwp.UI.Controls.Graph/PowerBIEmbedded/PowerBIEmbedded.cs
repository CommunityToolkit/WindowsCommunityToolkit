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
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PowerBI embedded control is a simple wrapper to an IFRAME for a PowerBI embed.
    /// </summary>
    [TemplatePart(Name = "RootGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "WebViewReportFrame", Type = typeof(WebView))]
    public partial class PowerBIEmbedded : Control
    {
        private const string PowerBIResourceId = "https://analysis.windows.net/powerbi/api";
        private const string ApiUrl = "https://api.powerbi.com/";
        private WebView _webViewReportFrame;
        private TaskCompletionSource<bool> _webViewInitializedTask = new TaskCompletionSource<bool>();
        private string _tokenForUser;
        private DispatcherTimer _tokenExpirationRefreshTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerBIEmbedded"/> class.
        /// </summary>
        public PowerBIEmbedded()
        {
            DefaultStyleKey = typeof(PowerBIEmbedded);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            ApplyTemplate();

            MicrosoftGraphService.Instance.Initialize(ClientId);

            if (_webViewReportFrame != null)
            {
                _webViewReportFrame.DOMContentLoaded -= WebViewReportFrame_DOMContentLoaded;
            }

            _webViewReportFrame = GetTemplateChild("WebViewReportFrame") as WebView;

            if (_webViewReportFrame != null)
            {
                _webViewReportFrame.DOMContentLoaded += WebViewReportFrame_DOMContentLoaded;
            }

            if (_tokenExpirationRefreshTimer != null)
            {
                _tokenExpirationRefreshTimer.Tick -= TokenExpirationRefreshTimer_Tick;
                _tokenExpirationRefreshTimer.Stop();
            }

            _tokenExpirationRefreshTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMinutes(15)
            };
            _tokenExpirationRefreshTimer.Tick += TokenExpirationRefreshTimer_Tick;
            _tokenExpirationRefreshTimer.Start();

            DisplayInformation.OrientationChanged -= DisplayInformation_OrientationChanged;
            DisplayInformation.OrientationChanged += DisplayInformation_OrientationChanged;

            
        }

        private async Task<string> GetUserTokenAsync()
        {
            try
            {
                _tokenForUser = await MicrosoftGraphService.Instance.Authentication.AquireTokenAsync(PowerBIResourceId);
                return _tokenForUser;
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

        private async void LoadAllAsync()
        {
            if (!string.IsNullOrEmpty(ClientId))
            {
                if (!string.IsNullOrEmpty(GroupId))
                {
                    await LoadGroupAsync();
                    return;
                }

                if (!string.IsNullOrEmpty(EmbedUrl))
                {
                    await LoadReportAsync();
                    return;
                }
            }

            ClearReport();
        }

        private async Task LoadReportAsync()
        {
            if (Uri.TryCreate(EmbedUrl, UriKind.Absolute, out Uri embedUri))
            {
                string token = await GetUserTokenAsync();

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
                            InvokeScriptAsync($"loadGroups(" +
                                $"'{token}', " +
                                $"{JsonConvert.SerializeObject(new Report[] { report })}, " +
                                $"{JsonConvert.SerializeObject(report)}, " +
                                (IsWindowsPhone ? $"'{DisplayInformation.CurrentOrientation.ToString()}'," : "'', ") +
                                $"{ShowFilter.ToString().ToLower()}" +
                                ")");

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

        private async Task LoadGroupAsync()
        {
            string token = await GetUserTokenAsync();

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

                    InvokeScriptAsync($"loadGroups(" +
                        $"'{token}', " +
                        $"{JsonConvert.SerializeObject(reports)}, " +
                        $"{JsonConvert.SerializeObject(selectionReport)}, " +
                        (IsWindowsPhone ? $"'{DisplayInformation.CurrentOrientation.ToString()}'," : "'', ") +
                        $"{ShowFilter.ToString().ToLower()}" +
                        ")");
                }
            }
            else
            {
                ClearReport();
            }
        }

        private void ClearReport()
        {
            InvokeScriptAsync("clearReport()");
        }

        private Task<bool> WaitWebviewContentLoaded()
        {
            return _webViewInitializedTask.Task;
        }

        private async void InvokeScriptAsync(string script)
        {
            await WaitWebviewContentLoaded();

            await _webViewReportFrame.InvokeScriptAsync(
               "eval",
               new string[] { script });
        }
    }
}
