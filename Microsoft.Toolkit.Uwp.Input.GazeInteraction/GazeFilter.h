//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

using namespace winrt::Windows::Foundation;
using namespace winrt::Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// This struct encapsulates the location and timestamp associated with the user's gaze 
/// and is used as an input and output parameter for the IGazeFilter::Update method
/// </summary>
struct GazeFilterArgs sealed
{
    /// <summary>
    /// The current point in the gaze stream
    /// </summary>
    Point Location () { return _location; }

    /// <summary>
    /// The timestamp associated with the current point
    /// </summary>
    TimeSpan Timestamp () { return _timestamp; }

    GazeFilterArgs(Point location, TimeSpan timestamp):
        _location{ location },
        _timestamp{ timestamp }
    {
    }

private:

    Point _location;
    TimeSpan _timestamp;
};

// Every filter must provide an Update method which transforms sample data
// and returns filtered output
class IGazeFilter
{
public:
    virtual GazeFilterArgs Update(GazeFilterArgs args) = 0;
    virtual void LoadSettings(ValueSet settings) = 0;
};


// Basic filter which performs no input filtering -- easy to
// use as a default filter.
class NullFilter sealed : public IGazeFilter
{
public:
    GazeFilterArgs Update(GazeFilterArgs args);

    void LoadSettings(ValueSet settings);
};

END_NAMESPACE_GAZE_INPUT