---
title: Markdown Parser
author: williamabradley
description: The Markdown Parser allows you to parse a Markdown String into a Markdown Document, and then Render it with a Markdown Renderer.
keywords: windows community toolkit, uwp community toolkit, uwp toolkit, microsoft community toolkit, microsoft toolkit, markdown, markdown parsing, parser, markdown rendering
dev_langs:
  - csharp
  - vb
---

# Markdown Parser

The [MarkdownDocument](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.parsers.markdown.markdowndocument) class allows you to parse a Markdown String into a Markdown Document, and then Render it with a Markdown Renderer.

## Example

```csharp
string md = "This is **Markdown**";
MarkdownDocument document = new MarkdownDocument();
document.Parse(md);

// Takes note of all of the Top Level Headers.
foreach (var element in document.Blocks)
{
    if (element is HeaderBlock header)
    {
        Console.WriteLine($"Header: {header.ToString()}");
    }
}
```
```vb
Dim md As String = "This is **Markdown**"
Dim document As MarkdownDocument = New MarkdownDocument()
document.Parse(md)

For Each element In document.Blocks
    If TypeOf element Is HeaderBlock Then
        Console.WriteLine($"Header: {element.ToString()}")
    End If
Next
End Sub
```

## Classes

| Class | Purpose |
| --- | --- |
| **Microsoft.Toolkit.Parsers.Markdown.MarkdownDocument** | Represents a Markdown Document. |
| **Microsoft.Toolkit.Parsers.Markdown.Render.MarkdownRendererBase** | A base renderer for Rendering Markdown into Controls. |

### MarkdownDocument

#### Properties

| Property | Type | Description |
| -- | -- | -- |
| Blocks | IList\<MarkdownBlock\> | Gets or sets the list of block elements. |

#### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Parse(string) | void | Parses markdown document text. |
| LookUpReference(string) | LinkReferenceBlock | Looks up a reference using the ID. |

## Create a Markdown Renderer

In order to create a Markdown Renderer, you can either implement your own, or inherit from `MarkdownRenderBase`, this class already has all the required methods, and some assistive code to make implementing a Renderer easy, all you have to do is implement the Block and Inline Rendering, and the output.

This requires an inherited `IRenderContext`, which allows you to keep track of the Context of the rendering.

The best way to figure out how to create a Renderer, is to look at the [implementation](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock/Render) for the UWP MarkdownTextBlock control.

## Sample Code

[Markdown Parser Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/MarkdownParser/MarkdownParserPage.xaml.cs).

You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Implementation | .NET Standard 1.4. |
| -- | -- |
| Namespace | Microsoft.Toolkit.Parsers |
| NuGet package | [Microsoft.Toolkit.Parsers](https://www.nuget.org/packages/Microsoft.Toolkit.Parsers/)  |

## API Source Code

- [Markdown Parser source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Parsers/Markdown)