//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

using namespace Windows::Foundation;

BEGIN_NAMESPACE_GAZE_INPUT

ref class GazeTargetItem;

private ref struct GazeHistoryItem
{
    property GazeTargetItem^ HitTarget;
    property TimeSpan Timestamp;
    property TimeSpan Duration;
};

END_NAMESPACE_GAZE_INPUT