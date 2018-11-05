// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CompositionPropertySet : CompositionObject
    {
        readonly Dictionary<string, float> _scalarProperties = new Dictionary<string, float>();
        readonly Dictionary<string, Vector2> _vector2Properties = new Dictionary<string, Vector2>();

        internal CompositionPropertySet(CompositionObject owner) 
        {
            Owner = owner;
        }

        public CompositionObject Owner { get; }
        public void InsertScalar(string name, float value) => _scalarProperties.Add(name, value);

        public void InsertVector2(string name, Vector2 value) => _vector2Properties.Add(name, value);

        public IEnumerable<KeyValuePair<string, float>> ScalarProperties => _scalarProperties;

        public IEnumerable<KeyValuePair<string, Vector2>> Vector2Properties => _vector2Properties;

        internal IEnumerable<string> PropertyNames => _scalarProperties.Keys.Concat(_vector2Properties.Keys);

        internal bool IsEmpty => !PropertyNames.Any();

        public override CompositionObjectType Type => CompositionObjectType.CompositionPropertySet;
    }
}
