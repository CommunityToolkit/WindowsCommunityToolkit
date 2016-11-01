# How to add new samples

This document describes how to add a new sample page for a new control you want to add to the toolkit.

## Sample page
First you need to create a Xaml page in the folder /SamplePages/YourControl.
This page needs to add this specific code to be able to connect with the property UI:
```
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }
        }
```

## Binding text
In order to provide a property UI and associated code, you have to define a text file associated with your page.
Here is an example:
```
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="48"></ColumnDefinition>
        <ColumnDefinition></ColumnDefinition>
    </Grid.ColumnDefinitions>
    <TextBlock Grid.Column="1" 
		Text="@[Text:String:Hey!]" Foreground="Black" 
		FontSize="@[FontSize:Slider:12:10-30]" 
		VerticalAlignment="@[VerticalAlignment:Enum:VerticalAlignment.Center]">
	</TextBlock>
</Grid>
```

You can define "interactive" values in this file. The values can be:
* String: You want the user to provide a text. The string is built like this @[Name:**String**:Default value]
* Slider: You want the user to provide a double value. The string is built like this @[Name:**Slider**:Default value:min-max]
* DoubleSlider: Same as slider but with double values (0.01 precision)
* Enum: You want the user to provide a enum value. The string is built like this @[Name:**Enum**:Default value]
* Brush: You want the user to select a color from a list. The string is built like this @[Name:**Brush**:Black]
* Bool: You want the user to enable or disable a property. The string is built like this @[Name:**Bool**:True]

You can then bind the value specified in your Xaml page (using xxx.Value path):

```
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="48"></ColumnDefinition>
        <ColumnDefinition></ColumnDefinition>
    </Grid.ColumnDefinitions>
    <TextBlock Grid.Column="1" Text="{Binding Text.Value, Mode=OneWay}" Foreground="Black" 
                FontSize="{Binding FontSize.Value, Mode=OneWay}" 
                VerticalAlignment="{Binding VerticalAlignment.Value, Mode=OneWay}"></TextBlock>
</Grid>
```

## Samples.json
After creating your page and the binding text, you just need to reference it in the /SamplePages/samples.json file.
Select the category where you want your page to be listed and add the following information:

```
[
  {
    "Name": "Panel controls",
    "Icon": "Icons/Layouts.png",
    "Samples": [
      {
        "Name": "AdaptiveGridView",
        "Type": "AdaptiveGridViewPage",
        "About": "The AdaptiveGridView control allows to present information within a Grid View perfectly adjusting the total display available space. It reacts to changes in the layout as well as the content so it can adapt to different form factors automatically. The number and the width of items are calculated based on the screen resolution in order to fully leverage the available screen space. The property ItemsHeight define the items fixed height and the property DesiredWidth sets the minimum width for the elements to add a new column.",
        "CodeUrl": "https://github.com/deltakosh/UWPToolkit",
        "XamlCodeFile": "AdaptiveGridViewCode.bind" 
      }
    ]
  }
]
```