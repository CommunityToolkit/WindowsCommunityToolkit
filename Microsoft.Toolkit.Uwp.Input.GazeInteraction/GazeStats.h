//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

using namespace Platform::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

private ref class GazeStats sealed
{
public:
    GazeStats(int maxHistoryLen);
    void Reset();
    void Update(float x, float y);

    property Point Mean
    {
        Point get()
        {
            UINT count = _history->Size;
            return Point((float)_sumX / count, (float)_sumY / count);
        }
    }

    //
    // StdDev = sqrt(Variance) = sqrt(E[X^2] – (E[X])^2)
    //
    property Point StandardDeviation
    {
        Point get()
        {
            UINT count = _history->Size;
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
    }

private:
    UINT            _maxHistoryLen;
    double          _sumX;
    double          _sumY;
    double          _sumSquaredX;
    double          _sumSquaredY;
    Vector<Point>^  _history;
};

END_NAMESPACE_GAZE_INPUT
