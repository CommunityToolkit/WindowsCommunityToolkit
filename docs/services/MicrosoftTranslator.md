---
title: Microsoft Translator Service
author: nmetulev
ms.date: 08/20/2017
description: The Microsoft Translator Service allows you to translate text to various supported languages.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, MicrosoftTranslator
---

# Microsoft Translator Service

The **Microsoft Translator Service** allows you to translate text to various supported languages.

## Set up Microsoft Translator Service

[Signup for Microsot Translator Service](https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation) using your Microsoft Azure subscription account. There is a free trial option for that allows you to translate up to 2,000,000 characters per month.

## Example Syntax

```csharp

// using Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator;

await TranslatorService.Instance.InitializeAsync("<translator service key");

// Translates the text to Italian.
var translatedText = await TranslatorService.Instance.TranslateAsync("Hello everyone!", "it");
```

## Example

[Microsoft Translator Service Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Microsoft%20Translator%20Service)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |

## API

* [Microsoft Translator Service source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.Services/Services/MicrosoftTranslator)


## NuGet Packages Required

**Microsoft.Toolkit.Uwp.Services**

See the [NuGet Packages page](../Nuget-Packages.md) for complete list.
