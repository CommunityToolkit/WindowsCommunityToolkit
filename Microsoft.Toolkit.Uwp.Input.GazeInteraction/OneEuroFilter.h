//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeFilter.h"

using namespace winrt::Windows::Foundation;
using namespace winrt::Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

const float ONEEUROFILTER_DEFAULT_BETA = 5.0f;
const float ONEEUROFILTER_DEFAULT_CUTOFF = 0.1f;
const float ONEEUROFILTER_DEFAULT_VELOCITY_CUTOFF = 1.0f;

class LowpassFilter sealed
{
public:
	LowpassFilter() :
		_previous{ Point(0, 0) }
	{
	}

	LowpassFilter(Point initial) :
		_previous{ initial }
	{
	}

	Point Previous() { return _previous; }

	Point Update(Point point, Point alpha)
	{
		Point pt;
		pt.X = (alpha.X * point.X) + ((1 - alpha.X) * _previous.X);
		pt.Y = (alpha.Y * point.Y) + ((1 - alpha.Y) * _previous.Y);
		_previous = pt;
		return _previous;
	}
private:
	Point _previous;
};

class OneEuroFilter sealed : public IGazeFilter
{
public:
	OneEuroFilter();
	OneEuroFilter(float cutoff, float beta);
	virtual GazeFilterArgs Update(GazeFilterArgs args);
	virtual void LoadSettings(ValueSet settings);

public:
	float Beta() { return _beta; }
	float Cutoff() { return _cutoff; }
	float VelocityCutoff() { return _velocityCutoff; }

private:
	float  Alpha(float rate, float cutoff);
	float _beta;
	float _cutoff;
	float _velocityCutoff;

private:
	TimeSpan       _lastTimestamp;
	LowpassFilter  _pointFilter;
	LowpassFilter  _deltaFilter;

};

END_NAMESPACE_GAZE_INPUT
