# Getting Started with the UWP Community Toolkit

The toolkit is available as NuGet packages that can be added to any existing or new project using Visual Studio.

- 1)	Download [Visual Studio 2015 Update 3 with Windows developer tools](https://developer.microsoft.com/en-us/windows/downloads) and the Windows 10 SDK.  **Important**: Ensure you choose the custom install option and select the *Universal Windows App Development Tools*.  

- 2)	Open an existing project, or create a new project using the Blank App template under Visual C# -> Windows -> Universal.  **Important**:  Build 10586 or higher is supported by current version of the Toolkit.   

- 3)	In Solution Explorer panel, right click on your project name and select **Manage NuGet Packages**. Search for **Microsoft.Toolkit.UWP**, and choose your desired [NuGet Packages](Nuget-Packages.md) from the list.

![NuGet Packages](resources/images/ManageNugetPackages.png "Manage NuGet Packages Image")

- 4)	Add a reference to the toolkit in your XAML pages or C#


- a.	In your XAML page, add a reference at the top of your page

```csharp

    xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"

```

- b.	In your C# page, add the namespaces to the toolkit

```csharp

    using Microsoft.Toolkit.Uwp;

```


- 5)	You can copy and paste code snippets for each feature from the [UWP Community Toolkit Sample App](http://aka.ms/uwptoolkitapp). 

## Other Resources 

Adding the [UI controls to the Visual Studio Toolbox](Toolbox-Support.md) explains deeper integration with Visual Studio. 

Download the [UWP Community Toolkit Sample App](http://aka.ms/uwptoolkitapp) from the Windows store to see the controls in an actual app.

We recommend developers new to UWP visit the [Getting Started with UWP Development](https://developer.microsoft.com/en-us/windows/getstarted) pages on the Developer portal. 

Visit the [UWP Community Toolkit Github Repository](http://aka.ms/uwptoolkit) to see the current source code, what is coming next, and to clone the repository.  Community contributions are welcome!

