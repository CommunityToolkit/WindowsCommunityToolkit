//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeFeedbackPopupFactory.h"
#include "GazeInput.h"

BEGIN_NAMESPACE_GAZE_INPUT

Popup GazeFeedbackPopupFactory::Get()
{
    Popup popup;
    winrt::Microsoft::UI::Xaml::Shapes::Rectangle rectangle;

    if (s_cache.size != 0)
    {
        popup = s_cache[0];
        s_cache.erase(s_cache.begin());

        rectangle = popup.Child;
    }
    else
    {
        popup = Popup();

        rectangle = winrt::Microsoft::UI::Xaml::Shapes::Rectangle();
        rectangle.IsHitTestVisible = false;

        popup.Child = rectangle;
    }

    rectangle.StrokeThickness = GazeInput::DwellStrokeThickness;

    return popup;
}

void GazeFeedbackPopupFactory::Return(Popup popup)
{
    popup.IsOpen = false;
    s_cache.push_back(popup);
}

END_NAMESPACE_GAZE_INPUT