//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeStats.h"

using namespace winrt::Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

GazeStats::GazeStats(unsigned int maxHistoryLen) :
    _maxHistoryLen{ maxHistoryLen }
{
}

void GazeStats::Reset()
{
    _sumX = 0;
    _sumY = 0;
    _sumSquaredX = 0;
    _sumSquaredY = 0;
    _history.clear();
}

void GazeStats::Update(float x, float y)
{
    Point pt(x, y);
    _history.push_back(pt);

    if (_history.size() > _maxHistoryLen)
    {
        auto oldest = _history.at(0);
        _history.erase(_history.begin());
            
        _sumX -= oldest.X;
        _sumY -= oldest.Y;
        _sumSquaredX -= oldest.X * oldest.X;
        _sumSquaredY -= oldest.Y * oldest.Y;
    }
    _sumX += x;
    _sumY += y;
    _sumSquaredX += x * x;
    _sumSquaredY += y * y;
}

END_NAMESPACE_GAZE_INPUT
