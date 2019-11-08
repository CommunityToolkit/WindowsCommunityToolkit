//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

using namespace winrt::Windows::Foundation::Collections;
using namespace winrt::Microsoft::UI;
using namespace winrt::Microsoft::UI::Xaml;
using namespace winrt::Microsoft::UI::Xaml::Controls;
using namespace winrt::Microsoft::UI::Xaml::Controls::Primitives;

BEGIN_NAMESPACE_GAZE_INPUT

class GazeCursor sealed
{
private:
	const int DEFAULT_CURSOR_RADIUS = 5;
	const bool DEFAULT_CURSOR_VISIBILITY = true;

public:
	void LoadSettings(ValueSet const& settings);
	int CursorRadius() { return _cursorRadius; }
	void CursorRadius(int const& value);

	bool IsCursorVisible() { return _isCursorVisible; }
	void IsCursorVisible(bool const& value);

	bool IsGazeEntered() { return _isGazeEntered; }
	void IsGazeEntered(bool const& value) { _isGazeEntered = value; }

	Point Position() { return _cursorPosition; }
	void Position(Point const& value)
	{
		_cursorPosition = value;
		_gazePopup.HorizontalOffset = value.X;
		_gazePopup.VerticalOffset = value.Y;
		SetVisibility();
	}

	UIElement PopupChild() { return _gazePopup.Child; };
	void PopupChild(UIElement value) { _gazePopup.Child = value; }


	FrameworkElement CursorElement() { return _gazePopup.Child; }

	GazeCursor();

private:
	void SetVisibility();

	Popup              _gazePopup;
	Point               _cursorPosition = {};
	int                 _cursorRadius = DEFAULT_CURSOR_RADIUS;
	bool                _isCursorVisible = DEFAULT_CURSOR_VISIBILITY;
	bool _isGazeEntered;

};

END_NAMESPACE_GAZE_INPUT