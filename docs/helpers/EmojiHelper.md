# EmojiHelper class

EmojiHelper provides attached properties that you can use to set emojis to either a TextBlock or to the Run element used within a TextBlock without the need for the EmojiIcon. Both properties use their own enum values to aid with choosing the right one.

## Example

```xml
    
    <TextBlock emoji:EmojiHelper.Emoji="CoupleWithHeart" />
    
    <TextBlock>
        <Run Text="The emoji is:" />
        <Run emoji:EmojiHelper.Emoji="CoupleWithHeart" />
        <Run emoji:EmojiHelper.Emoji="CoupleWithHeart"
             emoji:EmojiHelper.EmojiSkinTone="Type6" />
    </TextBlock>
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Emoji |

## API

* [EmojiHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp.UI/Helpers/Emoji/EmojiHelper.cs)

