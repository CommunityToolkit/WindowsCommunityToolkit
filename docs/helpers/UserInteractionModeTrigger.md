# UserInteractionModeTrigger

The **UserInteractionModeTrigger** allows changing the visual state depending on the current interaction style (mouse or touch) of the device.

## Example

```xaml
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup>
            <VisualState>
                <VisualState.StateTriggers>
                    <t:UserInteractionModeTrigger Mode="Touch" />
                </VisualState.StateTriggers>
                <VisualState.Setters>
                    <Setter Target="ListView.ItemContainerStyle" Value="{StaticResource TouchListViewItemStyle}"/>
                </VisualState.Setters>
            </VisualState>
            <VisualState>
                <VisualState.StateTriggers>
                    <t:UserInteractionModeTrigger Mode="Mouse" />
                </VisualState.StateTriggers>
                <VisualState.Setters>
                    <Setter Target="ListView.ItemContainerStyle" Value="{StaticResource MouseListViewItemStyle}"/>
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Triggers |

## API

* [UserInteractionModeTrigger source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/Triggers/UserInteractionModeTrigger.cs)

