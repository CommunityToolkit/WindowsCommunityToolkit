//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#include "pch.h"

using namespace Windows::Foundation;

BEGIN_NAMESPACE_GAZE_INPUT

public ref struct GazeEventArgs sealed
{
    property Point Location;
    property int64 Timestamp;

    GazeEventArgs(Point location, int64 timestamp)
    {
        Location = location;
        Timestamp = timestamp;
    }
};

// Every filter must provide an Wpdate method which transforms sample data 
// and returns filtered output
public interface class IGazeFilter
{
    GazeEventArgs^ Update(GazeEventArgs^ args);
};


// Basic filter which performs no input filtering -- easy to
// use as a default filter.
public ref class NullFilter sealed : public IGazeFilter
{
public:
    virtual inline GazeEventArgs^ Update(GazeEventArgs^ args)
    {
        return args;
    }
};

END_NAMESPACE_GAZE_INPUT