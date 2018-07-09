//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

using namespace Windows::Foundation::Collections;
using namespace Windows::UI;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;

BEGIN_NAMESPACE_GAZE_INPUT

private ref class GazeCursor sealed
{
private:
    const int DEFAULT_CURSOR_RADIUS = 5;
    const bool DEFAULT_CURSOR_VISIBILITY = true;

public:
    void LoadSettings(ValueSet^ settings);
    property int CursorRadius
    {
        int get() { return _cursorRadius; }
        void set(int value);
    }

    property bool IsCursorVisible
    {
        bool get() { return _isCursorVisible; }
        void set(bool value);
    }

    property bool IsGazeEntered
    {
        bool get() { return _isGazeEntered; }
        void set(bool value);
    }

    property Point Position
    {
        Point get()
        {
            return _cursorPosition;
        }

        void set(Point value)
        {
            _cursorPosition = value;
            _gazePopup->HorizontalOffset = value.X - CursorRadius;
            _gazePopup->VerticalOffset = value.Y - CursorRadius;
            SetVisibility();
        }
    }

internal:
    GazeCursor();

private:
    void SetVisibility();

    Popup^              _gazePopup;
    Shapes::Ellipse^    _gazeCursor;
    Point               _cursorPosition = {};
    int                 _cursorRadius = DEFAULT_CURSOR_RADIUS;
    bool                _isCursorVisible = DEFAULT_CURSOR_VISIBILITY;
    bool _isGazeEntered;

};

END_NAMESPACE_GAZE_INPUT