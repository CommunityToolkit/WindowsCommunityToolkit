# Sample Application

For the latest info, [visit the wiki article here](https://github.com/CommunityToolkit/WindowsCommunityToolkit/wiki/Sample-Development).

## How to add new samples

This document describes how to add a new sample page for a new control you want to add to the toolkit.

*`DropShadowPanel`*, *`ImageEx`*, and *`ImageCache`* are good examples of most of the features mentioned below.

### 1. Add Sample page and `.bind` template

First you need to create a XAML page in the folder `/SamplePages/YourControl`.  This will be the logical page used to by the app to navigate to the sample and contains code.

If providing 'live' XAML, a `.bind` file is loaded and dynamically fed to the `XamlReader.Load` method to convert into actual controls.  This changes a few things about how samples need to be written (detailed below), but allows developers to actually change the sample and see the results live.

This not only gives us a killer sample app, but it also means that all our samples are also self-validating.  There can't be a typo in the sample text given in the sample app anymore, as otherwise the sample won't work and should be caught during testing of said sample.

### 2. Binding text

The `.bind` files are templates which use `@[Property Name:Type:DefaultValue:Options]` syntax to allow for customized options to be presented to the user in the sample app.  The user can play with the values in the property page and see results change instantly.  This is accomplished by using {Binding} syntax when on the property page, but switches to the raw value when the developer goes to the XAML page.

This makes it easy for a developer to test out values for a control and then copy the XAML needed for that exact result into their app.

In order to provide a property UI and associated code, you have to define a `.bind` XAML file associated with your page.
Here is an example:

```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="48" />
        <ColumnDefinition />
    </Grid.ColumnDefinitions>
    <TextBlock Grid.Column="1"
        Foreground="Black"
        Text="@[Text:String:Hey!]"
        FontSize="@[FontSize:Slider:12:10-30]"
        VerticalAlignment="@[Vertical Alignment:Enum:VerticalAlignment.Center]">
    </TextBlock>
</Grid>
```

You can define “interactive” values in this file. The value types can be:

* `String`: You want the user to provide a text. An equivalent syntax is `@[Name:String:Default Value]`
* `Slider`: You want the user to provide a double value. An equivalent syntax is `@[Name:Slider:DefaultValue:Min-Max]`
* `DoubleSlider`: Same as slider but with double values (0.01 precision)
* `TimeSpan`: You want the user to provide a duration. An equivalent syntax is (all values in milliseconds) `@[Name:TimeSpan:DefaultValue:Min-Max]`
* `Enum`: You want the user to provide an enum value. An equivalent syntax is `@[Name:Enum:EnumType.DefaultValue]`
* `Brush`: You want the user to select a color from a list. An equivalent syntax is `@[Name:Brush:Black]`
* `Bool`: You want the user to enable or disable a property. An equivalent syntax is `@[Name:Bool:True]`
* `Thickness`: You want the user to provide a Thickness. An equivalent syntax is `@[Name:Thickness:0,20,10,0]`

The `Property Name` can also contain spaces, but these will be removed from the property name used for accessing the value in the property bag for any binding/access, see below.

The name and options will be translated **automatically** to the following syntax when your `.bind` template is being used on the property page:

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="48" />
        <ColumnDefinition />
    </Grid.ColumnDefinitions>
    <TextBlock Grid.Column="1" Foreground="Black"
        Text="{Binding Text.Value, Mode=OneWay}"
        FontSize="{Binding FontSize.Value, Mode=OneWay}"
        VerticalAlignment="{Binding VerticalAlignment.Value, Mode=OneWay}">
    </TextBlock>
</Grid>
```

When the developer switches to the XAML tab, they'll automatically see the selected values instead:

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="48" />
        <ColumnDefinition />
    </Grid.ColumnDefinitions>
    <TextBlock Grid.Column="1"
        Foreground="Black"
        Text="User Entered Text"
        FontSize="12"
        VerticalAlignment="Left">
    </TextBlock>
</Grid>
```

You can also reuse a `@[Property Name]` reference by itself again later to use the same binding/value again in the same template.  This will automatically get mapped to the right place without the need to specify all the types/options again.  Just set those options on your first usage.

If you happen to need a two-way binding for the generated XAML, then add an extra '**@**' after the property definition in the template:

```xml
<Slider Value="@[Value:Slider:0:0-180]@" />
```

### 3. Have a '*Shallow Copy*' of your example in the sample page

Even though the sample page content is ignored and the dynamic template injected, for the `XamlReader` to access some classes, a reference to the item is sometimes needed in the hosting app for it to be accessible.  (I assume it's an optimization thing.)

Therefore, for any new control/extension, you should still have a simplified snippet of it contained in the sample page compiled/loaded by the app.  You should remove names, events, and properties (unless extensions) from these, so the namespace isn't accidentally polluted.  If you re-use the same control, you don't have to include it twice.

### 4. For Events/Resource Templates: Have your sample page implement the **`IXamlRendererListener`** interface

This gets called whenever the template gets parsed (due to loading or user modification).  Here you can use the [`LogicalTree`](https://github.com/CommunityToolkit/WindowsCommunityToolkit/blob/main/Microsoft.Toolkit.Uwp.UI/Extensions/FrameworkElement/FrameworkElementExtensions.LogicalTree.cs) extensions to grab named controls in the template and register their events.  **Check for null first** as the developer may have removed the name from the element.

```cs
var markdownText = control.FindChild("MarkdownText") as MarkdownTextBlock;
if (markdownText != null)
{
    markdownText.LinkClicked += MarkdownText_LinkClicked;
}
```

You'll have to register all events and grab **`control.Resources`** for templates from this method as the regular sample page XAML isn't used, and you can't hook in an event from the dynamic XAML, it must be done via code by finding the element here.

### 5. For Interactive Buttons: Use **`SampleController.Current.RegisterNewCommand`**

Buttons can be added through this command and are accessible in the main panel, so they can be clicked when changing properties or editing XAML.  It's important instead of using buttons in your sample (as events can't be directly used, see above) to register these commands.

```cs
protected override async void OnNavigatedTo(NavigationEventArgs e)
{
    base.OnNavigatedTo(e);

    SampleController.Current.RegisterNewCommand("Image with placeholder", (sender, args) =>
    {
        AddImage(false, true);
    });

    // •••
}
```

If your command adds content dynamically, try and use a style template in the `.bind` XAML that the user can modify.  Then grab `resources = control.Resources;` in the *`OnXamlRendered`* event and set the element style from it:

```cs
if (resources?.ContainsKey("ThingStyle") == true)
{
    newThing.Style = resources["ThingStyle"] as Style;
}
```

### 6. *Optional:* If you need *extra stuff* around the sample

Now, the sample page content in the app is ignored, but you can override that behavior by adding a `<Grid x:Name="XamlRoot"/>` element to the page.  If this element is found, it will serve as the host to the dynamic `.bind` content instead.  In this manner you can have a status/warning message outside the control of the developer in the XAML sample tab.

## Update `Samples.json`

After creating your page and the binding text, you just need to reference it in the `/SamplePages/samples.json` file.
Select the category where you want your page to be listed and add the following information:

### Basic Structure

```json
[
  {
    "Name": "Panel controls",
    "Icon": "Icons/Layouts.png",
    "Samples": [
      {
        "Name": "AdaptiveGridView",
        "Type": "AdaptiveGridViewPage",
        "About": "The AdaptiveGridView control allows to present information within a Grid View perfectly adjusting the total display available space. It reacts to changes in the layout as well as the content so it can adapt to different form factors automatically. The number and the width of items are calculated based on the screen resolution in order to fully leverage the available screen space. The property ItemsHeight define the items fixed height and the property DesiredWidth sets the minimum width for the elements to add a new column.",
        "CodeUrl": "https://github.com/CommunityToolkit/WindowsCommunityToolkit/tree/main/Microsoft.Toolkit.Uwp.UI.Controls.Core/TextToolbar",
        "XamlCodeFile": "AdaptiveGridViewCode.bind",
        "DocumentationUrl": "https://raw.githubusercontent.com/CommunityToolkit/WindowsCommunityToolkit/main/docs/controls/AdaptiveGridView.md"
      }
    ]
  }
]
```

### Thumbnail Images

For creating new icons, please follow the [Thumbnail Style Guide and templates](https://github.com/CommunityToolkit/WindowsCommunityToolkit-design-assets)

### Restricting Samples to Specific API Sets

Some features used by samples aren't available on all the OS versions that the Sample App runs on.  In order to make sure a sample is valid for the host OS, add the `ApiCheck` key/value in your JSON definition.

The value is a string which is the fully-qualified type-name to check for the presence of.  You can also accompany this with the `BadgeUpdateVersionRequired` which uses the string provided to show a short message on the sample information so up level implementer know the minimum version required.

```json
    {
        // •••
        "About": "MySample needs 10.0.18362 or higher to work.",
        "ApiCheck": "Windows.UI.Xaml.Controls.NavigationView",
        "BadgeUpdateVersionRequired": "Fall Creators Update required",
        // •••
    }
```

If the specified type is not found on the system running the sample app, the sample will not appear in the sample list.

#### Adding documentation

Every API must be accompanied by Markdown doc in the [documentation](https://github.com/CommunityToolkit/WindowsCommunityToolkit/blob/main/Contributing.md#docs) [repository](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs).

Use the `DocumentationUrl` property to add a link to the raw documentation in *`samples.json`*. Please follow the following pattern:

`https://raw.githubusercontent.com/MicrosoftDocs/WindowsCommunityToolkitDocs/{branch}/docs/{folder/file.md}`

##### NOTES

* When building and running the app in release mode, the branch will automatically be changed to **main** before loading.
* The documentation is also packaged with the sample app. If there is no network connection, or the documentation is not yet on GitHub, the sample app will use the packaged version.
* To test your documentation in the sample app while running in debug mode, the Docs repository will need to be cloned in the same folder as this repository and named **`WindowsCommunityToolkitDocs`**. For

Example, this folder structure works best:

```txt
Repos
├── WindowsCommunityToolkit
├── WindowsCommunityToolkitDocs
└── Others
```

#### Using `CodeUrl`

The value of `CodeUrl` is modified when the app is built in release mode. The branch is automatically changed to **main**. This allows you to test the link in debug while pointing to dev.
