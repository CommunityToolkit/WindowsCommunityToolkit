// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// The data structure to define a possible grid layout.
    /// </summary>
    [ContentProperty(Name = "AreaDefinition")]
    public class GridLayoutDefinition
    {
        /// <summary>
        /// Dictionary to store the parsed result of area definition.
        /// </summary>
        /// <remarks>
        /// The string key is <see href="https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/x-name-attribute">x:Name attribute</see>
        /// of child elements of the grid, while the tuple value is the row index, row span value, column index and column span value of that
        /// specific element.
        /// </remarks>
        private IDictionary<string, (int Row, int RowSpan, int Column, int ColumnSpan)> _cellProperties = new Dictionary<string, (int Row, int RowSpan, int Column, int ColumnSpan)>();

        /// <summary>
        /// Gets a list of ColumnDefinition objects defined for this layout.
        /// </summary>
        public List<ColumnDefinition> ColumnDefinitions { get; } = new List<ColumnDefinition>();

        /// <summary>
        /// Gets a list of RowDefinition objects defined for this layout.
        /// </summary>
        public List<RowDefinition> RowDefinitions { get; } = new List<RowDefinition>();

        /// <summary>
        /// Sets area definition for this layout.
        /// </summary>
        /// <remarks>
        /// Aread definition string should list the <see cref="FrameworkElement.Name"/> of Grid elements in row-major order.
        /// Elements in same row should be separated with whitespaces and rows should be separated with semicolons.
        /// Row and column spans can be expressed by repeating element names.
        /// </remarks>
        /// <example>
        /// A simple 2x3 grid layout:
        /// .-----------.
        /// | A | B | C |
        /// |---|---|---|
        /// | D | E | F |
        /// '-----------'
        /// A B C;
        /// D E F;
        ///
        /// A 3x3 grid layout where the first and third rows span 3 columns:
        /// .-----------------------.
        /// |         header        |
        /// |------.--------.-------|
        /// | left | center | right |
        /// |------'--------'-------|
        /// |         footer        |
        /// '-----------------------'
        /// header header header;
        /// left   center right;
        /// footer footer footer;
        ///
        /// A 3x3 grid layout with row span and column span.
        /// .----------------------.
        /// |     |     header     |
        /// | nav |--------.-------|
        /// |     | center | right |
        /// |-----'--------'-------|
        /// |        footer        |
        /// '----------------------'
        /// nav    header header;
        /// nav    center right;
        /// footer footer footer;
        ///
        /// Incorrect usage:
        /// A B C;
        /// D A E;
        /// will result in
        /// .-----------.
        /// |   : B | C |
        /// |...A...|---|
        /// | D :   | E |
        /// '-----------'
        /// Dotted lines are used to illustrate A, B and D children are overlapped.
        /// This usage is not really invalid (it does not throw),
        /// but this API is not expected to be used like this.
        /// </example>
        public string AreaDefinition
        {
            set => ParseAreaDefinition(value);
        }

        internal void Apply(Grid grid)
        {
            ApplyRowColumnDefinitions(grid);
            UpdateChildren(grid);
        }

        /// <summary>
        /// Parse the area definition string, and save it in <see cref="_cellProperties"/>.
        /// </summary>
        /// <param name="str">The area definition string.</param>
        /// <remarks>
        /// The parsing goes as:<br/>
        /// 1. Split the string by semicolon, as the rows separator is semicolon.<br/>
        /// 2. Split every row string by space. <br/>
        /// The (i,j)th value of resulting 2 deminsional array is
        /// <see href="https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/x-name-attribute">x:Name attribute</see>
        /// of the element that should be placed at (i,j)th cell in the grid.<br/>
        /// If name of an element repeats, that element should span over all the cells with its name.
        /// </remarks>
        private void ParseAreaDefinition(string str)
        {
            _cellProperties.Clear();

            // split the area definition by semicolon and then split every row by space.
            var table = str
                .Split(";", StringSplitOptions.RemoveEmptyEntries)
                .Select(row => row.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(cell => cell.Trim()));

            // Since the table is Enumerable of Enumerable rather than list of list, we need to iterated over it via foreach loop.
            // foreach loop doesn't have a current index, but we need current index in the loop body, thus the explicit i and j declaration.
            var i = 0;
            foreach (var tableRow in table)
            {
                var j = 0;
                foreach (var childName in tableRow)
                {
                    if (_cellProperties.TryGetValue(childName, out var properties))
                    {
                        // if childName already exists in _cellProperties, it means the element name is repeating in the area definition.
                        // we should keep row index and column index, while setting row span and column span.
                        // The number of cells to span over is (currentIndex - startingIndex + 1).
                        var (row, _, column, _) = properties;
                        _cellProperties[childName] = (Row: row, RowSpan: i - row + 1, Column: column, ColumnSpan: j - column + 1);
                    }
                    else
                    {
                        // otherwise, the element should be placed at (i, j)th cell and span over 1 row, 1 column.
                        _cellProperties[childName] = (Row: i, RowSpan: 1, Column: j, ColumnSpan: 1);
                    }

                    ++j;
                }

                ++i;
            }
        }

        private void ApplyRowColumnDefinitions(Grid grid)
        {
            grid.ColumnDefinitions.Clear();
            foreach (var def in ColumnDefinitions)
            {
                grid.ColumnDefinitions.Add(def);
            }

            grid.RowDefinitions.Clear();
            foreach (var def in RowDefinitions)
            {
                grid.RowDefinitions.Add(def);
            }
        }

        private void UpdateChildren(Grid grid)
        {
            foreach (var child in grid.Children)
            {
                if (child is FrameworkElement element && _cellProperties.TryGetValue(element.Name, out var property))
                {
                    child.SetValue(Grid.ColumnProperty, property.Column);
                    child.SetValue(Grid.RowProperty, property.Row);
                    child.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
                    child.SetValue(Grid.RowSpanProperty, property.RowSpan);
                }
            }
        }
    }
}
