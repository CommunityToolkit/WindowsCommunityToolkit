---
title: RSS Parser
author: williamabradley
description: The RSS Parser allows you to parse an RSS content String into RSS Schema.
keywords: windows community toolkit, uwp community toolkit, uwp toolkit, microsoft community toolkit, microsoft toolkit, rss, rss parsing, parser
dev_langs:
  - csharp
  - vb
---

# RSS Parser

The [RssParser](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.parsers.rss.rssparser) class allows you to parse a RSS content String into a RSS Schema.

## Example

```csharp
public async void ParseRSS()
{
    string feed = null;

    using (var client = new HttpClient())
    {
        try
        {
            feed = await client.GetStringAsync("https://visualstudiomagazine.com/rss-feeds/news.aspx");
        }
        catch { }
    }

    if (feed != null)
    {
        var parser = new RssParser();
        var rss = parser.Parse(feed);

        foreach (var element in rss)
        {
            Console.WriteLine($"Title: {element.Title}");
			Console.WriteLine($"Summary: {element.Summary}");
        }
    }
}
```
```vb
Public Async Sub ParseRSS()
    Dim feed As String = Nothing
    Using client = New HttpClient()
        Try
            feed = Await client.GetStringAsync("https://visualstudiomagazine.com/rss-feeds/news.aspx")
        Catch
        End Try
    End Using

    If feed IsNot Nothing Then
        Dim parser = New RssParser()
        Dim rss = parser.Parse(feed)
        For Each element In rss
            Console.WriteLine($"Title: {element.Title}")
            Console.WriteLine($"Summary: {element.Summary}")
        Next
    End If
End Sub
```

## Classes

| Class | Purpose |
| --- | --- |
| **Microsoft.Toolkit.Parsers.Rss.RssParser** | Parser for Parsing RSS Strings into RSS Schema. |
| **Microsoft.Toolkit.Parsers.Rss.RssSchema** | Schema for Parsing RSS. |

### RssParser

#### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Parse(string) | IEnumerable\<RssSchema\> | Parse an RSS content string into RSS Schema. |

## Sample Code

[RSS Parser Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/RssParser/RssParserPage.xaml.cs).

You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Implementation | .NET Standard 1.4. |
| -- | -- |
| Namespace | Microsoft.Toolkit.Parsers |
| NuGet package | [Microsoft.Toolkit.Parsers](https://www.nuget.org/packages/Microsoft.Toolkit.Parsers/)  |

## API Source Code

- [RSS Parser source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Parsers/Rss)