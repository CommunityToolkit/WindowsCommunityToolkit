---
title: GazeInteraction
author: harishsk
description: The Gaze interaction library contains a set of helper classes to easily enable your UWP for interaction with eye trackers. 
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, gaze
---

# GazeInteraction

Microsoft announced native support of eye trackers in [Windows 10 Fall Creators Update](https://blogs.msdn.microsoft.com/accessibility/2017/08/01/from-hack-to-product-microsoft-empowers-people-with-eye-control-for-windows-10/). In RS4, Microsoft added developer support by announcing an [eye gaze API](https://docs.microsoft.com/en-us/uwp/api/windows.devices.input.preview) to build UWP applications that can interact with eye gaze and eye trackers. .

The GazeInteraction library is built on top of that API and provides developers helper classes to easily enable UWP applications to respond to eye gaze. The library abstracts away the complexity of dealing with raw gaze samples coming from the low level Windows API for eye-trackers.

## Prerequisites

In order to use the Windows 10 gaze API or this gaze interaction library, you need to be have the following:

* Windows 10 RS4 release
* A [supported eye tracker](https://blogs.msdn.microsoft.com/accessibility/2017/08/01/from-hack-to-product-microsoft-empowers-people-with-eye-control-for-windows-10/), like the [Tobii EyeX 4C](https://tobiigaming.com/products/)

## Supported features

The GazeInteraction library currently supports the following features:

* Dwell based activation of buttons, toggle buttons, check boxes, etc.
* Enabling gaze interaction for the whole page or a portion of it
* Customizing the dwell times associated with specific controls
* Controlling repetition of the invocation

## Gaze Concepts

A few eye gaze related concepts are useful to explain in order to better understand the rest of the document:

* **Saccade.** A saccade is movement of the eyes from one fixation point to another. Our eyes alternate between fixations and saccades.
* **Fixation.**  Fixation is the maintaining of gaze on a single location for a relatively short amount of time (roughly around 200ms). This happens after a saccadic motion when the eye rests upon an object and it comes into sharp focus.
* **Dwell.** This is concious fixation by the user for a duration greater than the fixation time. This  mechanism is typically used to identify user intent when the user is using only their eyes as an input method. This time duration is application dependent.
* **Enter/Exit.** These are states and properties specific to this API to help manage gaze related interaction and refer to the time elapsed since the first recorded gaze sample and the last recorded gaze sample on a particular control (Button, ToggleButton etc.)

The GazeApi library enables dwell based gaze interaction on the page by reading the data from the eye tracker over the page invoking specific controls when the user's gaze dwells on a control for a specific time. The application can configure this time based on its usage scenario.

## Quick Start

### To enable gaze interaction on the whole page

Add the following lines to your Page element to enable the whole page for gaze interaction

```xaml
    xmlns:gaze="using:Microsoft.Toolkit.UWP.Input.GazeInteraction"
    gaze:GazeInput.IsGazeEnabled="Enabled"
```

For e.g.

```xaml
    <Page
    x:Class="UwpApp.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:gaze="using:Microsoft.Toolkit.UWP.Input.GazeInteraction"
    gaze:GazeInput.IsGazeEnabled="True"
    mc:Ignorable="d">
```

#### To enable gaze interaction on a portion of the page

Gaze interaction can also be enabled only on a portion of the page by adding the same attributes to any XAML element on the page. 

To enable only a subset of the page, e.g. just one Grid on the page,

```xaml
    <Grid gaze:GazeInput.IsGazeEnabled="Enabled">
        <Button Content="Click Me" />
    <Grid />
```

In the above example, the button will be clicked when the user looks at the button in the grid for a period equal to the default dwell time.



### To change the dwell time for a control

The code below sets the Dwell period for the button to be 500ms. This means the button will be clicked after 500ms after the control enters Fixation state.
(See [PointerState](#PointerState) for details)

```xaml
    <Button Content="Click Me" gaze:GazeInput.Dwell="00:00:00.5">
```

## Properties

### <a name="pointerstate">PointerState</a>

The low level gaze API delivers a stream of timestamped `[x,y]` coordinates for the user's gaze location on the screen. The gaze interaction library aggregates these samples over each control and converts the stream into gaze events. Corresponding to these events, are the following states:

| Property | Type | Description |
| -- | -- | -- |
|Enter | enum | User's gaze has entered a control |
|Fixation| enum | User eye's are focused on the control. |
|Dwell | enum | User is conciously dwelling on the control with an intent to invoke, e.g. click a button|
|RepeatDelay| enum | This is a small delay after Dwell. If the button is configured for repeated invocation, it will do so after the time period associated with this state has elapsed.|
|DwellRepeat| enum | User is continuing to dwell on the control in order to invoke it repeatedly. |
|Exit|enum|User's gaze is no longer on the control|

### GazeInput properties

Whether the page is enabled for the gaze based interaction, the visibility and size of the gaze cursor, and the timings associated with the states above can be configured using the properties below:

| Property | Type | Description |
| -- | -- | -- |
| IsGazeEnabled | enum | Gets or sets the status of gaze interaction over that particular XAML element.  There are three options: <br /> <ul> <li>**Enabled.**  Gaze interaction is enabled on this element and all its children </li> <li> **Disabled** Gaze interaction is disabled on this element and all its children <li> **Inherited** Gaze interaction status is inherited from the nearest ancestor </ul>| 
| CursorVisible | bool | The gaze cursor shows where the user is looking at on the screen. This boolean property shows the gaze cursor when set to `true` and hides it when set to `false` |
|CursorRadius|int|Gets or sets the size of the gaze cursor radius|
| ThresholdDuration | TimeSpan | This duration controls when the PointerState moves to either the `Enter` state or the `Exit` state. When this duration has elapsed after the user's gaze first enters a control, the `PointerState` is set to `Enter`. And when this duration has elapsed after the user's gaze has left the control, the `PointerState` is set to `Exit`. In both cases, a `StateChanged` event is fired with the `PointerState` set to the corresponding value. The default is 50ms. |
| FixationDuration | TimeSpan | Gets or sets the duration for the control to transition from the `Enter` state to the `Fixation` state. At this point, a  `StateChanged` event is fired with `PointerState` set to `Fixation`. This event should be used to control the earliest visual feedback the application needs to provide to the user about the gaze location. The default is 400ms. **CHECK**|
| DwellDuration | TimeSpan | Gets or sets the duration for the control to transition from the `Fixation` state to the `Dwell` state. At this point, a  `StateChanged` event is fired with `PointerState` set to `Dwell`. The `Enter` and `Fixation` states are typicaly achieved too rapidly for the user to have much control over. In contrast `Dwell` is conscious event. This is the point at which the control is invoked, e.g. a button click. The application can modify this property to control when a gaze enabled UI element gets invoked after a user starts looking at it.
| RepeatDelayDuration | TimeSpan | Gets or sets the duration for the control to transition from the `Dwell` state to the `RepeatDelay` state. At this point, a  `StateChanged` event is fired with `PointerState` set to `RepeatDelay`. After this time has elapsed, the control enters  a 'repeat' mode, and the control will be repeatedly invoked as long as the user's gaze stays within the control area. |
| DwellRepeatDuration | TimeSpan | Gets or sets the duration for the control to transition from the `RepeatDelay` state to the `DwellRepeat` state. At this point, a  `StateChanged` event is fired with `PointerState` set to `DwellRepeat`. The control will be repeatedly invoked for every passage of this duration as long as the user's gaze stays within the control. |
| MaxDwellRepeatCount | int | The maximum times the control will invoked repeatedly without the user's gaze having to leave and re-enter the control. The default value is zero which disables repeated invocation of a control. Developers can set a higher value to enable repeated invocation. |


### GazeElement properties

| Property | Type | Description |
| -- | -- | -- |
|HasAttention|bool|A property that gets or sets whether user attention is currently on the control in question|
|InvokeProgress|double|A value between 0 and 1 that indicates the percent time elapsed towards the control being invoked. This property can be used to provide visual feedback to the user|

## GazeElement Events

| Events | Description |
| -- | -- |
| GazePointerEvent | This event is raised in response to each of the states associated with GazePointerState (except for the `DwellRepeat` state). An application can add a handler for this event to customize gaze related processing with respect to the various gaze pointer states mentioned above.|

### StateChangedEvent properties

| Property | Type | Description |
| -- | -- | -- |
|ElapsedTimeSpan|TimeSpan|The time the user has spent looking at the control to reach the specific pointer state above|
|PointerState|GazePointerState|The `GazePointerState` associated with this event|

<!-- Use <remarks> tag in C# to give more info about a propertie. For more info - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/remarks -->


<!-- Use <remarks> tag in C# to give more info about a method. For more info - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/remarks -->

<!-- Use <remarks> tag in C# to give more info about a event. For more info - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/remarks -->

## Examples

<!-- All control/helper must at least have an example to show the use of Properties and Methods in your control/helper with the output -->
<!-- Use <example> and <code> tags in C# to create a Propertie/method specific examples. For more info - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/example -->
<!-- Optional: Codes to achieve real-world use case with the output. For eg: Check https://docs.microsoft.com/en-us/windows/uwpcommunitytoolkit/animations/animationset#examples  -->

## Sample Code

[GazeInteractionPage](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/GazeInteraction/). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal,10.0.17133.0 or higher   |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.Input.Gaze |
| NuGet package | [NuGet package](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Input.Gaze/) |

## API Source Code

* [control/helper name source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.Input.Gaze)

## Related Topics

* [Windows 10 eye gaze API Preview](https://docs.microsoft.com/en-us/uwp/api/windows.devices.input.preview)
