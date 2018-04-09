//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#include "pch.h"
#include "IGazeFilter.h"
#include "GazeSettings.h"

using namespace Windows::Foundation;

BEGIN_NAMESPACE_GAZE_INPUT

public ref class LowpassFilter sealed
{
public:
    LowpassFilter()
    {
        Previous = Point(0, 0);
    }

    LowpassFilter(Point initial)
    {
        Previous = initial;
    }

    property Point Previous;

    Point Update(Point point, Point alpha)
    {
        Point pt;
        pt.X = (alpha.X * point.X) + ((1 - alpha.X) * Previous.X);
        pt.Y = (alpha.Y * point.Y) + ((1 - alpha.Y) * Previous.Y);
        Previous = pt;
        return Previous;
    }
};

public ref class OneEuroFilter sealed : public IGazeFilter
{
public:
    OneEuroFilter();
    OneEuroFilter(float cutoff, float beta);
    virtual GazeEventArgs^ Update(GazeEventArgs^ args);

public:
    property float Beta;
    property float Cutoff;
    property float VelocityCutoff;

private:
    float  Alpha(float rate, float cutoff);

private:
    long long       _lastTimestamp;
    LowpassFilter^  _pointFilter;
    LowpassFilter^  _deltaFilter;

    GazeSettings^   _gazeSettings;
};

END_NAMESPACE_GAZE_INPUT
