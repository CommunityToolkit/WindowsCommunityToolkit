//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeCursor.h"
#include <winrt/Microsoft.UI.Xaml.Media.h>

using namespace winrt::Microsoft::UI::Xaml::Media;

BEGIN_NAMESPACE_GAZE_INPUT

GazeCursor::GazeCursor()
{
    _gazePopup = Popup();
    _gazePopup.IsHitTestVisible(false);

    auto gazeCursor = Shapes::Ellipse();
    gazeCursor.Fill(SolidColorBrush(Colors::IndianRed()));
    gazeCursor.VerticalAlignment(winrt::Microsoft::UI::Xaml::VerticalAlignment::Top);
    gazeCursor.HorizontalAlignment(winrt::Microsoft::UI::Xaml::HorizontalAlignment::Left);
    gazeCursor.Width(2 * CursorRadius());
    gazeCursor.Height(2 * CursorRadius());
	gazeCursor.Margin(ThicknessHelper::FromLengths(-CursorRadius(), -CursorRadius(), 0, 0));
    gazeCursor.IsHitTestVisible(false);

    _gazePopup.Child(gazeCursor);
}

void GazeCursor::CursorRadius(int const& value)
{
    _cursorRadius = value;
	auto gazeCursor = CursorElement();
	if (gazeCursor != nullptr)
	{
		gazeCursor.Width(2 * _cursorRadius);
		gazeCursor.Height(2 * _cursorRadius);
		gazeCursor.Margin(ThicknessHelper::FromLengths(-_cursorRadius, -_cursorRadius, 0, 0));
	}
}

void GazeCursor::IsCursorVisible(bool const& value)
{
    _isCursorVisible = value;
    SetVisibility();
}

void GazeCursor::IsGazeEntered(bool const& value)
{
    _isGazeEntered = value;
    SetVisibility();
}

void GazeCursor::LoadSettings(ValueSet const& settings)
{
    if (settings.HasKey(L"GazeCursor.CursorRadius"))
    {
        CursorRadius(winrt::unbox_value<int>(settings.Lookup(L"GazeCursor.CursorRadius")));
    }
    if (settings.HasKey(L"GazeCursor.CursorVisibility"))
    {
        IsCursorVisible((bool)(settings.Lookup(L"GazeCursor.CursorVisibility")));
    }
}

void GazeCursor::SetVisibility()
{
    auto isOpen = _isCursorVisible && _isGazeEntered;
    if (_gazePopup.IsOpen() != isOpen)
    {
        _gazePopup.IsOpen(isOpen);
    }
    else if (isOpen)
    {
        auto topmost = VisualTreeHelper::GetOpenPopups(Window::Current()).First().Current();
        if (_gazePopup != topmost)
        {
            _gazePopup.IsOpen(false);
            _gazePopup.IsOpen(true);
        }
    }
}

END_NAMESPACE_GAZE_INPUT