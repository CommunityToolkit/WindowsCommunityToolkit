//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#include "pch.h"

using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

public ref struct GazeFilterArgs sealed
{
    property Point Location {Point get() { return _location; }}
    property TimeSpan Timestamp {TimeSpan get() { return _timestamp; }}

    GazeFilterArgs(Point location, TimeSpan timestamp)
    {
        _location = location;
        _timestamp = timestamp;
    }

private:

    Point _location;
    TimeSpan _timestamp;
};

// Every filter must provide an Wpdate method which transforms sample data 
// and returns filtered output
public interface class IGazeFilter
{
    GazeFilterArgs^ Update(GazeFilterArgs^ args);
    void LoadSettings(ValueSet^ settings);
};


// Basic filter which performs no input filtering -- easy to
// use as a default filter.
public ref class NullFilter sealed : public IGazeFilter
{
public:
    virtual inline GazeFilterArgs^ Update(GazeFilterArgs^ args)
    {
        return args;
    }

    virtual inline void LoadSettings(ValueSet^ settings)
    {

    }
};

END_NAMESPACE_GAZE_INPUT