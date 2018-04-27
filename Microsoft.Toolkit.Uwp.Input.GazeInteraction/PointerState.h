//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// TODO: harishsk
/// </summary>
public enum class PointerState
{
    /// <summary>
    /// TODO: harishsk
    /// </summary>
    Exit,

    // The order of the following elements is important because
    // they represent states that linearly transition to their
    // immediate successors. 

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    PreEnter,

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    Enter,

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    Fixation,

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    Dwell,

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    DwellRepeat
};

END_NAMESPACE_GAZE_INPUT