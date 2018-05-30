---
title: Array Extensions
author: michael-hawker
description: Array extension methods from toolkit
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Extensions, array
---

### How it works

Provides a few helpers for dealing with multi-dimensional and jagged arrays.  Also, provides string helpers for debug output.

## Syntax

```c#

using Microsoft.Toolkit.Extensions;

bool[,] inside = new bool[4, 5];

// Fill the inside of the boolean array with 'true' values.
inside.Fill(true, 1, 1, 3, 2);

Debug.WriteLine(inside.ToArrayString());

/*
Output:
[[False,	False,	False,	False,	False],
 [False,	True,	True,	True,	False],
 [False,	True,	True,	True,	False],
 [False,	False,	False,	False,	False]]
 */
```

&nbsp;

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Fill | void | Fills in values of a multi-dimensional rectangular array to specified value based on the position and size given. |
| GetRow | IEnumerable | Retrieve a row as an enumerable from a multi-dimensional rectangular array. |
| GetColumn | IEnumerable | Retrieve a column from a multi-dimensional rectangular or jagged array. |
| ToArrayString | string | Create a nice debug string representation of an array, multi-dimensional rectangular, or jagged array. |

&nbsp;

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit |
| NuGet package | [Microsoft.Toolkit](https://www.nuget.org/packages/Microsoft.Toolkit/) |

The Array Extensions supports .NET Standard

&nbsp;

## API

* [ArrayExtensions source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit/Extensions/ArrayExtensions.cs)