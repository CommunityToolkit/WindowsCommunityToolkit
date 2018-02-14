---
title: String Extensions
author: avknaidu
description: String extension methods from toolkit
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Extensions
---

### How it works

Developers can now leverage the StringExtension class and validate strings against common scenario's

## Syntax

```c#

using Microsoft.Toolkit;

string str = "test@test.com";
bool isvalid = str.IsEmail();		//Returns true

string str = "123+888";
bool isvalid = str.IsDecimal();		//Returns false

```

&nbsp;

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| IsEmail(string) | boolean | Returns whether said string is a valid email or not. Uses general Email Regex (RFC 5322 Official Standard) from emailregex.com |
| IsDecimal(string) | boolean | Returns whether said string is a valid decimal number or not. |
| IsNumeric(string) | boolean | Returns whether said string is a valid integer or not. |
| IsPhoneNumber(string) | boolean | Returns whether said string is a valid phonenumber or not. |
| IsCharacterString(string) | boolean | Returns whether said string contains only letters or not. |
| ToSafeString(object) | string | Converts object into string. |
| DecodeHtml(string) | string | Decode HTML string. |
| FixHtml(string) | string | Applies regular expressions to string of HTML to remove comments, scripts, styles. |
| Truncate(string, int) | string | Trims and Truncates the specified string to the specified length. |
| Truncate(string, int, bool) | string | Trims and Truncates the specified string to the specified length. |

&nbsp;

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit |

&nbsp;

## API

* [StringExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit/Extensions)