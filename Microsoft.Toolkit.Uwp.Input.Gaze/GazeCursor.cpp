//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeCursor.h"

BEGIN_NAMESPACE_GAZE_INPUT

GazeCursor::GazeCursor()
{
    _gazeSettings = GazeSettings::Instance;

    _cursorRadius = _gazeSettings->GazeCursor_Cursor_Radius;
    _isCursorVisible = _gazeSettings->GazeCursor_Cursor_Visibility;

    _gazePopup = ref new Popup();
    _gazeCanvas = ref new Canvas();

    _gazeCursor = ref new Shapes::Ellipse();
    _gazeCursor->Fill = ref new SolidColorBrush(Colors::IndianRed);
    _gazeCursor->VerticalAlignment = Windows::UI::Xaml::VerticalAlignment::Top;
    _gazeCursor->HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Left;
    _gazeCursor->Width = 2 * CursorRadius;
    _gazeCursor->Height = 2 * CursorRadius;

    _origSignalCursor = ref new Shapes::Ellipse();
    _origSignalCursor->Fill = ref new SolidColorBrush(Colors::Green);
    _origSignalCursor->VerticalAlignment = Windows::UI::Xaml::VerticalAlignment::Top;
    _origSignalCursor->HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Left;
    _origSignalCursor->Width = 2 * CursorRadius;
    _origSignalCursor->Height = 2 * CursorRadius;

    _gazeRect = ref new Shapes::Rectangle();
    _gazeRect->IsHitTestVisible = false;

    _gazeCanvas->Children->Append(_gazeCursor);
    _gazeCanvas->Children->Append(_gazeRect);

    // TODO: Reenable this once GazeCursor is refactored correctly
    //_gazeCanvas->Children->Append(_origSignalCursor);

    _gazePopup->Child = _gazeCanvas;
    _gazePopup->IsOpen = IsCursorVisible;
}

void GazeCursor::CursorRadius::set(int value)
{
    _cursorRadius = value;
    if (_gazeCursor != nullptr)
    {
        _gazeCursor->Width = 2 * _cursorRadius;
        _gazeCursor->Height = 2 * _cursorRadius;
    }
}

void GazeCursor::IsCursorVisible::set(bool value)
{
    _isCursorVisible = value;
    if (_gazePopup != nullptr)
    {
        _gazePopup->IsOpen = _isCursorVisible;
    }
}

END_NAMESPACE_GAZE_INPUT