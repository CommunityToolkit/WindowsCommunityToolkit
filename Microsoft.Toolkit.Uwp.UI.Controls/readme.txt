Thanks for installing the Windows Community Toolkit Controls NuGet package!

This is a meta-package made up of various Windows Community Toolkit packages to make it easy and convenient for you to use the controls. You also have the option to only use packages you need which can help optimize the size of your package once you are ready to ship. Visit https://aka.ms/wct/optimize to learn more.

You can find out more about the Windows Community Toolkit at https://aka.ms/windowstoolkit
Or even try our controls in our sample app at https://aka.ms/windowstoolkitapp
Docs are availabe here: https://aka.ms/windowstoolkitdocs

----

This package also depends on the WinUI library, so you'll need to set XamlControlsResources as your Application resources in App.xaml:

    <Application>
        <Application.Resources>
            <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
        </Application.Resources>
    </Application>

If you have other resources, then we recommend you add those to the XamlControlsResources' MergedDictionaries.
This works with the platform's resource system to allow overrides of the XamlControlsResources resources.

<Application
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="using:Microsoft.UI.Xaml.Controls">
    <Application.Resources>
        <controls:XamlControlsResources>
            <controls:XamlControlsResources.MergedDictionaries>
                <!-- Other app resources here -->
            </controls:XamlControlsResources.MergedDictionaries>
        </controls:XamlControlsResources>
    </Application.Resources>
</Application>

See http://aka.ms/winui for more information about the WinUI library.