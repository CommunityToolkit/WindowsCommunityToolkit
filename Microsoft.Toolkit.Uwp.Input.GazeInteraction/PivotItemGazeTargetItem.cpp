#include "pch.h"
#include "PivotItemGazeTargetItem.h"
#include "PivotItemGazeTargetItem.g.cpp"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    void PivotItemGazeTargetItem::Invoke()
    {
        PivotHeaderItem headerItem{ TargetElement().try_as<PivotHeaderItem>() };
        PivotHeaderPanel headerPanel{ VisualTreeHelper::GetParent(headerItem).try_as<PivotHeaderPanel>() };
        unsigned index;
        headerPanel.Children().IndexOf(headerItem, index);
        
        DependencyObject walker = headerPanel;
        Pivot pivot;
        do
        {
            walker = VisualTreeHelper::GetParent(walker);
            pivot = walker.try_as<Pivot>();
        } while (pivot == nullptr);
        
        pivot.SelectedIndex(index);
    }
}
