//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// This enum reflects the states that a user's gaze through while interacting with a control using their eyes.
/// </summary>
public enum class PointerState
{
    /// <summary>
    /// User's gaze is no longer on the control
    /// </summary>
    Exit = 0,

    // The order of the following elements is important because
    // they represent states that linearly transition to their
    // immediate successors. 

    /// <summary>
    /// For internal use only
    /// </summary>
    PreEnter = 1,

    /// <summary>
    /// User's gaze has entered a control
    /// </summary>
    Enter = 2,

    /// <summary>
    /// User eye's are focused on the control
    /// </summary>
    Fixation = 3,

    /// <summary>
    /// User is conciously dwelling on the control with an intent to invoke, e.g. click a button
    /// </summary>
    Dwell = 4,

    /// <summary>
    /// User is continuing to dwell on the control for repeated invocation. (For internal use only)
    /// </summary>
    DwellRepeat = 5
};

END_NAMESPACE_GAZE_INPUT
