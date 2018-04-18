---
title: Gaze
author: harishsk
description: The Gaze interaction library contains a set of helper classes to easily enable your UWP for interaction with eye trackers. 
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, gaze
---

# Gaze
Microsoft announced native support of eye trackers in [Windows 10 Fall Creators Update](https://blogs.msdn.microsoft.com/accessibility/2017/08/01/from-hack-to-product-microsoft-empowers-people-with-eye-control-for-windows-10/). In RS4, Microsoft added developer support by announcing an [eye gaze API]() to build UWP applications that can interact with eye gaze and eye trackers. . 

The Gaze library is built on top of that API and provides developers helper classes to easily enable UWP applications to respond to eye gaze. The library abstracts away the complexity of dealing with raw gaze samples coming from the low level Windows API for eye-trackers. 

### Prerequisites
In order to use the Windows 10 gaze API or this gaze interaction library, you need to be have the following:
* Windows 10 RS4 release
* A [supported eye tracker](https://blogs.msdn.microsoft.com/accessibility/2017/08/01/from-hack-to-product-microsoft-empowers-people-with-eye-control-for-windows-10/), like the [Tobii EyeX 4C](https://tobiigaming.com/products/)

### Supported features
The Gaze interaction currently supports the following features:
* Dwell based activation of buttons, toggle buttons, check boxes, etc. 
* Enabling gaze interaction for the whole page or a portion of it
* Customizing the dwell times associated with specific controls
* Controlling repetition of the invocation

### Gaze Concepts
A few eye gaze related concepts are useful to explain:

* **Saccaddes.** A saccade is movement of the eyes from one fixation point to another. Our eyes alternate between fixations and saccades. 

* **Fixation.**  Fixation is the maintaining of gaze on a single location for a relatively short amount of time (roughly around 200ms). This happens after a saccadic motion when the eye rests upon an object and it comes into shart focus. 

* **Dwell.** This is concious fixation for a duration greater than the fixation time. This time duration is application dependent. 

* **Enter/Exit** These are states and properties specific to this API to help manage gaze related interaction and refer to the time elapsed since the first recorded gaze sample and the last recorded gaze sample on a particular control (Button, ToggleButton etc.)

The GazeApi library enables dwell based gaze interaction on the page by reading the data from the eye tracker over the page invoking specific controls when the user's gaze dwells on a control for a specific time. The application can configure this time based on its usage scenario. 

## Quick Start

#### To enable gaze interaction on the whole page 
**XAML**

To enable the whole page for gaze interaction, add the following lines to your Page element
```

    xmlns:gaze="using:Microsoft.Toolkit.UWP.Input.Gaze"
    gaze:GazeApi.IsGazeEnabled="True"
    
```

For e.g.


```xaml
<Page
    x:Class="UwpApp.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:gaze="using:Microsoft.Toolkit.UWP.Input.Gaze"
    gaze:GazeApi.IsGazeEnabled="True"
    mc:Ignorable="d">

```

#### To enable gaze interaction on a portion of the page
To enable only a subset of the page, e.g. just one Grid on the page, 
```xaml
<Grid gaze:GazeApi.IsGazeEnabled="True">
	<Button Content="Click Me" />
<Grid />
```
In the above example, the button will be clicked when the user looks at the button in the grid for a period equal to the default dwell time.




### To change the dwell time for a control

The code below sets the Dwell period for the button to be 1 second. This means the button will be clicked after the user stares it for a second.

```xaml
<Button Content="Click Me" gaze:GazeApi.Dwell="00:00:01.0">
```

## Properties



### GazePointerState
The low level gaze API deliver a stream of timestamped `[x,y]` coordinates for the user's gaze location on the screen. The gaze interaction library aggregates these samples over each control and converts the stream into gaze events. Corresponding to these events, are the following states:

| Property | Type | Description |
| -- | -- | -- |
|Enter | enum | User's gaze has entered a control |
|Fixation| enum | User eye's are focused on the control. |
|Dwell | enum | User is conciously dwelling on the control with an intent to invoke, e.g. click a button|
|DwellRepeat| enum | This is a small delay after Dwell. If the button is configured for repeated invocation, it will do so after the time period associated with this state has elapsed.|
|Exit|enum|User's gaze has is no longer on the control|



### GazeApi properties
Whether the page is enabled for the gaze based interaction, the visibility and size of the gaze cursor, and the timings associated with the states above can be configured using the properties below:

| Property | Type | Description |
| -- | -- | -- |
| IsGazeEnabled | bool | Gets or sets whether gaze interaction is enabled on the element and its children.  |
| IsGazeCursorVisible | bool | The gaze cursor shows where the user is looking at on the screen. This boolean property shows the gaze cursor when set to `true` and hides it when set to `false` |
|GazeCursorRadius|int|Gets or sets the size of the gaze cursor radius|
| Enter | TimeSpan | The time elapsed after the first gaze sample was recorded on a control to be in `Enter` state. When this time has elapsed, `GazeApi` fires a `GazePointerEvent` with the `PointerState` set to `Enter`. The default is 50ms. |
| Fixation | TimeSpan | The time elapsed after the first gaze sample was recorded on a control to be in the `Fixation` state. When this time has elapsed, `GazeApi` fires a `GazePointerEvent` with the `PointerState` set to `Fixation`. The `Fixation` property should be used to control the earliest visual feedback the application needs to provide to the user about the gaze location. The default is 400ms. **CHECK**|
| Dwell | TimeSpan | The time elapsed after the first gaze sample was recorded on a control to be in the `Dwell` state. When this time has elapsed, `GazeApi` fires a `GazePointerEvent` with the `PointerState` set to `Dwell`. `Enter` and `Fixation` states are achieved very rapidly for the user to have much control over. In contrast `Dwell` is conscious event. This is the point at which the control is invoked, e.g. a button click. The application should modify this property to control when a gaze enabled UI element gets invoked after a user starts looking at it.
| DwellRepeat | TimeSpan | The time elapsed after the first gaze sample was recorded on a control to be in the `DwellRepeat` state. After this time has elapsed, the control enters  a 'repeat' mode, and the control will be repeatedly invoked as long as the user's gaze stays within the control area. No event is fired when this time has elapsed. After this, for every elapse of time equal to (Dwell - Fixation) another Dwell event is fired. **CHECK**|
| Exit | TimeSpan | The time elapsed after the last gaze sample was recorded on a control. After this time, the control returns to its normal state, e.g. a button returns to its normal (unpressed) state. When this time has elapsed, `GazeApi` fires a `GazePointerEvent` with the `PointerState` set to `Exit`. The default is 50ms. |
| MaxRepeatCount | int | The maximum times the control will invoked repeatedly without the user's gaze having to leave and re-enter the control |


>[IMPORTANT] For correct operation, the Fixation time must be greater than the Enter time, the Dwell time must be greater than the Fixation time and  the DwellRepeat time must be greater than the Dwell time.



### GazeElement properties
| Property | Type | Description |
| -- | -- | -- |
|HasAttention|bool|A property that gets whether user attention is currently on the control in question|
|InvokeProgress|double|A value between 0 and 1 that indicates the percent time elapsed towards the control being invoked. This property can be used to provide visual feedback to the user|


## GazeElement Events

| Events | Description |
| -- | -- |
| GazePointerEvent | This event is raised in response to each of the states associated with GazePointerState (except for the `DwellRepeat` state). An application can add a handler for this event to customize gaze related processing with respect to the various gaze pointer states mentioned above.|


### GazePointerEventArgs properties
| Property | Type | Description |
| -- | -- | -- |
|ElapsedTimeSpan|TimeSpan|The time the user has spent looking at the control to reach the specific pointer state above|
|PointerState|GazePointerState|The `GazePointerState` associated with this event|


<!-- Use <remarks> tag in C# to give more info about a propertie. For more info - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/remarks -->

## Methods

<!-- Explain all methods in a table format -->

| Methods | Return Type | Description |
| -- | -- | -- |
| A(int) | bool | Description |
| B(float, string) | int | Description |

<!-- Use <remarks> tag in C# to give more info about a method. For more info - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/remarks -->


<!-- Use <remarks> tag in C# to give more info about a event. For more info - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/remarks -->

## Examples

<!-- All control/helper must at least have an example to show the use of Properties and Methods in your control/helper with the output -->
<!-- Use <example> and <code> tags in C# to create a Propertie/method specific examples. For more info - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/example -->
<!-- Optional: Codes to achieve real-world use case with the output. For eg: Check https://docs.microsoft.com/en-us/windows/uwpcommunitytoolkit/animations/animationset#examples  -->

## Sample Code

<!-- Link to the sample page in the UWP Community Toolkit Sample App -->
[GazeInteractionPage](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/GazeInteraction/). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal,10.0.17133.0 or higher   |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.Input.Gaze |
| NuGet package | [NuGet package](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Input.Gaze/) |


## API Source Code

- [control/helper name source code](source-code-link)

## Related Topics

- [Windows 10 eye gaze API Preview](link)

