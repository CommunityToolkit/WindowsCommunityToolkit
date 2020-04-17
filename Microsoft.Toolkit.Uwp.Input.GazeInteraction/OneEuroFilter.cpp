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
    _beta = ONEEUROFILTER_DEFAULT_BETA;
    _cutoff = ONEEUROFILTER_DEFAULT_CUTOFF;
    _velocityCutoff = ONEEUROFILTER_DEFAULT_VELOCITY_CUTOFF;
}

OneEuroFilter::OneEuroFilter(float cutoff, float beta)
{
    _lastTimestamp = TimeSpanZero;
    _beta = beta;
    _cutoff = cutoff;
    _velocityCutoff = ONEEUROFILTER_DEFAULT_VELOCITY_CUTOFF;
}

GazeFilterArgs OneEuroFilter::Update(GazeFilterArgs args)
{
    if (_lastTimestamp == TimeSpanZero)
    {
        _lastTimestamp = args.Timestamp();
        _pointFilter = LowpassFilter(args.Location());
        _deltaFilter = LowpassFilter(Point());
        return GazeFilterArgs(args.Location(), args.Timestamp());
    }

    Point gazePoint = args.Location();

    // Reducing _beta increases lag. Increasing beta decreases lag and improves response time
    // But a really high value of beta also contributes to jitter
    float beta = _beta;

    // This simply represents the cutoff frequency. A lower value reduces jiiter
    // and higher value increases jitter
    float cf = _cutoff;
    Point cutoff = Point(cf, cf);

    // determine sampling frequency based on last time stamp
    float samplingFrequency = 10000000.0f / max(1, (args.Timestamp() - _lastTimestamp).count());
    _lastTimestamp = args.Timestamp();

    // calculate change in distance...
    Point deltaDistance;
    deltaDistance.X = gazePoint.X - _pointFilter.Previous().X;
    deltaDistance.Y = gazePoint.Y - _pointFilter.Previous().Y;

    // ...and velocity
    Point velocity(deltaDistance.X * samplingFrequency, deltaDistance.Y * samplingFrequency);

    // find the alpha to use for the velocity filter
    float velocityAlpha = Alpha(samplingFrequency, VelocityCutoff());
    Point velocityAlphaPoint(velocityAlpha, velocityAlpha);

    // find the filtered velocity
    Point filteredVelocity = _deltaFilter.Update(velocity, velocityAlphaPoint);

    // ignore sign since it will be taken care of by deltaDistance
    filteredVelocity.X = abs(filteredVelocity.X);
    filteredVelocity.Y = abs(filteredVelocity.Y);

    // compute new cutoff to use based on velocity
    cutoff.X += beta * filteredVelocity.X;
    cutoff.Y += beta * filteredVelocity.Y;

    // find the new alpha to use to filter the points
    Point distanceAlpha(Alpha(samplingFrequency, cutoff.X), Alpha(samplingFrequency, cutoff.Y));

    // find the filtered point
    Point filteredPoint = _pointFilter.Update(gazePoint, distanceAlpha);

    // compute the new args
    auto fa = GazeFilterArgs(filteredPoint, args.Timestamp());
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
void OneEuroFilter::LoadSettings(ValueSet settings)
{
    if (settings.HasKey(L"OneEuroFilter.Beta"))
    {
        _beta = winrt::unbox_value<float>(settings.Lookup(L"OneEuroFilter.Beta"));
    }
    if (settings.HasKey(L"OneEuroFilter.Cutoff"))
    {
        _cutoff = winrt::unbox_value<float>(settings.Lookup(L"OneEuroFilter.Cutoff"));
    }
    if (settings.HasKey(L"OneEuroFilter.VelocityCutoff"))
    {
        _velocityCutoff = winrt::unbox_value<float>(settings.Lookup(L"OneEuroFilter.VelocityCutoff"));
    }
}

END_NAMESPACE_GAZE_INPUT