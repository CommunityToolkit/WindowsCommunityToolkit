//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "OneEuroFilter.h"

//
// http://www.lifl.fr/~casiez/1euro/
// http://www.lifl.fr/~casiez/publications/CHI2012-casiez.pdf
//

BEGIN_NAMESPACE_GAZE_INPUT

OneEuroFilter::OneEuroFilter()
{

    _lastTimestamp = TimeSpanZero;
    Beta = ONEEUROFILTER_DEFAULT_BETA;
    Cutoff = ONEEUROFILTER_DEFAULT_CUTOFF;
    VelocityCutoff = ONEEUROFILTER_DEFAULT_VELOCITY_CUTOFF;
}

OneEuroFilter::OneEuroFilter(float cutoff, float beta)
{
    _lastTimestamp = TimeSpanZero;
    Beta = beta;
    Cutoff = cutoff;
    VelocityCutoff = ONEEUROFILTER_DEFAULT_VELOCITY_CUTOFF;
}

GazeFilterArgs^ OneEuroFilter::Update(GazeFilterArgs^ args)
{
    if (_lastTimestamp == TimeSpanZero)
    {
        _lastTimestamp = args->Timestamp;
        _pointFilter = ref new LowpassFilter(args->Location);
        _deltaFilter = ref new LowpassFilter(Point());
        return ref new GazeFilterArgs(args->Location, args->Timestamp);
    }

    Point gazePoint = args->Location;

    // Reducing _beta increases lag. Increasing beta decreases lag and improves response time
    // But a really high value of beta also contributes to jitter
    float beta = Beta;

    // This simply represents the cutoff frequency. A lower value reduces jiiter
    // and higher value increases jitter
    float cf = Cutoff;
    Point cutoff = Point(cf, cf);

    // determine sampling frequency based on last time stamp
    float samplingFrequency = 10000000.0f / max(1, (args->Timestamp - _lastTimestamp).Duration);
    _lastTimestamp = args->Timestamp;

    // calculate change in distance...
    Point deltaDistance;
    deltaDistance.X = gazePoint.X - _pointFilter->Previous.X;
    deltaDistance.Y = gazePoint.Y - _pointFilter->Previous.Y;

    // ...and velocity
    Point velocity(deltaDistance.X * samplingFrequency, deltaDistance.Y * samplingFrequency);

    // find the alpha to use for the velocity filter
    float velocityAlpha = Alpha(samplingFrequency, VelocityCutoff);
    Point velocityAlphaPoint(velocityAlpha, velocityAlpha);

    // find the filtered velocity
    Point filteredVelocity = _deltaFilter->Update(velocity, velocityAlphaPoint);

    // ignore sign since it will be taken care of by deltaDistance
    filteredVelocity.X = abs(filteredVelocity.X);
    filteredVelocity.Y = abs(filteredVelocity.Y);

    // compute new cutoff to use based on velocity
    cutoff.X += beta * filteredVelocity.X;
    cutoff.Y += beta * filteredVelocity.Y;

    // find the new alpha to use to filter the points
    Point distanceAlpha(Alpha(samplingFrequency, cutoff.X), Alpha(samplingFrequency, cutoff.Y));

    // find the filtered point
    Point filteredPoint = _pointFilter->Update(gazePoint, distanceAlpha);

    // compute the new args
    auto fa = ref new GazeFilterArgs(filteredPoint, args->Timestamp);
    return fa;
}

float OneEuroFilter::Alpha(float rate, float cutoff)
{
    const float PI = 3.14159265f;
    float  te = 1.0f / rate;
    float tau = (float)(1.0f / (2 * PI * cutoff));
    float alpha = te / (te + tau);
    return alpha;
}
void OneEuroFilter::LoadSettings(ValueSet^ settings)
{
    if (settings->HasKey("OneEuroFilter.Beta"))
    {
        Beta = (float)(settings->Lookup("OneEuroFilter.Beta"));
    }
    if (settings->HasKey("OneEuroFilter.Cutoff"))
    {
        Cutoff = (float)(settings->Lookup("OneEuroFilter.Cutoff"));
    }
    if (settings->HasKey("OneEuroFilter.VelocityCutoff"))
    {
        VelocityCutoff = (float)(settings->Lookup("OneEuroFilter.VelocityCutoff"));
    }
}

END_NAMESPACE_GAZE_INPUT