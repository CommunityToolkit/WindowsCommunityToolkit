#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

public ref struct GazeEventArgs sealed
{
    property bool Handled;
    property Point Location;
    property TimeSpan Timestamp;

    GazeEventArgs()
    {
    }

    void Set(Point location, TimeSpan timestamp)
    {
        Handled = false;
        Location = location;
        Timestamp = timestamp;
    }
};

END_NAMESPACE_GAZE_INPUT
