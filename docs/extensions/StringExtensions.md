---
title: String Extensions
author: avknaidu
description: String extension methods from toolkit
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Extensions, string
---

### String Extensions

Provides helpers and extensions for strings, including validation methods for common scenarios.

## Syntax

```c#
using Microsoft.Toolkit.Extensions;

string str = "test@test.com";
bool isvalid = str.IsEmail();		//Returns true

string str = "123+888";
bool isvalid = str.IsDecimal();		//Returns false
```

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| IsEmail(string) | boolean | Determines whether a string is a valid email address. |
| IsDecimal(string) | boolean | Determines whether a string is a valid decimal number. |
| IsNumeric(string) | boolean | Determines whether a string is a valid integer. |
| IsPhoneNumber(string) | boolean | Determines whether a string is a valid phone number. |
| IsCharacterString(string) | boolean | Determines whether a string contains only letters. |
| ToSafeString(object) | string | Returns a string representation of an object. |
| DecodeHtml(string) | string | Returns a string with HTML comments, scripts, styles, and tags removed. |
| FixHtml(string) | string | Returns a string with HTML comments, scripts, and styles removed. |
| Truncate(string, int) | string | Truncates a string to the specified length. |
| Truncate(string, int, bool) | string | Truncates a string to the specified length, adding an ellipsis (`...`) at the end of the string when set to true. |

### Formats Supported for **IsPhoneNumber** Extension

```
(987) 654-3210
(987)654-3210
987-654-3210
9876543210
+1 9876543210
001 9876543210
001 987-654-3210
19876543210
1-987-654-3210
```

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Extensions |
| NuGet package | [Microsoft.Toolkit](https://www.nuget.org/packages/Microsoft.Toolkit/) |

The String Extensions supports .NET Standard

## API

* [StringExtensions source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit/Extensions/StringExtensions.cs)
