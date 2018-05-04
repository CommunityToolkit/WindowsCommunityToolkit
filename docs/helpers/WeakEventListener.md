---
title: WeakEventListener
author: nmetulev
description: The WeakEventListener allows the owner to be garbage collected if its only remaining link is an event handler.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, WeakEventListener
---

# WeakEventListener

The [WeakEventListener](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.weakeventlistener-3) allows the owner to be garbage collected if its only remaining link is an event handler.

## Properties

| Property | Type | Description |
| -- | -- | -- |
| OnDetachAction | WeakEventListener<TInstance,TSource,TEventArgs>> | Gets or sets the method to call when detaching from the event |
| OnEventAction | Action<TInstance,TSource,TEventArgs> | Gets or sets the method to call when the event fires |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Detach() | void | Detaches from the subscribed event |
| OnEvent(TSource, TEventArgs) | void | Handler for the subscribed event calls OnEventAction to handle it |

## Sample Code

You can find examples of WeakEventListener in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_WeakEventListener.cs)

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API Source Code

* [WeakEventListener source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/WeakEventListener.cs)
