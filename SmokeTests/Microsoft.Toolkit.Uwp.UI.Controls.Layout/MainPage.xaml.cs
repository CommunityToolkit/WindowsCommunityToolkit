// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using Windows.UI;

namespace SmokeTest
{
    public sealed partial class MainPage
    {
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

        private readonly Random _random;

        public class Item
        {
            public int Index { get; internal set; }

            public int Width { get; internal set; }

            public int Height { get; internal set; }

            public Color Color { get; internal set; }
        }

        public MainPage()
        {
            InitializeComponent();

            _random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < _random.Next(1000, 5000); i++)
            {
                var item = new Item { Index = i, Width = _random.Next(50, 250), Height = _random.Next(50, 250), Color = Color.FromArgb(255, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255)) };
                Items.Add(item);
            }
        }
    }
}
