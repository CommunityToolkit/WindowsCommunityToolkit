//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

using namespace winrt::Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

class GazeStats sealed
{
public:
    GazeStats(unsigned int maxHistoryLen);
    void Reset();
    void Update(float x, float y);

    Point Mean()
    {
        UINT count = _history.size();
        return Point((float)_sumX / count, (float)_sumY / count);
    }

    //
    // StdDev = sqrt(Variance) = sqrt(E[X2] – (E[X])2)
    //
    Point StandardDeviation()
    {
        UINT count = _history.size();
        if (count < _maxHistoryLen)
        {
            return Point(0.0f, 0.0f);
        }
        double meanX = _sumX / count;
        double meanY = _sumY / count;
        float stddevX = (float)sqrt((_sumSquaredX / count) - (meanX * meanX));
        float stddevY = (float)sqrt((_sumSquaredY / count) - (meanY * meanY));
        return Point(stddevX, stddevY);
    }

private:
    UINT            _maxHistoryLen;
    double          _sumX;
    double          _sumY;
    double          _sumSquaredX;
    double          _sumSquaredY;
    std::vector<Point>  _history;
};

END_NAMESPACE_GAZE_INPUT
