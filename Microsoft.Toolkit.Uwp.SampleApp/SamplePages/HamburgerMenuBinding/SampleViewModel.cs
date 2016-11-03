﻿using Microsoft.Toolkit.Uwp.SampleApp.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.HamburgerMenuBinding
{
    public class SampleViewModel : BindableBase
    {
        public ObservableCollection<SampleMenuItem> MenuItems { get; private set; }
        public ObservableCollection<SampleMenuItem> OptionItems { get; private set; }

        public SampleViewModel()
        {
            MenuItems = new ObservableCollection<SampleMenuItem>();
            OptionItems = new ObservableCollection<SampleMenuItem>();

            MenuItems.Add(new SampleMenuItem() { Label = "Big four summer heat", ImagePath = "/Assets/Photos/BigFourSummerHeat.png" });
            MenuItems.Add(new SampleMenuItem() { Label = "Bison badlands Chillin", ImagePath = "/Assets/Photos/BisonBadlandsChillin.png" });
            MenuItems.Add(new SampleMenuItem() { Label = "Giant slab in Oregon", ImagePath = "/Assets/Photos/GiantSlabInOregon.png" });
            MenuItems.Add(new SampleMenuItem() { Label = "Lake Ann Mushroom", ImagePath = "/Assets/Photos/LakeAnnMushroom.png" });

            OptionItems.Add(new SampleMenuItem { Label = "Settings", SymbolAsChar = '\uE13D' });
        }
    }
}