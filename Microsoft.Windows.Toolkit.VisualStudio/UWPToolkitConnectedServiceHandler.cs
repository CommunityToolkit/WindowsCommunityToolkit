// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EnvDTE;

using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.Windows.Toolkit.VisualStudio.Helpers;

using NuGet.VisualStudio;

namespace Microsoft.Windows.Toolkit.VisualStudio
{
    [ConnectedServiceHandlerExport("Microsoft.Windows.Toolkit.VisualStudio.SocialServices", AppliesTo = "CSharp + WindowsAppContainer")]
    internal class UWPToolkitConnectedServiceHandler : ConnectedServiceHandler
    {
        [Import]
        internal IVsPackageInstaller PackageInstaller { get; set; }

        [Import]
        internal IVsPackageInstallerServices PackageInstallerServices { get; set; }

        private static Tuple<string, Version>[] requiredPackages = new[]
        {
            Tuple.Create("Newtonsoft.Json", new Version("8.0.3")), 
            Tuple.Create("WindowsAppStudio.DataProviders", new Version("1.3.0")), 
        };

        public async override Task<AddServiceInstanceResult> AddServiceInstanceAsync(ConnectedServiceHandlerContext context, CancellationToken ct)
        {
            Project project = ProjectHelper.GetProjectFromHierarchy(context.ProjectHierarchy);
            var toolkitServicesInstance = context.ServiceInstance as UWPToolkitConnectedServiceInstance;

            string templateResourceUri = "pack://application:,,/" + GetType().Assembly.ToString() + ";component/Templates/ProviderHelperTemplate.cs";
            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, "Adding and generating helper classes");
            string generatedHelperPath = Path.Combine(
                context.HandlerHelper.GetServiceArtifactsRootFolder(), 
                Constants.SERVICE_FOLDER_NAME, 
                $"{context.ServiceInstance.Name}GeneratedProviderHelper.cs");

            AddFileOptions addFileOptions = new AddFileOptions();

            string tokensPropertiesAndValues = string.Empty;

            foreach (var oAuthKeyValue in toolkitServicesInstance.Metadata)
            {
                if (oAuthKeyValue.Value.ToString() != Constants.OAUTH_KEY_VALUE_DEFAULT_NOT_REQUIRED_VALUE)
                {
                    tokensPropertiesAndValues = string.Concat(tokensPropertiesAndValues, $@"tokens.{oAuthKeyValue.Key} = ""{oAuthKeyValue.Value.ToString()}""; ");
                }
            }

            addFileOptions.AdditionalReplacementValues.Add("TOKEN_PROPERTIES_AND_VALUES", tokensPropertiesAndValues);

            var queryParamPropertyName = DataProviderDiscovery.Instance.FindQueryParamStringNameByProviderPublisherKeyName(context.ServiceInstance.Name);

            addFileOptions.AdditionalReplacementValues.Add("QUERY_PARAM_PROPERTY_NAME", queryParamPropertyName);

            await context.HandlerHelper.AddFileAsync(templateResourceUri, generatedHelperPath, addFileOptions);

            templateResourceUri = "pack://application:,,/" + GetType().Assembly.ToString() + ";component/Templates/DataProviderConnectorTemplate.cs";

            generatedHelperPath = Path.Combine(
                context.HandlerHelper.GetServiceArtifactsRootFolder(), 
                Constants.SERVICE_FOLDER_NAME, 
                $"{context.ServiceInstance.Name}GeneratedProviderConnector.cs");

            await context.HandlerHelper.AddFileAsync(templateResourceUri, generatedHelperPath, addFileOptions);

            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, "Updating Config");
            using (EditableXmlConfigHelper configHelper = context.CreateEditableXmlConfigHelper())
            {
                configHelper.SetAppSetting(
                    $@"{context.ServiceInstance.Name}:ConnectionString", 
                    $@"AppId={toolkitServicesInstance.Metadata["AppId"]};AppSecret={toolkitServicesInstance.Metadata["AppSecret"]};AccessToken={toolkitServicesInstance.Metadata["AccessToken"]};AccessTokenSecret={toolkitServicesInstance.Metadata["AccessTokenSecret"]};DataProviderType={toolkitServicesInstance.DataProviderModel.ProviderType}", 
                    context.ServiceInstance.Name);
                configHelper.Save();
            }

            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, "Adding NuGets");
            await AddNuGetPackagesAsync(context, project);

            AddServiceInstanceResult result = new AddServiceInstanceResult(
                                                            Constants.SERVICE_FOLDER_NAME, 
                                                            new Uri("https://github.com/"));
                                                            return result;
        }

        public override async Task<UpdateServiceInstanceResult> UpdateServiceInstanceAsync(ConnectedServiceHandlerContext context, CancellationToken ct)
        {
            return new UpdateServiceInstanceResult();
        }

        private async Task AddNuGetPackagesAsync(ConnectedServiceHandlerContext context, Project project)
        {
            IEnumerable<IVsPackageMetadata> installedPackages = PackageInstallerServices.GetInstalledPackages(project);
            Dictionary<string, string> packagesToInstall = new Dictionary<string, string>();

            foreach (Tuple<string, Version> requiredPackage in requiredPackages)
            {
                IVsPackageMetadata installedPackage = installedPackages.FirstOrDefault(p => p.Id == requiredPackage.Item1);
                if (installedPackage == null)
                {
                    // The package does not exist - notify and install the package.
                    await context.Logger.WriteMessageAsync(
                        LoggerMessageCategory.Information, 
                        "Installing NuGet package '{0}' version {1}.", 
                        requiredPackage.Item1, 
                        requiredPackage.Item2.ToString());
                }
                else
                {
                    Version installedVersion = GetNuGetPackageVersion(installedPackage);
                    if (installedVersion == null)
                    {
                        // Unable to parse the version - continue.
                        continue;
                    }
                    else if (installedVersion.Major < requiredPackage.Item2.Major)
                    {
                        // An older potentially non-compatible version of the package already exists - warn and upgrade the package.
                        await context.Logger.WriteMessageAsync(
                            LoggerMessageCategory.Warning, 
                            "Upgrading NuGet package '{0}' from version {1} to {2}.  A major version upgrade may introduce compatibility issues with existing code.", 
                            requiredPackage.Item1, 
                            installedPackage.VersionString, 
                            requiredPackage.Item2.ToString());
                    }
                    else if (installedVersion.Major > requiredPackage.Item2.Major)
                    {
                        // A newer potentially non-compatible version of the package already exists - warn and continue.
                        await context.Logger.WriteMessageAsync(
                            LoggerMessageCategory.Warning, 
                            "The code being added depends on NugGet package ‘{0}’ version {1}.  A newer version ({2}) is already installed.  This may cause compatibility issues.", 
                            requiredPackage.Item1, 
                            requiredPackage.Item2.ToString(), 
                            installedPackage.VersionString);

                        continue;
                    }
                    else if (installedVersion >= requiredPackage.Item2)
                    {
                        // A newer semantically compatible version of the package already exists - continue.
                        continue;
                    }
                    else
                    {
                        // An older semantically compatible version of the package exists - notify and upgrade the package.
                        await context.Logger.WriteMessageAsync(
                            LoggerMessageCategory.Information, 
                            "Upgrading NuGet package '{0}' from version {1} to {2}.", 
                            requiredPackage.Item1, 
                            installedPackage.VersionString, 
                            requiredPackage.Item2.ToString());
                    }
                }

                packagesToInstall.Add(requiredPackage.Item1, requiredPackage.Item2.ToString());
            }

            if (packagesToInstall.Any())
            {
                PackageInstaller.InstallPackagesFromVSExtensionRepository(
                    "Microsoft.Windows.Toolkit.VisualStudio.Chris Barker.432a25eb-7cbc-413e-8e31-0e8090bfa5fc", 
                    false, 
                    false, 
                    project, 
                    packagesToInstall);
            }
        }

        private static Version GetNuGetPackageVersion(IVsPackageMetadata package)
        {
            Version version;
            string versionString = package.VersionString;
            int dashIndex = versionString.IndexOf('-');
            if (dashIndex != -1)
            {
                // Trim off any pre-release versions.  Because the handler should never install pre-release
                // versions they can be ignored when comparing versions.
                versionString = versionString.Substring(0, dashIndex);
            }

            if (!Version.TryParse(versionString, out version))
            {
                Debug.Fail("Unable to parse the NuGet package version " + versionString);
            }

            return version;
        }
    }
}
