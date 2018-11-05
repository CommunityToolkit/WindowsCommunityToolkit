// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools
{

    /// <summary>
    /// Serializes a <see cref="CompositionObject"/> graph into DGML format.
    /// </summary>
    sealed class CompositionObjectDgmlSerializer
    {
        static readonly XNamespace ns = "http://schemas.microsoft.com/vs/2009/dgml";
        ObjectGraph<ObjectData> _objectGraph;
        int _idGenerator;
        int _groupIdGenerator;

        CompositionObjectDgmlSerializer() { }

        public static XDocument ToXml(CompositionObject compositionObject)
        {
            return new CompositionObjectDgmlSerializer().ToXDocument(compositionObject);
        }

        XDocument ToXDocument(CompositionObject compositionObject)
        {
            // Build the graph of objects.
            _objectGraph = ObjectGraph<ObjectData>.FromCompositionObject(compositionObject, includeVertices: true);

            // Give names to each object.
            foreach ((var node, var name) in CodeGen.NodeNamer<ObjectData>.GenerateNodeNames(_objectGraph.Nodes))
            {
                node.Name = name;
            }

            // Initialize each node. 
            foreach (var n in _objectGraph.Nodes)
            {
                n.Initialize(this);
            }

            // Second stage initialization - relies on all nodes having had the first stage of initialization.
            foreach (var n in _objectGraph.Nodes)
            {
                n.Initialize2();
            }

            var rootNode = _objectGraph[compositionObject];

            // Give the root object a special name and color.
            rootNode.Name = $"{rootNode.Name} (Root)";
            rootNode.Category = "Root";

            // Get the groups.
            var groups = GroupTree(rootNode, null).ToArray();

            // Create the DGML nodes.
            var nodes =
                from n in _objectGraph.Nodes
                where n.IsDgmlNode
                select CreateNodeXml(id: n.Id, label: n.Name, category: n.Category);

            // Create the DGML nodes for the groups.
            nodes = nodes.Concat(
                from gn in groups
                select CreateNodeXml(id: gn.Id, label: gn.GroupName, @group: "Expanded"));

            // Create the links between the nodes.
            var links =
                from n in _objectGraph.Nodes
                where n.IsDgmlNode
                from otherNode in n.Children
                select new XElement(ns + "Link", new XAttribute("Source", n.Id), new XAttribute("Target", otherNode.Id));

            // Create the "contains" links for the nodes contained in groups.
            var containsLinks =
                (from g in groups
                 from member in g.ItemsInGroup
                 select new XElement(ns + "Link", new XAttribute("Source", g.Id), new XAttribute("Target", member.Id), new XAttribute("Category", "Contains"))).ToArray();

            // Create the "contains" links for the groups contained in groups
            var groupContainsGroupsLinks =
                (from g in groups
                 from member in g.GroupsInGroup
                 select new XElement(ns + "Link", new XAttribute("Source", g.Id), new XAttribute("Target", member.Id), new XAttribute("Category", "Contains"))).ToArray();

            containsLinks = containsLinks.Concat(groupContainsGroupsLinks).ToArray();

            // Create the XML
            return new XDocument(new XElement(ns + "DirectedGraph",
                new XElement(ns + "Nodes", nodes),
                new XElement(ns + "Links", links.Concat(containsLinks)),
                new XElement(ns + "Categories",
                    new XElement(ns + "Category",
                                new XAttribute("Id", "Contains"),
                                new XAttribute("Label", "Contains"),
                                new XAttribute("Description", "Whether the source of the link contains the target object"),
                                new XAttribute("CanBeDataDriven", "False"),
                                new XAttribute("CanLinkedNodesBeDataDriven", "True"),
                                new XAttribute("IncomingActionLabel", "Contained By"),
                                new XAttribute("IsContainment", "True"),
                                new XAttribute("OutgoingActionLabel", "Contains")),
                    new XElement(ns + "Category",
                        new XAttribute("Id", "ShapeVisual"),
                        new XAttribute("Label", "ShapeVisual"),
                        new XAttribute("Background", "#FF44CCCC"),
                        new XAttribute("IsTag", "True")),
                    new XElement(ns + "Category",
                        new XAttribute("Id", "Root"),
                        new XAttribute("Label", "Root"),
                        new XAttribute("Background", "#FFDF0174"),
                        new XAttribute("IsTag", "True")),
                    new XElement(ns + "Category",
                        new XAttribute("Id", "ContainerShape"),
                        new XAttribute("Label", "ContainerShape"),
                        new XAttribute("Background", "#FF44CCCC"),
                        new XAttribute("IsTag", "True")),
                    new XElement(ns + "Category",
                        new XAttribute("Id", "Shape"),
                        new XAttribute("Label", "Shape"),
                        new XAttribute("Background", "#FFF7FE2E"),
                        new XAttribute("IsTag", "True"))
                        ),
                new XElement(ns + "Properties",
                    CreatePropertyXml(id: "Bounds", dataType: "System.Windows.Rect"),
                    CreatePropertyXml(id: "CanBeDataDriven", label: "CanBeDataDriven", description: "CanBeDataDriven", dataType: "System.Boolean"),
                    CreatePropertyXml(id: "CanLinkedNodesBeDataDriven", label: "CanLinkedNodesBeDataDriven", description: "CanLinkedNodesBeDataDriven", dataType: "System.Boolean"),
                    CreatePropertyXml(id: "Group", label: "Group", description: "Display the node as a group", dataType: "Microsoft.VisualStudio.GraphModel.GraphGroupStyle"),
                    CreatePropertyXml(id: "IncomingActionLabel", label: "IncomingActionLabel", description: "IncomingActionLabel", dataType: "System.String"),
                    CreatePropertyXml(id: "IsContainment", dataType: "System.Boolean"),
                    CreatePropertyXml(id: "Label", label: "Label", description: "Displayable label of an Annotatable object", dataType: "System.String"),
                    CreatePropertyXml(id: "Layout", dataType: "System.String"),
                    CreatePropertyXml(id: "OutgoingActionLabel", label: "OutgoingActionLabel", description: "OutgoingActionLabel", dataType: "System.String"),
                    CreatePropertyXml(id: "UseManualLocation", dataType: "System.Boolean"),
                    CreatePropertyXml(id: "ZoomLevel", dataType: "System.String"))));
        }

        static XElement CreatePropertyXml(string id, string label = null, string description = null, string dataType = null)
        {
            return new XElement(ns + "Property", CreateAttributes(new[] { ("Id", id), ("Label", label), ("Description", description), ("DataType", dataType) }));
        }


        static XElement CreateNodeXml(string id, string label = null, string name = null, string category = null, string @group = null)
        {
            return new XElement(ns + "Node", CreateAttributes(new[] { ("Id", id), ("Label", label), ("Category", category), ("Group", @group) }));
        }

        static IEnumerable<XAttribute> CreateAttributes(IEnumerable<(string Name, string Value)> attrs)
        {
            foreach (var (Name, Value) in attrs)
            {
                if (Value != null)
                {
                    yield return new XAttribute(Name, Value);
                }
            }
        }


        IEnumerable<GroupNode> GroupTree(ObjectData node, GroupNode group)
        {
            if (group != null)
            {
                group.ItemsInGroup.Add(node);
            }

            var childLinks = node.Children.ToArray();

            foreach (var child in childLinks)
            {
                GroupNode childGroup;
                var childObject = child.Object as CompositionObject;
                var childDescription = (childObject != null && !string.IsNullOrWhiteSpace(childObject.ShortDescription))
                    ? childObject.ShortDescription
                    : "";

                // Start a new group for the child if:
                //   a) There is more than one child and the child is not a leaf
                //  or
                //   b) The child has a ShortDescription starting with "Layer: "
                if ((childLinks.Length > 1 && child.Children.Any()) || childDescription.StartsWith("Layer "))
                {
                    childGroup = new GroupNode(this)
                    {
                        Id = GenerateGroupId(),
                        GroupName = childDescription,
                    };

                    if (group != null)
                    {
                        group.GroupsInGroup.Add(childGroup);
                    }
                    yield return childGroup;
                }
                else
                {
                    childGroup = group;
                }

                // Recurse to group the subtree.
                foreach (var groupNode in GroupTree(child, childGroup))
                {
                    yield return groupNode;
                }
            }
        }

        string GenerateId() => $"id{_idGenerator++}";
        string GenerateGroupId() => $"gid{_groupIdGenerator++}";


        sealed class ObjectData : Graph.Node<ObjectData>
        {
            CompositionObjectDgmlSerializer _owner;
            ObjectData _parent;
            readonly List<ObjectData> _children = new List<ObjectData>();

            internal string Name { get; set; }
            internal string Category { get; set; }
            internal bool IsDgmlNode { get; private set; }
            internal string Id { get; private set; }

            // The links from this node to its children.
            internal IEnumerable<ObjectData> Children => _children;

            // Called after the graph has been created. Do things here that depend on other nodes
            // in the graph.
            internal void Initialize(CompositionObjectDgmlSerializer owner)
            {
                _owner = owner;
                var obj = Object as CompositionObject;
                if (obj != null)
                {
                    switch (obj.Type)
                    {
                        case CompositionObjectType.AnimationController:
                        case CompositionObjectType.ColorKeyFrameAnimation:
                        case CompositionObjectType.CompositionColorBrush:
                        case CompositionObjectType.CompositionEllipseGeometry:
                        case CompositionObjectType.CompositionGeometricClip:
                        case CompositionObjectType.CompositionPathGeometry:
                        case CompositionObjectType.CompositionPropertySet:
                        case CompositionObjectType.CompositionRectangleGeometry:
                        case CompositionObjectType.CompositionRoundedRectangleGeometry:
                        case CompositionObjectType.CompositionViewBox:
                        case CompositionObjectType.CubicBezierEasingFunction:
                        case CompositionObjectType.ExpressionAnimation:
                        case CompositionObjectType.InsetClip:
                        case CompositionObjectType.LinearEasingFunction:
                        case CompositionObjectType.PathKeyFrameAnimation:
                        case CompositionObjectType.ScalarKeyFrameAnimation:
                        case CompositionObjectType.StepEasingFunction:
                        case CompositionObjectType.Vector2KeyFrameAnimation:
                        case CompositionObjectType.Vector3KeyFrameAnimation:
                            return;
                        case CompositionObjectType.CompositionContainerShape:
                            Category = "ContainerShape";
                            break;
                        case CompositionObjectType.CompositionSpriteShape:
                            {
                                Category = "Shape";
                            }
                            break;
                        case CompositionObjectType.ContainerVisual:
                            break;
                        case CompositionObjectType.ShapeVisual:
                            Category = "ShapeVisual";
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                    IsDgmlNode = true;
                    Id = _owner.GenerateId();
                }
            }

            internal void Initialize2()
            {

                // Create a link to the parent and from the parent to the child.
                if (IsDgmlNode)
                {
                    _parent = InReferences.Select(v => v.Node).Where(n => n.IsDgmlNode).FirstOrDefault();

                    if (_parent != null)
                    {
                        _parent._children.Add(this);
                    }

                }
            }


            public override string ToString() => Id;
        }

        sealed class GroupNode
        {
            readonly CompositionObjectDgmlSerializer _owner;
            internal GroupNode(CompositionObjectDgmlSerializer owner)
            {
                _owner = owner;
            }

            internal HashSet<ObjectData> ItemsInGroup = new HashSet<ObjectData>();
            internal List<GroupNode> GroupsInGroup = new List<GroupNode>();
            internal string Id;
            internal string GroupName;
            public override string ToString() => Id;
        }
    }
}
