// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

using Microsoft.VisualStudio.DesignTools.Extensibility;
using Microsoft.VisualStudio.DesignTools.Extensibility.Features;
using Microsoft.VisualStudio.DesignTools.Extensibility.Metadata;
using Microsoft.VisualStudio.DesignTools.Extensibility.Model;

namespace CommunityToolkit.WinUI.UI.Controls.Design
{
    internal class RadialGaugeDefaults : DefaultInitializer
    {
        public override void InitializeDefaults(ModelItem item)
        {
            item.Properties[nameof(RadialGauge.Value)].SetValue(0d);
        }
    }

    internal class RadialGaugeMetadata : AttributeTableBuilder
    {
        public RadialGaugeMetadata()
            : base()
        {
            AddCallback(ControlTypes.RadialGauge,
                b =>
                {
                    b.AddCustomAttributes(new FeatureAttribute(typeof(RadialGaugeDefaults)));
                    b.AddCustomAttributes(nameof(RadialGauge.Minimum), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RadialGauge.Maximum), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RadialGauge.StepSize), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RadialGauge.IsInteractive), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RadialGauge.ScaleWidth), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.NeedleBrush), new CategoryAttribute(Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(RadialGauge.Value), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RadialGauge.Unit), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RadialGauge.TrailBrush), new CategoryAttribute(Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(RadialGauge.ScaleBrush), new CategoryAttribute(Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(RadialGauge.TickBrush), new CategoryAttribute(Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(RadialGauge.ValueStringFormat), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RadialGauge.TickSpacing), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.NeedleLength), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.NeedleWidth), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.ScalePadding), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.ScaleTickWidth), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.TickWidth), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.TickLength), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.MinAngle), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialGauge.MaxAngle), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
        }
    }
}