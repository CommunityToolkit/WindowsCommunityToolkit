# SpaceView XAML Control 

The `SpaceView` control provides a new control, inherited from the `ItemsControl`. All items are arranged in circle around a center element

![SpaceView Overview](../resources/images/SpaceView.gif "SpaceView")  

## Syntax

```xaml

<controls:SpaceView OrbitsEnabled="True" 
                    AnchorsEnabled="False" 
                    IsItemClickEnabled="True" 
                    MinItemSize="20" 
                    MaxItemSize="60"
                    AnchorColor="Gray"
                    OrbitColor="Gray">
  <controls:SpaceView.ItemTemplate>
    <DataTemplate x:DataType="controls:SpaceViewItem">
      <controls:DropShadowPanel Color="Black" BlurRadius="20" VerticalContentAlignment="Stretch" HorizontalContentAlignment="Stretch">
        <Ellipse >
          <Ellipse.Fill>
            <ImageBrush ImageSource="{x:Bind Image}"></ImageBrush>
          </Ellipse.Fill>
        </Ellipse>
      </controls:DropShadowPanel>
    </DataTemplate>
  </controls:SpaceView.ItemTemplate>
  <controls:SpaceView.ItemsSource>
    <controls:SpaceViewItemCollection>
      <controls:SpaceViewItem Image="ms-appx:///Assets/People/shen.png" Distance="0.1" Label="Shen" Diameter="0.2"></controls:SpaceViewItem>
      <controls:SpaceViewItem Image="ms-appx:///Assets/People/david.png" Distance="0.2" Label="David" Diameter="0.5"></controls:SpaceViewItem>
      <controls:SpaceViewItem Image="ms-appx:///Assets/People/petri.png" Distance="0.4" Label="Petri" Diameter="0.6"></controls:SpaceViewItem>
      <controls:SpaceViewItem Image="ms-appx:///Assets/People/vlad.png" Distance="0.8" Label="Vlad" Diameter="0.8"></controls:SpaceViewItem>
    </controls:SpaceViewItemCollection>
  </controls:SpaceView.ItemsSource>
  <controls:SpaceView.CenterContent>
    <Grid>
      <controls:DropShadowPanel>
        <Ellipse Fill="White" Height="105" Width="105" Stroke="Black" StrokeThickness="2"></Ellipse>
      </controls:DropShadowPanel>
      <Ellipse Height="100" Width="100" VerticalAlignment="Center" HorizontalAlignment="Center">
        <Ellipse.Fill>
          <ImageBrush ImageSource="ms-appx:///Assets/People/nikola.png"></ImageBrush>
        </Ellipse.Fill>
      </Ellipse>
    </Grid>
  </controls:SpaceView.CenterContent>
</controls:SpaceView>

```

## SpaceView Properties

### IsItemClickEnabled

### OrbitsEnabled

### OrbitColor

### OrbitThickness

### OrbitDashArray

### AnchorsEnabled

### AnchorColor

### AnchorThickness

## SpaceView Events

### ItemClicked

## SpaceViewItem


## Default Template 

[SpaceView XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/SpaceView/SpaceView.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family]("http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [Carousel source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/SpaceView)

