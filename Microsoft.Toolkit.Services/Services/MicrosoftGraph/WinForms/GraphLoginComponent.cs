// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;

namespace Microsoft.Toolkit.Services.WinForms
{
    /// <summary>
    /// This class provides a simple Login functionality for the Microsoft Graph.
    /// It is implemented as a Windows Forms Component for a drag and drop experience from the toolbox.
    /// </summary>
    /// <seealso cref="IComponent" />
    public partial class GraphLoginComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphLoginComponent" /> class.
        /// </summary>
        public GraphLoginComponent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphLoginComponent" /> class.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> associated with the component.</param>
        public GraphLoginComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the ClientId of the application registered in the Azure AD V2 portal: http://apps.dev.microsoft.com
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the array of request Scopes for accessing the Microsoft Graph.
        /// Must not be null when calling LoginAsync
        /// </summary>
        public string[] Scopes { get; set; }

        /// <summary>
        /// Gets the default image for the logged on user from the Microsoft Graph
        /// </summary>
        public System.Drawing.Image Photo { get; private set; }

        /// <summary>
        /// Gets the display name for the logged on user from the Microsoft Graph
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the job title for the logged on user from the Microsoft Graph.
        /// </summary>
        public string JobTitle { get; private set; }

        /// <summary>
        /// Gets the email address (UPN) for the logged on user from the Microsoft Graph.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the instance of the Microsoft.Graph.GraphServiceClient from the logon request
        /// </summary>
        public GraphServiceClient GraphServiceClient { get; private set; }

        /// <summary>
        /// LoginAsync provides entry point into the MicrosoftGraphService LoginAsync
        /// </summary>
        /// <returns>A MicrosoftGraphService reference</returns>
        public async Task<bool> LoginAsync()
        {
            // check inputs
            if (string.IsNullOrEmpty(ClientId))
            {
                // error
                return false;
            }

            if (Scopes == null || !Scopes.Any())
            {
                // error
                return false;
            }

            // Initialize the MicrosoftGraphService
            if (!MicrosoftGraphService.Instance.Initialize(ClientId, delegatedPermissionScopes: Scopes))
            {
                // error
                return false;
            }

            // Attempt to LoginAsync
            try
            {
                await MicrosoftGraphService.Instance.LoginAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // error
                return false;
            }

            // Initialize fields from the User's information from the Microsoft Graph
            var user = await MicrosoftGraphService.Instance.GraphProvider.Me.Request().GetAsync();

            DisplayName = user.DisplayName;
            JobTitle = user.JobTitle;
            Email = user.Mail;

            // get the profile picture
            using (var photoStream = await MicrosoftGraphService.Instance.GraphProvider.Me.Photo.Content.Request().GetAsync().ConfigureAwait(false))
            {
                if (photoStream != null)
                {
                    Photo = System.Drawing.Image.FromStream(photoStream);
                }
            }

            // return Microsoft.Graph.GraphServiceClient from the MicrosoftGraphService.Instance.GraphProvider
            GraphServiceClient = MicrosoftGraphService.Instance.GraphProvider;
            return true;
        }
    }
}
