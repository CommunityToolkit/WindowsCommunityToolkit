// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI;
using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Hosting;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;

namespace UnitTests.UWP.UI.Animations
{
    [TestClass]
    [TestCategory("Test_ExpressionNode")]
    public class Test_ExpressionNode : VisualUITestBase
    {
        [TestMethod]
        public async Task EvaluateExpressionNode()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                Grid grid = new();
                await SetTestContentAsync(grid);

                var visual = ElementCompositionPreview.GetElementVisual(grid);

                // Test basic vector algebra
                visual.Offset = Vector3.UnitX;
                var vector3AlgebraNode = (-visual.GetReference().Offset
                                    + Vector3.UnitY
                                     - Vector3.UnitZ) * 5;

                Assert.AreEqual(new Vector3(-5, 5, -5), vector3AlgebraNode.Evaluate());

                // Test swizzle operation
                visual.CenterPoint = Vector3.UnitY;
                var swizzleNode = visual.GetReference().CenterPoint.GetSubchannels(
                    Vector3Node.Subchannel.Y,
                    Vector3Node.Subchannel.X,
                    Vector3Node.Subchannel.Y);

                Assert.AreEqual(new Vector3(1, 0, 1), swizzleNode.Evaluate());

                // Test retrieving properties from set
                var propertySet = visual.Compositor.CreatePropertySet();
                var propertyName = "test";
                var propertyNode = propertySet.GetReference().GetVector4Property(propertyName);

                propertySet.InsertVector4(propertyName, Vector4.UnitW);
                Assert.AreEqual(Vector4.UnitW, propertyNode.Evaluate());

                propertySet.InsertVector4(propertyName, Vector4.UnitX);
                Assert.AreEqual(Vector4.UnitX, propertyNode.Evaluate());
            });
        }
    }
}