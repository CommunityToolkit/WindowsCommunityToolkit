//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeTargetItem.g.h"
#include "GazeInput.h"
#include "PointerState.h"

using namespace winrt::Windows::Foundation;
using namespace winrt::Microsoft::UI::Xaml;
using namespace winrt::Microsoft::UI::Xaml::Controls::Primitives;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
	struct GazeTargetItem : GazeTargetItemT<GazeTargetItem>
	{
	public:
		GazeTargetItem(Microsoft::UI::Xaml::UIElement const& target);
		static Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem GetOrCreate(Microsoft::UI::Xaml::UIElement const& element);
		static Microsoft::UI::Xaml::DependencyProperty GazeTargetItemProperty();
		Windows::Foundation::TimeSpan DetailedTime();
		void DetailedTime(Windows::Foundation::TimeSpan const& value);
		Windows::Foundation::TimeSpan OverflowTime();
		void OverflowTime(Windows::Foundation::TimeSpan const& value);
		Windows::Foundation::TimeSpan ElapsedTime();
		Windows::Foundation::TimeSpan NextStateTime();
		void NextStateTime(Windows::Foundation::TimeSpan const& value);
		Windows::Foundation::TimeSpan LastTimestamp();
		void LastTimestamp(Windows::Foundation::TimeSpan const& value);
		Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState ElementState();
		void ElementState(Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState const& value);
		Microsoft::UI::Xaml::UIElement TargetElement();
		void TargetElement(Microsoft::UI::Xaml::UIElement const& value);
		int32_t RepeatCount();
		void RepeatCount(int32_t value);
		int32_t MaxDwellRepeatCount();
		void MaxDwellRepeatCount(int32_t value);
		virtual void Invoke();
		virtual bool IsInvokable();
		void Reset(Windows::Foundation::TimeSpan const& nextStateTime);
		void GiveFeedback();

	private:

		TimeSpan _detailedTime;
		TimeSpan _overflowTime;
		TimeSpan _nextStateTime;
		TimeSpan _lastTimestamp;
		PointerState _elementState{ PointerState::Exit };
		UIElement _targetElement{ nullptr };
		int _repeatCount;
		int _maxDwellRepeatCount;

		void RaiseProgressEvent(DwellProgressState state);

		PointerState _notifiedPointerState{ PointerState::Exit };
		TimeSpan _prevStateTime;
		DwellProgressState _notifiedProgressState{ DwellProgressState::Idle };
		Popup _feedbackPopup{ nullptr };

		static Microsoft::UI::Xaml::DependencyProperty m_gazeTargetItemProperty;
	};
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
	struct GazeTargetItem : GazeTargetItemT<GazeTargetItem, implementation::GazeTargetItem>
	{
	};
}
