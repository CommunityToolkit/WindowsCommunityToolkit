using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        // Gets Column or Row definition at index from grid based on resize direction
        private static DependencyObject GetGridDefinition(Grid grid, int index, GridResizeDirection direction)
        {
            return direction == GridResizeDirection.Columns ? (DependencyObject)grid.ColumnDefinitions[index] : (DependencyObject)grid.RowDefinitions[index];
        }

        // Gets Column or Row definition at index from grid based on resize direction
        private static void SetDefinitionLength(DependencyObject definition, GridLength length)
        {
            definition.SetValue(definition is ColumnDefinition ? ColumnDefinition.WidthProperty : RowDefinition.HeightProperty, length);
        }

        private void UpdateDisplayIcon()
        {
            if (_iconDisplay == null)
            {
                return;
            }

            if (_resizeData.ResizeDirection == GridResizeDirection.Columns)
            {
                _iconDisplay.Text = GripperBarVertical;
            }
            else if (_resizeData.ResizeDirection == GridResizeDirection.Rows)
            {
                _iconDisplay.Text = GripperBarHorizontal;
            }
        }

        private GridResizeDirection GetEffectiveResizeDirection()
        {
            GridResizeDirection direction = ResizeDirection;

            if (direction == GridResizeDirection.Auto)
            {
                // When HorizontalAlignment is Left, Right or Center, resize Columns
                if (HorizontalAlignment != HorizontalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Columns;
                }
                else if (VerticalAlignment != VerticalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Rows;
                }
                else if (ActualWidth <= ActualHeight)// Fall back to Width vs Height
                {
                    direction = GridResizeDirection.Columns;
                }
                else
                {
                    direction = GridResizeDirection.Rows;
                }
            }

            return direction;
        }

        // Convert BasedOnAlignment to Next/Prev/Both depending on alignment and Direction
        private GridResizeBehavior GetEffectiveResizeBehavior(GridResizeDirection direction)
        {
            GridResizeBehavior resizeBehavior = ResizeBehavior;

            if (resizeBehavior == GridResizeBehavior.BasedOnAlignment)
            {
                if (direction == GridResizeDirection.Columns)
                {
                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            resizeBehavior = GridResizeBehavior.PreviousAndCurrent;
                            break;
                        case HorizontalAlignment.Right:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.PreviousAndNext;
                            break;
                    }
                }
                else
                {
                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            resizeBehavior = GridResizeBehavior.PreviousAndCurrent;
                            break;
                        case VerticalAlignment.Bottom:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.PreviousAndNext;
                            break;
                    }
                }
            }

            return resizeBehavior;
        }


        #region Helper Methods

        #region Row/Column Abstractions 
        // These methods are to help abstract dealing with rows and columns.  
        // DefinitionBase already has internal helpers for getting Width/Height, MinWidth/MinHeight, and MaxWidth/MaxHeight

        // Returns true if the row/column has a Star length
        private bool IsStar(DependencyObject definition)
        {
            return ((GridLength)definition.GetValue(
                _resizeData.ResizeDirection == GridResizeDirection.Columns
                    ? ColumnDefinition.WidthProperty
                    : RowDefinition.HeightProperty)).IsStar;
        }

       

        // Retrieves the ActualWidth or ActualHeight of the definition depending on its type Column or Row
        private double GetActualLength(DependencyObject definition)
        {
            ColumnDefinition column = definition as ColumnDefinition;

            return column == null ? ((RowDefinition)definition).ActualHeight : column.ActualWidth;
        }

        

        #endregion

        // Get the minimum and maximum Delta can be given definition constraints (MinWidth/MaxWidth)
        private void GetDeltaConstraints(out double minDelta, out double maxDelta)
        {
            double definition1Len = GetActualLength(_resizeData.Definition1);
            var isColumn = _resizeData.ResizeDirection == GridResizeDirection.Columns;
            double definition1Min = (double)_resizeData.Definition1.GetValue(
                        isColumn ?
                        ColumnDefinition.MinWidthProperty :
                        RowDefinition.MinHeightProperty);
            double definition1Max = (double)_resizeData.Definition1.GetValue(
                        isColumn ?
                        ColumnDefinition.MaxWidthProperty :
                        RowDefinition.MaxHeightProperty);

            double definition2Len = GetActualLength(_resizeData.Definition2);
            double definition2Min = (double)_resizeData.Definition2.GetValue(
                        isColumn ?
                        ColumnDefinition.MinWidthProperty :
                        RowDefinition.MinHeightProperty);
            double definition2Max = (double)_resizeData.Definition2.GetValue(
                        isColumn ?
                        ColumnDefinition.MaxWidthProperty :
                        RowDefinition.MaxHeightProperty);

            //Set MinWidths to be greater than width of splitter
            if (_resizeData.SplitterIndex == _resizeData.Definition1Index)
            {
                definition1Min = Math.Max(definition1Min, _resizeData.SplitterLength);
            }
            else if (_resizeData.SplitterIndex == _resizeData.Definition2Index)
            {
                definition2Min = Math.Max(definition2Min, _resizeData.SplitterLength);
            }

            if (_resizeData.SplitBehavior == SplitBehavior.Split)
            {
                // Determine the minimum and maximum the columns can be resized
                minDelta = -Math.Min(definition1Len - definition1Min, definition2Max - definition2Len);
                maxDelta = Math.Min(definition1Max - definition1Len, definition2Len - definition2Min);
            }
            else if (_resizeData.SplitBehavior == SplitBehavior.Resize1)
            {
                minDelta = definition1Min - definition1Len;
                maxDelta = definition1Max - definition1Len;
            }
            else
            {
                minDelta = definition2Len - definition2Max;
                maxDelta = definition2Len - definition2Min;
            }
        }

        //Sets the length of definition1 and definition2 
        private void SetLengths(double definition1Pixels, double definition2Pixels)
        {
            // For the case where both definition1 and 2 are stars, update all star values to match their current pixel values
            if (_resizeData.SplitBehavior == SplitBehavior.Split)
            {
                IEnumerable definitions = _resizeData.ResizeDirection == GridResizeDirection.Columns ? (IEnumerable)_resizeData.Grid.ColumnDefinitions : (IEnumerable)_resizeData.Grid.RowDefinitions;

                int i = 0;
                foreach (DependencyObject definition in definitions)
                {
                    // For each definition, if it is a star, set is value to ActualLength in stars
                    // This makes 1 star == 1 pixel in length
                    if (i == _resizeData.Definition1Index)
                    {
                        SetDefinitionLength(definition, new GridLength(definition1Pixels, GridUnitType.Star));
                    }
                    else if (i == _resizeData.Definition2Index)
                    {
                        SetDefinitionLength(definition, new GridLength(definition2Pixels, GridUnitType.Star));
                    }
                    else if (IsStar(definition))
                    {
                        SetDefinitionLength(definition, new GridLength(GetActualLength(definition), GridUnitType.Star));
                    }

                    i++;
                }
            }
            else if (_resizeData.SplitBehavior == SplitBehavior.Resize1)
            {
                SetDefinitionLength(_resizeData.Definition1, new GridLength(definition1Pixels));
            }
            else
            {
                SetDefinitionLength(_resizeData.Definition2, new GridLength(definition2Pixels));
            }
        }

        // Move the splitter by the given Delta's in the horizontal and vertical directions
        private void MoveSplitter(double horizontalChange, double verticalChange)
        {
            if (_resizeData == null)
            {
                InitializeData();
            }

            double delta;

            // Calculate the offset to adjust the splitter.  If layout rounding is enabled, we
            // need to round to an integer physical pixel value to avoid round-ups of children that
            // expand the bounds of the Grid.  In practice this only happens in high dpi because
            // horizontal/vertical offsets here are never fractional (they correspond to mouse movement
            // across logical pixels).  Rounding error only creeps in when converting to a physical
            // display with something other than the logical 96 dpi.
            if (_resizeData.ResizeDirection == GridResizeDirection.Columns)
            {
                delta = horizontalChange;
            }
            else
            {
                delta = verticalChange;
            }

            DependencyObject definition1 = _resizeData.Definition1;
            DependencyObject definition2 = _resizeData.Definition2;
            if (definition1 != null && definition2 != null)
            {
                double actualLength1 = GetActualLength(definition1);
                double actualLength2 = GetActualLength(definition2);



                double min, max;
                GetDeltaConstraints(out min, out max);

                // Flip when the splitter's flow direction isn't the same as the grid's
                if (FlowDirection != _resizeData.Grid.FlowDirection)
                    delta = -delta;

                // Constrain Delta to Min/MaxWidth of columns
                delta = Math.Min(Math.Max(delta, min), max);

                // With floating point operations there may be loss of precision to some degree. Eg. Adding a very 
                // small value to a very large one might result in the small value being ignored. In the following 
                // steps there are two floating point operations viz. actualLength1+delta and actualLength2-delta. 
                // It is possible that the addition resulted in loss of precision and the delta value was ignored, whereas 
                // the subtraction actual absorbed the delta value. This now means that 
                // (definition1LengthNew + definition2LengthNewis) 2 factors of precision away from 
                // (actualLength1 + actualLength2). This can cause a problem in the subsequent drag iteration where 
                // this will be interpreted as the cancellation of the resize operation. To avoid this imprecision we use 
                // make definition2LengthNew be a function of definition1LengthNew so that the precision or the loss 
                // thereof can be counterbalanced. See DevDiv 

                double definition1LengthNew = actualLength1 + delta;
                double definition2LengthNew = actualLength1 + actualLength2 - definition1LengthNew;

                SetLengths(definition1LengthNew, definition2LengthNew);
            }

        }

        private void InitializeData()
        {
            if (Resizable != null)
            {
                // Setup data used for resizing
                _resizeData = new ResizeData();
                _resizeData.Grid = Resizable;
                _resizeData.ResizeDirection = GetEffectiveResizeDirection();
                _resizeData.ResizeBehavior = GetEffectiveResizeBehavior(_resizeData.ResizeDirection);
                _resizeData.SplitterLength = Math.Min(ActualWidth, ActualHeight);

                // Store the rows and columns to resize on drag events
                if (!SetupDefinitionsToResize())
                {
                    // Unable to resize, clear data
                    _resizeData = null;
                }
            }
        }

        private bool SetupDefinitionsToResize()
        {
            var gridSpan = (int)GetValue(_resizeData.ResizeDirection == GridResizeDirection.Columns ? Grid.ColumnSpanProperty : Grid.RowSpanProperty);

            if (gridSpan == 1)
            {
                var splitterIndex = (int)GetValue(_resizeData.ResizeDirection == GridResizeDirection.Columns ? Grid.ColumnProperty : Grid.RowProperty);

                // Select the columns based on Behavior
                int index1;
                int index2;
                switch (_resizeData.ResizeBehavior)
                {
                    case GridResizeBehavior.PreviousAndCurrent:
                        // get current and previous
                        index1 = splitterIndex - 1;
                        index2 = splitterIndex;
                        break;
                    case GridResizeBehavior.CurrentAndNext:
                        // get current and next
                        index1 = splitterIndex;
                        index2 = splitterIndex + 1;
                        break;
                    default: // GridResizeBehavior.PreviousAndNext
                        // get previous and next
                        index1 = splitterIndex - 1;
                        index2 = splitterIndex + 1;
                        break;
                }

                // Get # of rows/columns in the resize direction
                var count = (_resizeData.ResizeDirection == GridResizeDirection.Columns) ? _resizeData.Grid.ColumnDefinitions.Count : _resizeData.Grid.RowDefinitions.Count;

                if (index1 >= 0 && index2 < count)
                {
                    _resizeData.SplitterIndex = splitterIndex;
                    _resizeData.Definition1Index = index1;
                    _resizeData.Definition1 = GetGridDefinition(_resizeData.Grid, index1, _resizeData.ResizeDirection);
                    _resizeData.Definition2Index = index2;
                    _resizeData.Definition2 = GetGridDefinition(_resizeData.Grid, index2, _resizeData.ResizeDirection);

                    // Determine how to resize the columns
                    bool isStar1 = IsStar(_resizeData.Definition1);
                    bool isStar2 = IsStar(_resizeData.Definition2);
                    if (isStar1 && isStar2)
                    {
                        // If they are both stars, resize both
                        _resizeData.SplitBehavior = SplitBehavior.Split;
                    }
                    else
                    {
                        // One column is fixed width, resize the first one that is fixed
                        _resizeData.SplitBehavior = !isStar1 ? SplitBehavior.Resize1 : SplitBehavior.Resize2;
                    }

                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
