// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.CodeGen
{
    abstract class CanonicalizedNode<T> : Graph.Node<T> where T : CanonicalizedNode<T>, new()
    {
        Vertex[] m_canonicalInRefs;

        public CanonicalizedNode()
        {
            Canonical = (T)this;
            NodesInGroup = new T[] { (T)this };
        }

        /// <summary>
        /// The node that is equivalent to this node. Initially set to this.
        /// </summary>
        public T Canonical { get; set; }

        /// <summary>
        /// The nodes that are canonicalized to the canonical node.
        /// </summary>
        public IEnumerable<T> NodesInGroup { get; set; }

        public bool IsCanonical => Canonical == this;

        public IEnumerable<Vertex> CanonicalInRefs
        {
            get
            {
                if (m_canonicalInRefs == null)
                {
                    m_canonicalInRefs =
                        // Get the references from all canonical nodes
                        // that reference all versions of this node.
                        (from n in NodesInGroup
                         from r in n.InReferences
                         where r.Node.IsCanonical
                         orderby r.Position
                         select r).ToArray();
                }
                return m_canonicalInRefs;
            }
        }
    }
}