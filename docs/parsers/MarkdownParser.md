---
title: Markdown Parser
author: williamabradley
description: The Markdown Parser allows you to parse a Markdown String into a Markdown Document, and then Render it with a Markdown Renderer.
keywords: uwp community toolkit, uwp toolkit, microsoft community toolkit, microsoft toolkit, markdown, markdown parsing, parser, markdown rendering
---

# Markdown Parser

The [MarkdownDocument](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.parsers.markdown.markdowndocument) class allows you to parse a Markdown String into a Markdown Document, and then Render it with a Markdown Renderer.

## Classes

| Class | Purpose |
| --- | --- |
| **Microsoft.Toolkit.Parsers.Markdown.MarkdownDocument** | Represents a Markdown Document. |
| **Microsoft.Toolkit.Parsers.Markdown.Render.MarkdownRendererBase** | A base renderer for Rendering Markdown into Controls. |

## Syntax

**C#**

```csharp
string md = "This is **Markdown**";
MarkdownDocument Document = new MarkdownDocument();
Document.Parse(md);
``` 

## Create a Markdown Renderer

In order to create a Markdown Renderer, you can either implement your own, or inherit from `MarkdownRenderBase`, this class already has all the required methods, and some assistive code to make implementing a Renderer easy, all you have to do is implement the Block and Inline Rendering, and the output.

This requires an inherited `IRenderContext`, which allows you to keep track of the Context of the rendering.

The best way to figure out how to create a Renderer, is to look at the [implementation](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock/Render) for the UWP MarkdownTextBlock control.

## Sample Code

[Markdown Parser Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/MarkdownParser/MarkdownParserPage.xaml.cs).

You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Implementation | .NET Standard 1.4. |
| -- | -- |
| Namespace | Microsoft.Toolkit.Parsers |
| NuGet package | [Microsoft.Toolkit.Parsers](https://www.nuget.org/packages/Microsoft.Toolkit.Parsers/)  |

## API Source Code

- [Markdown Parser source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Parsers/Markdown)