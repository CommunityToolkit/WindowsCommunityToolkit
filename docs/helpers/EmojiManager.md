# EmojiManager class

EmojiManager provides extension methods that you can use to get the text string for an emoji or to determine whether your selected emoji supports multi-color modifiers.

## Example

```c#
    
string emoji = Emoji.SmilingFaceWithHalo.GetEmoji();
EmojiIcon.Text = emoji;

bool supportsMultiColorModifier = Emoji.SmilingFaceWithHalo.SupportsMultipleColors();
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Emoji |

## API

* [EmojiHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp.UI/Helpers/Emoji/EmojiManager.cs)

