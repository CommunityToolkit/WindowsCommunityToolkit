//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeFilter.h"

using namespace winrt::Windows::Foundation;
using namespace winrt::Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

GazeFilterArgs NullFilter::Update(GazeFilterArgs args)
{
	return args;
}

void NullFilter::LoadSettings(ValueSet settings)
{
}

END_NAMESPACE_GAZE_INPUT