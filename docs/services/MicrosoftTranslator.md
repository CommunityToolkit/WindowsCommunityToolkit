---
title: Microsoft Translator Service
author: nmetulev
description: The Microsoft Translator Service allows you to translate text to various supported languages.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, MicrosoftTranslator
dev_langs:
  - csharp
  - vb
---

# Microsoft Translator Service

The **Microsoft Translator Service** allows you to translate text to various supported languages.

## Set up Microsoft Translator Service

[Signup for Microsoft Translator Service](https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation) using your Microsoft Azure subscription account. There is a free trial option for that allows you to translate up to 2,000,000 characters per month.

## Example Syntax

```csharp
// using Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator;

await TranslatorService.Instance.InitializeAsync("<translator service key");

// Retrieves friendly names for the languages available for text translation.
var languages = await TranslatorService.Instance.GetLanguageNamesAsync();

// Detects the language of a text.
var language = await TranslatorService.Instance.DetectLanguageAsync("Hello everyone!");

// Translates the text to Italian.
var translatedText = await TranslatorService.Instance.TranslateAsync("Hello everyone!", "it");
```
```vb
' Imports Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator

Await TranslatorService.Instance.InitializeAsync("<translator service key")

' Retrieves friendly names for the languages available for text translation.
Dim languages = Await TranslatorService.Instance.GetLanguageNamesAsync()

' Detects the language of a text.
Dim language = Await TranslatorService.Instance.DetectLanguageAsync("Hello everyone!")

' Translates the text to Italian.
Dim translatedText = Await TranslatorService.Instance.TranslateAsync("Hello everyone!", "it")
```

## Sample Code

[Microsoft Translator Service Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Microsoft%20Translator%20Service). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |
| NuGet package | [Microsoft.Toolkit.Uwp.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Services/) |

## API

* [Microsoft Translator Service source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.Services/Services/MicrosoftTranslator)
