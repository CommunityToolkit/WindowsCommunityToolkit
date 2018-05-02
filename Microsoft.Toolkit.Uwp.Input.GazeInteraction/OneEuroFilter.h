//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "IGazeFilter.h"

using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

const float ONEEUROFILTER_DEFAULT_BETA = 5.0f;
const float ONEEUROFILTER_DEFAULT_CUTOFF = 0.1f;
const float ONEEUROFILTER_DEFAULT_VELOCITY_CUTOFF = 1.0f;

private ref class LowpassFilter sealed
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

private ref class OneEuroFilter sealed : public IGazeFilter
{
public:
    OneEuroFilter();
    OneEuroFilter(float cutoff, float beta);
    virtual GazeFilterArgs^ Update(GazeFilterArgs^ args);
    virtual void LoadSettings(ValueSet^ settings);

public:
    property float Beta;
    property float Cutoff;
    property float VelocityCutoff;

private:
    float  Alpha(float rate, float cutoff);

private:
    TimeSpan       _lastTimestamp;
    LowpassFilter^  _pointFilter;
    LowpassFilter^  _deltaFilter;

};

END_NAMESPACE_GAZE_INPUT
