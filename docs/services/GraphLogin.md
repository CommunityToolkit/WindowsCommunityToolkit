---
title: GraphLogin Component
author: AdamBraden
description: Windows Forms component to authenticate with Azure AD v2 and the Microsoft Graph
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Windows Forms, GraphLogin 
---

# GraphLogin Component
<!-- Describe your control -->
The [GraphLogin component](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.win32.ui.controls.winforms.graphlogin) is a Windows Forms component that provides an easy to use experience for authenticating with Azure AD and the Microsoft Graph.  This component is being provided for Windows Forms developers that need a Microsoft Graph authentication solution on Windows 10 builds 1803 and earlier and on Windows 7.

This component wraps the Toolkit's **MicrosoftGraphService** for an easy to use Login experience.  The control then provides read-only properties about the logged on user and an instance of the **GraphServiceClient** which can be used for additional calls with the Microsoft Graph SDK.

> [!IMPORTANT]
Before using this component, the application must be registered in the Azure AD v2 endpoint.  For more information on registering your app see https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-app-registration.


## Syntax
To use this sample code in a Windows Forms application, install the Microsoft.Toolkit.Win32.UI.Controls nuget package, and add a button, 2 labels, and a picturebox on the form.  Then drag a GraphLogin component from the toolbox on to the form, and enter the following code:

```csharp

	using Microsoft.Toolkit.Services.Services.MicrosoftGraph;


	// Instance of Microsoft Graph 
	private GraphServiceClient graphClient = null;

        public Form1()
        {
            InitializeComponent();

           // values to connect to Microsoft Graph
            graphLoginComponent1.ClientId = "{your app's clientid}";
            graphLoginComponent1.Scopes = new string[] { MicrosoftGraphScope.UserRead };
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!await graphLoginComponent1.LoginAsync())
            {
                return;
            }

            //update the user's display fields
            label1.Text = graphLoginComponent1.DisplayName;
            label2.Text = graphLoginComponent1.JobTitle;
            pictureBox1.Image = graphLoginComponent1.Photo;

            // Do more things with the graph
            graphClient = graphLoginComponent1.GraphServiceClient;
        }
```

<!-- ## Sample Output -->

<!-- Image/Text can show the output of the control/helper -->

## Add the GraphLogin component to the Visual Studio Toolbox

<!-- Remove these steps before going public. This is for early adopters only -->
1. Open the Visual Studio **Toolbox**, then right-click anywhere in the toolbox, and select the **Choose Items** option.

2. In the **.NET Framework Components** tab of the **Choose Toolbox Items** dialog box.

3. Use the **Browse** button to locate the **Microsoft.Toolkit.Services.dll** in your NuGet package folder (choose the *net461* folder).

   For help finding that folder, see [Managing the global packages, cache, and temp folders](https://docs.microsoft.com/nuget/consume-packages/managing-the-global-packages-and-cache-folders).

4. Add that file to the list of Toolbox controls, and then close the **Choose Toolbox Items** dialog box.

   The **GraphLoginComponent** appears in the **General** section of the **Toolbox**.

## Properties

| Property | Type | Description |
| -- | -- | -- |
| ClientId | string| The ClientId of the application as registered with Azure AD v2 |
| Scopes | string[]| An array of scopes requested for the Microsoft Graph.  Use values from the MicrosoftGraphScope enum |
| DisplayName | string | Display name for the logged on user from the Microsoft Graph |
| JobTitle | string | Job title for the logged on user from the Microsoft Graph |
| Email | string | Email address (UPN) for the logged on user from the Microsoft Graph |
| Photo | System.Drawing.Image | Profile picture for the logged on user from the Microsoft Graph |
| GraphServiceClient | Microsoft.Graph.GraphServiceClient | The GraphServiceClient instance for the logged on user from the Microsoft Graph |


<!-- Use <remarks> tag in C# to give more info about a propertie. For more info - https://docs.microsoft.com/dotnet/csharp/programming-guide/xmldoc/remarks -->

## Methods

<!-- Explain all methods in a table format -->

| Methods | Return Type | Description |
| -- | -- | -- |
| LoginAsync | bool | Returns true of success, false otherwise |


## Requirements

| Device family | .NetFramework 4.6.1 or higher   |
| -- | -- |
| Namespace | Microsoft.Toolkit.Services.WinForms |
| NuGet package | [Microsoft.Toolkit.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Services) |

## API Source Code

- [WinForms.GraphLogin](/Microsoft.Toolkit.Services/Services/MicrosoftGraph/WinForms)
