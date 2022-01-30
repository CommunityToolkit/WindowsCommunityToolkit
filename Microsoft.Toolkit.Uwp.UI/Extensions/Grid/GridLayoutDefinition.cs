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
        private IDictionary<string, (int Row, int RowSpan, int Column, int ColumnSpan)> _cellProperties;

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
            set => _cellProperties = ParseAreaDefinition(value);
        }

        internal void Apply(Grid grid)
        {
            ApplyRowColumnDefinitions(grid);
            UpdateChildren(grid);
        }

        private static IDictionary<string, (int, int, int, int)> ParseAreaDefinition(string str)
        {
            var def = new Dictionary<string, (int Row, int RowSpan, int Column, int ColumnSpan)>();
            var table = str
                .Split(";", StringSplitOptions.RemoveEmptyEntries)
                .Select(row => row.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(cell => cell.Trim()).ToList())
                .ToList();
            for (var i = 0; i < table.Count; ++i)
            {
                var tableRow = table[i];
                for (var j = 0; j < tableRow.Count; ++j)
                {
                    var childName = tableRow[j];
                    if (def.TryGetValue(childName, out var properties))
                    {
                        var (row, _, column, _) = properties;
                        def[childName] = (Row: row, RowSpan: i - row + 1, Column: column, ColumnSpan: j - column + 1);
                    }
                    else
                    {
                        def[childName] = (Row: i, RowSpan: 1, Column: j, ColumnSpan: 1);
                    }
                }
            }

            return def;
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
