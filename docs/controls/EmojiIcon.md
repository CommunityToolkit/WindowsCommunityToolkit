# EmojiIcon Control

The **EmojiIcon Control** provides an easy way to render any of the 1000+ emojis from within your UWP app. Some emojis support the multi-color skin tone modifier in order to change how the emoji looks, if the emoji you've chose supports this, you can set the EmojiSkinTone property to change it. If it doesn't changing this property will result in no changes to your chose emoji. Both properties use their own enum values to aid with choosing the right one
You can use these properties :

* Emoji
* EmojiSkinTone

## Syntax

```xml

<controls:EmojiIcon Emoji="CoupleWithHeart"
                    EmojiSkinTone="Type5"
                    FontSize="30" />

```

## Example Code

[EmojiIcon Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/EmojiIcon)

## Default Template 

[EmojiIcon XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/EmojiIcon/EmojiIcon.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family]("http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [EmojiIcon source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/EmojiIcon)