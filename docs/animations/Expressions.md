---
title: ExpressionBuilder
author: nmetulev
description: The ExpressionBuilder classes are a C#-only alternative to building Composition Expressions with type safety.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, expressions, expressionbuilder
---

# ExpressionBuilder

- [Quick Start](#quick-start)
    - [Setting up the ExpressionBuilder classes with your app](#setting-up-the-expressionbuilder-classes-with-your-app)
    - [Getting started with ExpressionBuilder classes](#getting-started-with-expressionbuilder-classes)
        - [ExpressionAnimation Overview](#expressionAnimation-overview)
    - [Using the ExpressionBuilder classes](#using-the-expressionbuilder-classes)
        - [Extension Methods](#extension-methods)
        - [How to template with ExpressionBuilder](#how-to-template-with-expressionbuilder)
        - [E2E Example](#e2e-example)
        - [Things to Note](#things-to-note)
- [Intro](#intro)
    - [What are Expressions?](#what-are-expressions)
    - [Why ExpressionBuilder?](#why-expressionbuilder)
- [How to: Build Core Components of Expressions](#how-to-build-core-components-of-expressions)
    - [General Construction](#general-construction)
        - [Interacting with ExpressionNodes via Static Methods](#interacting-with-expressionnodes-via-static-methods)
        - [Implicit Conversion to ExpressionNodes](#implicit-conversion-to-expressionnodes)
        - [Using ExpressionNodes with StartAnimation](#using-expressionnodes-with-startanimation)
        - [Using ExpressionNodes with ExpressionKeyFrames](#using-expressionnodes-with-expressionkeyframes)
        - [Using ExpressionNodes in other places](#using-expressionnodes-in-other-places)
    - [Parameters](#parameters)
        - [Definitions: Constants vs. References](#definitions-constants-vs-references)
        - [Definitions: Dynamic vs. Static Parameters](#definitions-dynamic-vs-static-parameters)
        - [Creating Constant Parameters](#creating-constant-parameters)
        - [Creating Reference Parameters](#creating-reference-parameters)
        - [Subchanneling (Swizzling)](#subchanneling-swizzling)
        - [Templating](#templating)
        - [Keywords](#expressions-keywords)
    - [Math shortcuts & basic operators](#math-shortcuts-basic-operators)
        - [Basic Operators](#basic-operators)
        - [Math Shortcuts (Functions)](#math-shortcuts-functions)
    - [Advanced Operations](#advanced-operations)
        - [Comparison Operators](#comparison-operators)
        - [Conditional Operation](#conditional-operation)
    - [Tips and Tricks for using Classes](#tips-and-tricks-for-using-classes)
        - [Shortening Class Names](#shortening-class-names)
- [Translating Old World to New](#translating-old-world-to-new)
    - [Creating an Expression](#creating-an-expression)
    - [Defining Constant Parameters](#defining-constant-parameters)
    - [Building Constants](#building-constants)
    - [Defining Reference Parameters](#defining-reference-parameters)
    - [Using Math Functions & Math Operators](#using-math-functions-math-operators)
    - [Using Ternary and Conditional Operators](#using-ternary-and-conditional-operators)
    - [Keywords](#new-world-keywords)
    - [Starting an Expression on a CompositionObject](#starting-an-expression-on-a-compositionobject)
- [E2E Building Examples](#e2e-building-examples)
    - [Parallaxing Listing Items](#parallaxing-listing-items)
        - [Old Expression](#parallaxing-old-expression)
        - [Summary of Expression definition](#parallaxing-summary-of-expression-definition)
        - [Building with ExpressionNodes](#parallaxing-building-with-expressionnodes)
        - [Final code snippet](#parallaxing-final-code-snippet)
    - [PropertySets](#propertysets)
        - [Old Expression](#propertysets-old-expression)
        - [Summary of Expression definition](#propertysets-summary-of-expression-definition)
        - [Building with ExpressionNodes](#propertysets-building-with-expressionnodes)
        - [Final code snippet](#propertysets-final-code-snippet)
    - [Curtain](#curtain)
        - [Old Expression](#curtain-old-expression)
        - [Summary Expression Definition](#curtain-summary-expression-definition)
        - [Building with ExpressionNodes](#curtain-building-with-expressionnodes)
        - [Final code snippet](#curtain-final-code-snippet)
- [Requirements](#requirements)
- [API](#api)

## <a name="quick-start"></a>Quick Start 

Welcome to the ExpressionBuilder classes! The ExpressionBuilder classes are a C#-only alternative to building Expressions with type safety. Below is a quick introduction to using the ExpressionBuilder classes with your application. Complete documentation and walkthroughs will start from [Intro](#intro) section.

## <a name="setting-up-the-expressionbuilder-classes-with-your-app"></a>Setting up the ExpressionBuilder classes with your app

To use the ExpressionBuilder in your app, add the Microsoft.Toolkit.Uwp.UI.Animations nuget package to your project. Next, within your app project, make sure to add the using statement to leverage the ExpressionBuilder classes:

```csharp
using  Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
``` 

Once you have the nuget package added to your project, you are all set to start using the ExpressionBuilder classes!

## <a name="getting-started-with-expressionbuilder-classes"></a>Getting started with ExpressionBuilder classes

### <a name="expressionAnimation-overview"></a>ExpressionAnimation Overview

A brief recap of ExpressionAnimations:

- ExpressionAnimations are a type of CompositionAnimations used to create mathematical relationships between Composition Objects. Simple examples include making a relationship such that one object will move relative to another. 

- Like other CompositionAnimations, ExpressionAnimations are templates, meaning you can create an Expression and use it to animate multiple objects. You can also change aspects of the animation and have those changes take effect the next time you animate an object (without affecting any previously connected animations). 

- For more information on ExpressionAnimations, [please check our documentation](https://docs.microsoft.com/uwp/api/Windows.UI.Composition.ExpressionAnimation).

ExpressionAnimations can create some very powerful and unique experiences, but can be a bit cumbersome to author. One of the big pain points with ExpressionAnimations is that the equation or mathematical relationship that defines the animation is written as a string, e.g.:

```csharp
_parallaxExpression = compositor.CreateExpressionAnimation(
	"(ScrollManipulation.Translation.Y + StartOffset) * ParallaxValue - 
	(ScrollManipulation.Translation.Y + StartOffset)");
``` 

This creates a series of challenges when authoring Expressions in this manner:

- No type safety checks

- No intellisense or autocomplete

- Semantic errors with the equation appear at runtime, not compile time

Thus, the ExpressionBuilder classes were created to help alleviate these challenges and present an alternative way to create ExpressionAnimations.

## <a name="using-the-expressionbuilder-classes"></a>Using the ExpressionBuilder classes

For full documentation on how to use the ExpressionBuilder classes, please refer to the [Word document](https://github.com/Microsoft/WindowsUIDevLabs/tree/master/ExpressionBuilder/Docs) from the composition team.

Before we highlight how to use the classes, let's reiterate the core components that make up an Expression:

- Parameters: These are key-value pairs that can either be references to a CompositionObject or constant values. The values to these keys can be changed later on.

- Functions: Similar to operators, there are a series of mathematical functions that provide a series of common math behaviors (clamp, max, min, cos, etc.)

- Keywords: a set of known phrases to the Expression language to provide specific behaviors (referencing the CurrentValue, etc.)

- Operators: The glue that brings together all the components of an equation. Your typical mathematical operators (+, -, *, /) 

On the surface, the ExpressionBuilder classes provide three major components to build Expressions with:

- ExpressionFunctions Class: object that defines all the math functions and type (Scalar, Vector2, Vector3, etc.) constructors

- ExpressionValues Class: object that defines the creation of parameters and keywords

- Extension Methods for Composition Objects: a series of extension methods off of CompositionObject and it's children.

Behind the scenes, methods off the ExpressionFunctions and ExpressionValues classes construct ExpressionNode objects that represent an Expression. These nodes can be combined with other ExpressionNodes or System.Numerics objects using operators, resulting in a new ExpressionNode. Via the extension methods, anywhere you would normally insert an ExpressionAnimation object, you can instead use an ExpressionNode.

### <a name="extension-methods"></a>Extension Methods (GetReference(), StartAnimation())

Prior to ExpressionBuilder, in order to reference a CompositionObject property, a SetReferenceParameter on the ExpressionAnimation must always be called:

```csharp
var expression = _compositor.CreateExpressionAnimation("visualA.Offset.X + 50");
expression.SetReferenceParameter("visualA", _visualA);
_visualB.StartAnimation("Offset.X", expression);
```
With ExpressionBuilder, you can use the GetReference() extension method that performs this same behavior if you don't need to template, but in a type safe manner:

```csharp
_visualB.StartAnimation("Offset.X", _visualA.GetReference().Offset.X + 50f);
```
Also notice in the above code snippet, the CompositionObject.StartAnimation() extension method  was used to pass in an ExpressionNode instead of an ExpressionAnimation.

### <a name="how-to-template-with-expressionbuilder"></a>How to template with ExpressionBuilder

Templating is a big value prop of CompositionAnimations. As a developer, you define a template for an animation that you then can create multiple instances of later when binding to CompositionObjects via StartAnimation(). In some cases, when templating, you want to change the value of parameters you define. For example, changing which Visual you want to reference, or changing the value of a constant. This means that parameters must be able to be referenced later on so their reference or value can be changed; for this reason, parameters are defined with a string property name.

In the following code snippet, we update the Expression defined earlier:

- Make the Visual we reference a parameter so it can be changed at a later time

- Create a constant parameter instead of hardcoding the value “50f”, so this can easily be changed at a later time

```csharp
var additionOffset = ExpressionValues.Constant.CreateScalarConstant("addOffset", 50f);
var expressionNode = ExpressionValues.Reference.CreateVisualReference("visualA", _visualA) + addOffset;
[...]
// If want to change what "visualA" references and value of "addOffset" in the Expression template ...
expressionNode.SetReferenceParameter("visualA", _visualC);
expressionNode.SetScalarParameter("addOffset", 100f);
```  

### <a name="e2e-example"></a>E2E Example

Let's walk through the expression used in the PullToAnimate sample to animate Opacity with InteractionTracker

```csharp
// Expression written with strings
var progressExp = _compositor.CreateExpressionAnimation();
progressExp.Expression = "Clamp(tracker.Position.Y / tracker.MaxPosition.Y, 0, 1)";
progressExp.SetReferenceParameter("tracker", _tracker);
visual.StartAnimation("Opacity", progressExp);
```

Now let's show what this looks like with ExpressionBuilder:

```csharp
// Expression written with ExpressionBuilder
var trackerNode = _tracker.GetReference();
var progressExp = EF.Clamp(trackerNode.Position.Y / trackerNode.MaxPosition.Y, 0, 1);
_propertySet.StartAnimation("progress", progressExp);
```

### <a name="things-to-note"></a>Things to Note

If you are familiar with how Expressions were built with strings, there are a few things to note:

- The ternary operator (condition ? ifTrue : ifFalse) is now represented by ExpressionFunctions.Conditional(condition, ifTrue, ifFalse)

- The "And" and "Or" operators (“&&” and “||”) are now represented by the & and | operators.

- If using ExpressionBuilder to create expressions for use with InteractionTracker’s InertiaModifiers, the following extensions methods are available:

	- InteractionTrackerInertiaRestingValue.SetCondition

	- InteractionTrackerInertiaRestingValue.SetRestingValue

	- InteractionTrackerInertiaMotion.SetCondition

	- InteractionTrackerInertiaMotion.SetMotion

	- Referencing ExpressionValues and ExpressionFunctions in your code can be a bit verbose, so you can define shortened versions in the Using section of your app:

	```csharp
	using EF = ExpressionBuilder.ExpressionFunctions;
	using EV = ExpressionBuilder.ExpressionValues;
	```

# <a name="intro"></a>Intro

## <a name="what-are-expressions">What are Expressions?

ExpressionAnimations (or Expressions, for short) are a new type of animation introduced to Windows App developers in Windows 10 to provide a more expressive animation model than what is provided from traditional KeyFrameAnimations and
XAML Storyboards.

Expressions are mathematical equations and relationships that are defined by the developer and used by the system to calculate the value of an animation property each frame. These equations can be used to define relationships between objects such as relative size to more complex UI experiences such as Parallax, Sticky Headers, and other input-driven experiences.

The documentation below assumes you are familiar with the Composition and CompositionAnimation APIs, including Expressions. For more information on these, check out the following resources:

- [Composition Overview](https://msdn.microsoft.com/windows/uwp/graphics/visual-layer)

- [Composition Animation Overview](https://msdn.microsoft.com/windows/uwp/graphics/composition-animation)

- [Windows UI Dev Labs Github](https://github.com/Microsoft/WindowsUIDevLabs)

- [ExpressionAnimation MSDN Documentation](https://msdn.microsoft.com/library/windows/apps/windows.ui.composition.expressionanimation.aspx)

## <a name="why-expressionbuilder">Why ExpressionBuilder?

To use ExpressionAnimations today, developers are required to write their mathematical equation/relationship in a string (example shown below).

```csharp
parallaxExpression = compositor.CreateExpressionAnimation();
_parallaxExpression.Expression = "(ScrollManipulation.Translation.Y + StartOffset - (0.5 * ItemHeight)) * ParallaxValue - (ScrollManipulation.Translation.Y + StartOffset - (0.5 * ItemHeight))";
```

This experience presents a series of challenges for developers:

- No Intellisense or auto complete support.

- No type safety when building equations.

- All errors are runtime errors, many of which are desirable to be detected at compile time.

- Working with strings for complicated equations not intuitive or ideal.

To improve the Expression authoring experience, the team put together the ExpressionBuilder classes, which act as a series of “helper classes” to bring
type safety, Intellisense, and compile-time errors to the Expression-building experience. The classes can be used as an alternative experience to building Expressions than what is available today.

# <a name="how-to-build-core-components-of-expressions"></a>How to: Build Core Components of Expressions

The following sections will cover how the classes work to create the core
components of an Expression.

**Note:** Each section will provide short code lines that implement the Expression Builder syntax to demonstrate the core concept. These are not meant to be E2E working samples - E2E walkthroughs using the Expression Builder Classes are provided in Section [E2E Building Examples](#e2e-building-examples).

## <a name="general-construction"></a>General Construction

What is the general model for building Expressions with the classes? How do the classes plug into existing entry points for CompositionAnimations? (StartAnimation, ExpressionKeyFrames)

Before talking about the core components of Expressions and how the ExpressionBuilder classes achieve them, let’s first discuss how to think about these classes, their general architecture and how to integrate into your existing app code.

### <a name="interacting-with-expressionnodes-via-static-methods"></a>Interacting with ExpressionNodes via Static Methods

To start, let’s talk about how Expressions are represented in the Helper Class. When using the Expression Builder Classes, developers will be generating ExpressionNodes – a single node, or a combination of nodes, can be used to define an Expression. It is important to note that you do not need to “new-up” ExpressionNodes; instead, there are a series of static methods to create typed Expression nodes such as:

- ScalarNode

- Vector2Node

- Vector3Node

- Vector4Node

- ColorNode

- QuaternionNode

- Matrix3x2Node

- Matrix4x4Node

For example, to generate a Vector3ExpressionNode, use a static method:

```csharp
// Using ExpressionFunction class and the Vector3 static method
// Defines an Expression that creates a Vec3 of 1, 2, 3 
var vec3Node = ExpressionFunctions.Vector3(1.0f, 2.0f, 3.0f);
```

Don’t worry about understanding the syntax right now, we’ll cover that later. For now, understand that you will create most ExpressionNodes using static methods off two main classes:

- ExpressionValues

- ExpressionFunction

As stated earlier, a combination of ExpressionNodes can be used to define an
Expression. Like any math equation, ExpressionNodes can be combined using mathematical or logical operators and mathematical functions (more on this
later).

For example, the code snippet below shows adding together two Vector3Nodes that will result in a Vector3Node (invalid combinations will be caught as
compile-time errors).

```csharp
// Combining together multiple Expression Nodes using add operator
Vector3Node vec3Sum = ExpressionFunctions.Vector3(1.0f, 2.0f, 3.0f) +
                      ExpressionFunctions.Vector3(4.0f, 5.0f, 6.0f);
```

### <a name="implicit-conversion-to-expressionnodes"></a>Implicit Conversion to ExpressionNodes

In addition to using the static methods to generate ExpressionNodes, the classes will also handle implicit conversion of numerical values (e.g. System.Numerics) to the appropriate ExpressionNode type. This is done so you do not need to explicitly create new nodes for existing Numerics objects you have already defined.

For example, if you have already defined a System.Numerics object, you can use it directly when combining with other ExpressionNodes:

```csharp
// Using a Numerics Vector3 object directly with an ExpressionNode
Vector3Node vec3NodeA = ExpressionFunctions.Vector3(1.0f, 2.0f, 3.0f);
Vector3Node vec3Sum = vec3NodeA + new System.Numerics.Vector3(1.0f, 1.0f, 1.0f);
```

When building your Expression with ExpressionNodes, you can use a numerics type anywhere an ExpressionNode would normally be used and it will get implicitly converted to one. For example, the math function Length(…) takes in a QuaternionNode, if a System.Numerics.Quaternion object was provided, it would get implicitly converted:

```csharp
var quatLength = ExpressionFunctions.Length(new Quaternion(new Vector3(1), 1f));
```

### <a name="using-expressionnodes-with-startanimation"></a>Using ExpressionNodes with StartAnimation

Once you’ve created an ExpressionNode using the ExpressionBuilder classes, you need to connect the ExpressionNode to a target (CompositionObject). The ExpressionBuilder classes include an extension method that mimics the publicly available StartAnimation(…) API, but instead of taking in a CompositionAnimation, it takes in an ExpressionNode. This extension method is defined in CompositionExtension.StartAnimation(…), and is accessible via CompositionObject.StartAnimation(…).

The following uses the ExpressionNode defined earlier and attaches it via StartAnimation to a Composition Visual:

```csharp
var numericsVec3 = new System.Numerics.Vector3(1.0f, 1.0f, 1.0f);
var vec3NodeA = ExpressionFunctions.Vector3(1.0f, 2.0f, 3.0f);
Vector3Node vec3Sum = vec3NodeA + numericsVec3;

// StartAnimation extension method takes in a ExpressionNode
_visualA.StartAnimation("Offset", vec3Sum);
```

### <a name="using-expressionnodes-with-expressionkeyframes"></a>Using ExpressionNodes with ExpressionKeyFrames

In the existing CompositionAnimation API, you can also use Expressions with KeyFrameAnimations. This is done by using an ExpressionKeyFrame where you define the progression point and a string representing the equation (the Expression provided is used by the system to evaluate the keyframe value each frame).

The ExpressionBuilder classes also provides an extension method for the InsertExpressionKeyFrame API that takes in an ExpressionNode instead of a string.

The following defines a KeyFrameAnimation that uses an ExpressionKeyFrame with an ExpressionNode:

```csharp
var kfa = _compositor.CreateVector3KeyFrameAnimation();

Vector3Node vec3Sum = ExpressionFunctions.Vector3(1.0f, 2.0f, 3.0f) + 
                      new System.Numerics.Vector3(1.0f, 1.0f, 1.0f);

// Extension method for InsertExpressionKeyFrame to take in an ExpressionNode
kfa.InsertExpressionKeyFrame(1.0f, vec3Sum);
_visual.StartAnimation("Offset", kfa);
```

**Note:** In the above example, the Expression only consists of constant parameters for example purposes. Your Expression should always contain at least one reference; an Expression made up of only constant parameters is wasteful as it can be equivalently accomplished with a direct property set via the API .

### <a name="using-expressionnodes-in-other-places"></a>Using ExpressionNodes in other places

Using ExpressionNodes with StartAnimation and ExpressionKeyFrames will be the most common places you will utilize Expressions. However, there are other places that Expressions are used today – for each of the cases below, extension methods are provided that will take in an ExpressionNode instead of a string:

- InteractionTrackerInertiaMotion Extension Methods

    - SetCondition

    - SetMotion

- InteractionTrackerInertiaRestingValue ExtensionMethods

    - SetCondition

    - SetMotion

## <a name="parameters"></a>Parameters

The big value prop to use ExpressionAnimations is that you can define equations and mathematical relationships that utilize constants and reference values from other objects. These objects are often other CompositionObjects or variables that make the mathematical relationship more meaningful. For example, you can use a parameter to create an equation that references another’s object’s x Offset.

There are two types of Parameters: Constants and References. Both Constants and References can be described as either a dynamic or static Parameter - this defines whether you can change what they refer to. These topics will be discussed in more detail in the next sections.

### <a name="definitions-constants-vs-references"></a>Definitions: Constants vs. References

There are two types of Parameters that can be used in an Expression. Moving forward, we’ll use the following definitions to distinguish Constants vs References:

- **Constant:** A typed value (Scalar, Vector2/3/4, etc.), which will be used directly as a literal in the Expression.

```csharp
var extraOffset = new Vector3(50f, 50f, 0f);
```

- **Reference:** A CompositionObject (Visual, Clip, InteractionTracker, etc.), whose properties will be evaluated each frame the Expression is processed in the Compositor.

    - The usefulness of including a Reference Parameter in an Expression is to reference properties off it (e.g. referencing the Offset property of a Visual).

```csharp
var redVisual = _compositor.CreateSpriteVisual();
```

### <a name="definitions-dynamic-vs-static-parameters"></a>Definitions: Dynamic vs. Static Parameters

Templating, which is discussed in more detail in section [Templating](#templating) refers to the reuse of an Expression while changing the values of dynamic parameters. Unless you are in a templating scenario, you will only need static parameters.

- **Static:** A parameter in which the value or CompositionObject it references will never change

- **Dynamic:** A parameter in which the value or CompositionObject can be changed without modifying the Expression. It is changed by associating a string parameter name with a new value or CompositionObject. Dynamic parameters are required for templating.

How to create static and dynamic parameters for both Constants and References will be discussed in the next two sections.

### <a name="creating-constant-parameters"></a>Creating Constant Parameters

**How to create static Constant Parameters:**

- Simply place the value straight into the equation

In the example below, we want to utilize a float constant variable that gets defined earlier in the equation.

```csharp
var delta = new Vector3(50.0f);

// Place “delta” into the equation to represent a static constant parameter 
// (aka a constant value)
var newPosition = ExpressionFunctions.Vector3(50.0f, 75.0f, 0.0f) + delta;

// When StartAnimation is called, for the lifetime of the Expression, 
// “delta” will be a Vec3 of (50,50,50)
_visual.StartAnimation("Offset", newPosition);
```

**Note:** Plugging in CompositionObject values directly into the equation will have the same effect, as they are just variable values. For example, plugging in \_visual.Offset will get evaluated to its Vector3 value and treated as a Constant. If you want the equation to use the frame-accurate value of a CompositionObject property, make it either a static or dynamic Reference Parameter.

**How to create dynamic Constant Parameters:**

- Create a named parameter using the static methods off ExpressionValues.Constants class

You can create a constant parameter via static Create\*Parameter() methods (e.g. ExpressionValues.Constants.CreateScalarParameter(“foo”, 7)). Note: setting the initial value as part of the creation is optional; you can always set the value of the parameter using ExpressionNode.Set\*Parameter(). Let’s expand the above example. In this case, let’s say we want to create a generic equation that can be reused for similar scenarios, but tailored by changing the value of constant(s). In the example below, we create the Expression that contains a Constant Parameter, using ExpressionValues.Constant.CreateConstantVector3(…). Before connecting it to a target, the Expression is tailored by setting the parameter using ExpressionNode.SetVector3Parameter(…).

```csharp
var delta = new Vector3(50.0f);

// Create a dynamic Constant Parameter for delta of value (50,50,50)
var newPosition = ExpressionFunctions.Vector3(50.0f, 75.0f, 0.0f) +
                  ExpressionValues.Constant.CreateConstantVector3("delta", delta);

_visualA.StartAnimation("Offset", newPosition);

// [...]
// Later in the code, we want to reuse the newPosition ExpressionNode but with a 
// different value for the “delta” parameter.
newPosition.SetVector3Parameter("delta", new Vector3(75f, 85f, 0f));

// When StartAnimation is called, delta parameter is (75,85,0) not (50,50,50)
_visualB.StartAnimation("Offset", newPosition);
```

### <a name="creating-reference-parameters"></a>Creating Reference Parameters

**How to create static Reference Parameters:**

- Call the GetReference() extension method off of the CompositionObject you would like to create a reference for

In the example below, we further expand on the above code such that instead of using a constant Vector3 for the first part of the equation, we will reference the frame-accurate Offset of a Visual:

```csharp
var delta = new Vector3(50.0f);

// Create a static Reference Parameter to the _redBall Visual and its Offset property
var newPosition = _redBall.GetReference().Offset + delta;
_visual.StartAnimation("Offset", newPosition);
```

Thus, as the Offset value of \_redBall changes (via direct property set or animation), so will the output of this equation.

**How to create dynamic Reference Parameters:**

- Create a Parameter using the static methods off the ExpressionValues.Reference class

    - The value of the parameter can be set via the SetReferenceParameter function

In the example below, we further expand on the above code such that instead of using a static reference to the redBall visual, we use a named one, so that we can change the parameter to refer to a blueBall Visual later.

```csharp
var delta = new Vector3(50.0f);

// Create a dynamic Reference Parameter to the Red Ball Visual so can change later 
var newPosition = ExpressionValues.Reference.CreateVisualReference("ball").Offset + delta;

// When StartAnimation is called, “ball” parameter refers to _redBall Visual
newPosition.SetReferenceParameter("ball",_redball);
_visual.StartAnimation("Offset", newPosition);

// [Later in code, we want to use same Expression but use different Visual for Parameter]

// When StartAnimation is called, “ball” parameter refers to _blueBall Visual
newPosition.SetReferenceParameter("ball", _blueBall);
_visualB.StartAnimation("Offset", newPosition);
```

To refer to a property in a CompositionPropertySet, you will need to get or create a reference to the PropertySet, then call GetParameter() function and pass in the name of the property in the form of a string.

```csharp
_propSet.InsertScalarProperty("delta", new Vector3(10));

// Create a static Reference Parameter to a PropertySet and a property inside it
var newPosition = _redBall.GetReference().Offset +
	           _propSet.GetReference().GetVector3Property("delta");
_visual.StartAnimation("Offset", newPosition);
```

### <a name="subchanneling-swizzling"></a>Subchanneling (Swizzling)

*[Also known as “dotting into things”]*

When using a vector or matrix node type in your equations, you also can access a subchannel of (or “dot into”) the parameter to use an individual component property. For example, when using a Vector3 Constant, developers can dot into its X, Y, or Z component:

```csharp
var delta = new Vector3(50f, 100f, 150f);
Vector3Node vec3NodeA = ExpressionValues.Constant.CreateConstantVector3("delta", delta);
ScalarNode xComponent = vec3NodeA.X;
ScalarNode yComponent = vec3NodeA.Y;
ScalarNode zComponent = vec3NodeA.Z;
```

In addition, when using Reference Parameters, all the animatable properties on the different CompositionObjects can be subchanneled into as well. For example, modifying the above example to use the offset of a Visual instead of a Vector3 constant:

```csharp
// _redBall is a SpriteVisual created previously
var vec3NodeA = _redBall.GetReference().Offset;
var xComponent = vec3NodeA.X;
var yComponent = vec3NodeA.Y;
var zComponent = vec3NodeA.Z;
```

The classes only provide the common subchannels off the different types. However, in Expressions, you can also reference more complicated subchannels such as XX, XXY, etc. In this case, you can use the Subchannels(…) function to define a particular combination:

In the example below, the developer wants to grab a subchannel reference to an XY component of a Visual’s Offset. The output of this is a Vector2Node:

```csharp
Vector2Node xYChannel = _visual.GetReference().Offset.Subchannels(
	                 Vector3Node.Subchannel.X, Vector3Node.Subchannel.Y);
```

### <a name="templating"></a>Templating

There are times where you want to use a generic Expression across different parts of your app to animate multiple CompositionObjects. However, depending on which target the Expression is connected to, different values for parameters in the equation may be desired.

This is where using named Dynamic Parameters with Constant and Reference Parameters comes into play. When creating parameters using the ExpressionValues.Constant or ExpressionValues.Reference classes, you define a string name that you will later use to set the value of the parameter. For constants, this will be a different variable/value that you want to use. For References, this will be a different CompositionObject that you want to reference.

**Note:** Whenever a templated Expression is connected to a target (via StartAnimation(…)), an instance of that Expression is created and associated with that target. For this reason, any changes to parameters (via Set\*Parameter(…)) only affect the template and future instances created from that template. For example, take an Expression with a parameter “P” that is connected to three targets: “T1”, “T2”, and “T3” (in order). If the value of “P” is changed after “T1” and “T2” have been connected, this new value will only be used in “T3”.

In the example below, we create a generic Expression and attach to two different Visuals. Each time, we change the value of the parameter before starting the animation.

```csharp
// Define the Expression template
var delta = new Vector3(50.0f);
var deltaExpression = 
	ExpressionValues.Reference.CreateVisualReference("visual").Offset + delta;

// [Later on in code  ...]
// Set value of "visual" to be a reference to _redBall
deltaExpression.SetReferenceParameter("visual", _redBall);
_visualA.StartAnimation("Offset", deltaExpression);

// [Later on in code ...]
// Set value of "visual" to be a refernece to _blueBall
deltaExpression.SetReferenceParameter("visual", _blueBall);
_visualB.StartAnimation("Offset", deltaExpression);
```

A real-world example that demonstrates the need for changing the value of a Parameter using different constants would be using the index number of an itemized List as a Constant Parameter.

We can extend this concept by imagining a real-world scenario in which a common equation is needed across many targets: list items. A list is typically comprised of homogeneous items, each with a unique ID indicating its position in the list. Each item needs to be behave very similarly, with slight differences based on its position. For this example, a single Expression could be designed that gives a consistent behavior across all items, but is customized by using the list item ID as a Constant Parameter. When connecting this Expression template to each list item, the ID Parameter is set using SetScalarParameter(…) with the ID of the current list item.

### <a name="expressions-keywords"></a>Keywords

In Expressions, there are several certain keywords that can be used as shortcuts when defining the equation. These keywords are available off of the
ExpressionValues object:

- **ExpressionValues.Target** – This keyword defines a reference to whichever CompositionObject this Expression is connected to.

- **ExpressionValues.StartingValue** – This keyword defines a reference to the property the Expression targets, sampled at the first frame of execution. **Note**: if the Expression is connected to a subchannel of a property (e.*g. “Offset.X”), then StartingValue will be of the same data type as the subchannel (e.g. ScalarStartingValue for “Offset.X”).

- **ExpressionValues.CurrentValue** – This keyword defines a frame-accurate reference to the property the Expression targets. *Note: if the Expression is connected to a subchannel of a property (e.*g. “Offset.X”), then CurrentValue will be of the same data type as the subchannel (e.g. ScalarCurrentValue for “Offset.X”).

In the example below, we create an Expression using the Target keyword:

```csharp
// windowWidth defined earlier
// Target creates a reference to _visual
var opacityExpression = 
	ExpressionValues.Target.CreateVisualTarget().Offset.X / windowWidth;
_visual.StartAnimation("Opacity", opacityExpression);
```

 The usage of the Target keyword here is a shortcut to a Reference Parameter that references the CompositionObject being targeted by the Expression. In this example, that would be \_visual, but will always refer to the object StartAnimation(…) is called on.

## <a name="math-shortcuts-basic-operators"></a>Math shortcuts & basic operators

### <a name="basic-operators"></a>Basic Operators

As mentioned earlier, you define an Expression by a single ExpressionNode or multiple – when defined by multiple, they are combined using operators. The basic supported operators are:

- Plus (+)

- Minus (-)

- Multiply (\*)

- Divide (/)

- Mod (%)

The classes are designed such that you will be able to use Intellisense to identify compile-time errors for invalid math operations. For example, the following ExpressionNode attempts to add a Vector3 reference from a Visual to Vector2 constant – note that this will throw a compile time error in Visual Studio:

```csharp
var numericsVec2 = new Vector2(1f, 2f);

// Invalid operation: cannot add Vec3 to Vec2, compile time + intellisense error
var expNodeSum = _visual.GetReference().Offset + numericsVec2;
```

### <a name="math-shortcuts-functions"></a>Math Shortcuts (Functions)

To build more complex equations, more advanced mathematical operations are needed. Some operations are tedious to perform manually, so helper functions (a subset of System.Numerics functions, e.g. Min, Max, etc.) are available in the ExpressionFunctions class. The following example creates an Expression that calculates the length of a Quaternion:

```csharp
var targetVisual = _visual.GetReference(); 
var quatLength = ExpressionFunctions.Length(targetVisual.Orientation);
```

## <a name="advanced-operations"></a>Advanced Operations

### <a name="comparison-operators"></a>Comparison Operators

In addition to the basic mathematical operations (+, -, /, etc.), you can also create Expressions that use comparison operators:

- Greater than (\>)

- Less than (\<)

- Greater than or equal to (\>=)

- Less than or equal to (\<=)

- Equal to (==)

- Not Equal to (!=)

The following example demonstrates creating an Expression that outputs a Boolean Node showing whether the length of one Quaternion is equal to another:

```csharp
var visAReference = _visualA.GetReference();
var visBReference = _visualB.GetReference();
             
BooleanNode equalLength = ExpressionFunctions.Length(visAReference.Orientation) ==
                  ExpressionFunctions.Length(visBReference.Orientation);
```

### <a name="conditional-operation"></a>Conditional Operation

Finally, you can make some of the most powerful Expressions using a Conditional operation. This enables developers to define different behaviors for an Expression depending on a condition. This operation is defined by the ExpressionFunctions.Conditional method and contains three parts to mimic the standard *condition ? true: false* ternary operator:

- The Boolean Expression condition that is checked

- The Expression to be run if the condition is true

- The Expression to be run if the condition is false

The following example builds upon the above example. It compares the length of two quaternions, and based on the result uses one of two rotations in the form of a quaternion:

```csharp
var visAReference = _visualA.GetReference();
var visBReference = _visualB.GetReference();
             
var condition = ExpressionFunctions.Length(visAReference.Orientation) <=
                ExpressionFunctions.Length(visBReference.Orientation);

var trueCase = visAReference.Orientation;
var falseCase = visBReference.Orientation;

// This Expression chooses between A or B’s Orientation, based on their lengths
var ternary = ExpressionFunctions.Conditional(condition, trueCase, falseCase);

_visual.StartAnimation("Orientation", ternary);
```

## <a name="tips-and-tricks-for-using-classes"></a>Tips and Tricks for using Classes

Below are some tips and tricks that can be used for interacting with the classes

### <a name="shortening-class-names"></a>Shortening Class Names

One of the challenges with this class model is an ExpressionNode can get very verbose and lengthy because the needing to “dot into” a class object to access the static method. If you run into this yourself, you can shorten the naming of the classes by defining a shortened version via the “using” syntax at the top of your file:

```csharp
using EF = HelperClasses.ExpressionFunctions;
// Later in code ...
var test = EF.Abs(_visual.GetReference().Offset.X);
```

# <a name="translating-old-world-to-new"></a>Translating Old World to New

If you’re familiar with building Expressions in the old world by writing the equation as a string, the following sections outlines how the creation of an Expression compares between the old and new way.

## <a name="creating-an-expression"></a>Creating an Expression

In the old world, you use the CreateExpressionAnimation() method off the Compositor. In the new one, you simply assign the variable an ExpressionNode (output from static methods of ExpressionValue, ExpressionFunctions or extension methods)

```csharp
// Old way
var expOldWorld = _compositor.CreateExpressionAnimation("visB.Offset");
expOldWorld.SetReferenceParameter("visB", _visualB);

// New way
var expNewWorld = _visualB.GetReference().Offset;
```

## <a name="defining-constant-parameters"></a>Defining Constant Parameters

In the old world for both Constants and References, whether you intended them to be static or dynamic, you were required to define a string name and set the parameter value separately. In the new world, you only need to set the parameter if you want to template the Expression and later change what the parameter points to. Otherwise, you simply include the value directly in the equation.

In the example below, we plan to template this expression, varying the value of “extraOffset”. Shown is how to achieve this in the new and old way:

```csharp
var extraOffset = new Vector3(50f);

// Old way
var expOldWorld = _compositor.CreateExpressionAnimation("visB.Offset + extraOffset");
expOldWorld.SetReferenceParameter("visB", _visualB);
expOldWorld.SetVector3Parameter("extraOffset", extraOffset);

// New way (Note: we could have set value in CreateConstantVector3)
var expNewWorldTemplate = _visualB.GetReference().Offset +
                          ExpressionValues.Constant.CreateConstantVector3("extraOffset");
expNewWorldTemplate.SetVector3Parameter("extraOffset", extraOffset);
```

## <a name="building-constants"></a>Building Constants

In the old world, you could construct constant types within the string equation. In the new world, you use the static methods off the ExpressionFunction class.

```csharp
// Old way
var expOldWorld = 
	_compositor.CreateExpressionAnimation("visB.Offset + Vector3(50f, 50f, 0f)");
expOldWorld.SetReferenceParameter("visB", _visualB);

// New Way (Option 1)
var expNewWorldTemplate = 
	_visualB.GetReference().Offset + ExpressionFunctions.Vector3(50f, 50f, 50f);
// New Way (Option 2)
var expNewWorldTemplate = 
	_visualB.GetReference().Offset + new System.Numerics.Vector3(50, 50, 50);
```

## <a name="defining-reference-parameters"></a>Defining Reference Parameters

In the old world, if you wanted to create a reference to a CompositionObject, it needed to have the parameter set for the string name in the equation. In the new world, you can either *get* the reference using the extension method off the CompositionObject, or create one using static methods off ExpressionValue.

```csharp
// Old way
var expOldWorld = _compositor.CreateExpressionAnimation("visB.Offset");
expOldWorld.SetReferenceParameter("visB", _visualB);

// New Way (1)
// "Get" a reference to a known Composition Object
var expNewWorld = _visualB.GetReference().Offset;

// New Way (2)
// "Create" a reference and assign the value
var expNewWorldTemplate = 
	ExpressionValues.Reference.CreateVisualReference("visB").Offset;
expNewWorldTemplate.SetReferenceParameter("visB", _visualB);
```

## <a name="using-math-functions-math-operators"></a>Using Math Functions & Math Operators

In the old world, you would include the function name inside the string equation. This presented problems with misspelling, knowing what parameters to provide and type safety on the output. In the new world, you get this through Intellisense as all the available Math functions are available off the ExpressionFunction class.

For operators, they were simply included in the string in the old world. In the new world, you can use them similar to the System.Numerics experience.

```csharp
// Old way
var extraOffset = new Vector3(50f);
var expOldWorld = _compositor.CreateExpressionAnimation(
	"Lerp(0f, 1f, visB.Offset.X / windowWidth");
expOldWorld.SetReferenceParameter("visB", _visualB);
expOldWorld.SetScalarParameter("windowWidth", windowWidth);

// New Way
var expNewWorld = 
	ExpressionFunctions.Lerp(0f, 1f, _visualB.GetReference().Offset.X / windowWidth);
```

## <a name="using-ternary-and-conditional-operators"></a>Using Ternary and Conditional Operators

In the old world you would use the *Condition ? ifTrue : ifFalse* format for the ternary operation, using the appropriate conditional operators in the condition portion of the string. In the new world, the Ternary operator behavior is found off the Conditional function under the ExpressionFunctions class. All the same comparison operators are supported and can be used in the same format like the basic math operators.

```csharp
// rotateBy30, rotateBy45 are Quaternions defined earlier

// Old way
var expOldWorld = _compositor.CreateExpressionAnimation(
	"(visA.Orientation == visB.Orientation) ? rotBy30 : rotby45");
expOldWorld.SetReferenceParameter("visB", _visualB);
expOldWorld.SetReferenceParameter("visA", _visualA);
expOldWorld.SetQuaternionParameter("rotBy30", rotateBy30);
expOldWorld.SetQuaternionParameter("rotBy45", rotateBy45);

// New Way
var condition = ExpressionFunctions.Length(_visualA.GetReference().Orientation) ==
                ExpressionFunctions.Length(_visualB.GetReference().Orientation);
var expNewWorld = ExpressionFunctions.Conditional(condition, rotateBy30, rotateBy45);
```

## <a name="new-world-keywords"></a>Keywords

In the old world, there were reserved string keywords that can be used to achieve specific behavior:

- This.StartingValue

- This.CurrentValue

- This.Target

- Pi

- True/False

The challenge with this model in the old world was they were not discoverable.

In the new world, the StartingValue/CurrentValue/Target keywords are made available off the ExpressionValue class. For the use of Pi and True/False, the values defined in C\# are sufficient.

```csharp
// Old way
var expOldWorld = _compositor.CreateExpressionAnimation(
	"visA.RotationAngle <= Pi ? this.StartingValue : fullSize");
expOldWorld.SetReferenceParameter("visA", _visualA);
expOldWorld.SetVector2Parameter("fullSize", fullSize);

// New Way
var condition = ExpressionFunctions.Length(_visualA.GetReference().RotationAngle) <= (float)Math.PI;
var expNewWorld = ExpressionFunctions.Conditional(condition, ExpressionValues.StartingValue.CreateVector2StartingValue(), fullSize);
```

## <a name="starting-an-expression-on-a-compositionobject"></a>Starting an Expression on a CompositionObject

In the old world, developers utilized the StartAnimation() method off of CompositionObject that passed in two values: the string name of the property animate and the ExpressionAnimation defined by a string. In the new world, there is an extension method that takes in an ExpressionNode instead an ExpressionAnimation.

# <a name="e2e-building-examples"></a>E2E Building Examples

This section is dedicated to walking through building a few different Expressions using the Expression Builder Classes. Each of the examples will start with an Expression (and any needed supporting code) and break down how these can be re-written using the new classes. All the examples will be pulled from samples on the [Windows UI Dev Labs Github Project](https://github.com/Microsoft/WindowsUIDevLabs).

There is an assumption that the reader has a general understanding of what Expressions are and how the ExpressionBuilder classes work. If not, it is recommended to read the [Intro](#intro) and [How to: Build Core Components of Expressions](#how-to-build-core-components-of-expressions) first.

## <a name="parallaxing-listing-items"></a>Parallaxing Listing Items

([Github Link](https://github.com/Microsoft/WindowsUIDevLabs/tree/master/SampleGallery/Samples/SDK%2010586/ParallaxingListItems))

The first example we will walk through is the Parallaxing List Item sample found on the Windows UI Dev Labs Github Sample Gallery project. In this sample, we want to create a UI experience such that the background image for each list item parallax as the user scrolls through the list.

### <a name="parallaxing-old-expression"></a>Old Expression

Let’s first look at the relevant code for how the Expression is built today using strings:

```csharp
_parallaxExpression = compositor.CreateExpressionAnimation();
_parallaxExpression.SetScalarParameter("StartOffset", 0.0f);
_parallaxExpression.SetScalarParameter("ParallaxValue", 0.5f);
_parallaxExpression.SetScalarParameter("ItemHeight", 0.0f);
_parallaxExpression.SetReferenceParameter("ScrollManipulation", _scrollProperties);
_parallaxExpression.Expression = "(ScrollManipulation.Translation.Y + StartOffset - (0.5 * ItemHeight)) * ParallaxValue - (ScrollManipulation.Translation.Y + StartOffset - (0.5 * ItemHeight))";
//[Later in the code …]
_parallaxExpression.SetScalarParameter(
	"StartOffset", (float)args.ItemIndex * visual.Size.Y / 4.0f);
visual.StartAnimation("Offset.Y", _parallaxExpression);
```

### <a name="parallaxing-summary-of-expression-definition"></a>Summary of Expression definition

- The core of this Expression is uses a ScrollManipulationPropertySet, a CompositionPropertySet that contains information about the XAML ScrollViewer that manages the item in the XAML ListView.

    - Specifically, we are looking at the Translation.Y property. When building our Expression, we will need to grab a reference to this property.

- There are three other scalar parameters that comprise the remainder of this equation (StartOffset, ParallaxValue and ItemHeight). Note, that in this sample, the intent was to make this Expression a template, meaning that these values may need to be changed later.

    - If the intent was not to template, the Expression would have been created differently, with the values being written directly into the string.

- Finally, the equation itself has a common component (we’ll denote it “A”) that gives it the form A\*Parallax – A.

    - In this case “A” is:  
        "(ScrollManipulation.Translation.Y + StartOffset - (0.5 \* ItemHeight))”

### <a name="parallaxing-building-with-expressionnodes"></a>Building with ExpressionNodes

So let’s get started building this Expression into an ExpressionNode. To start, we’ll make three variables to keep track of the three Scalar Parameters and specifically for templating purposes:

```csharp
var startOffset = 
	ExpressionValues.Constant.CreateConstantScalar("startOffset", 0.0f);
var parallaxValue = 
	ExpressionValues.Constant.CreateConstantScalar("parallaxValue", 0.5f);
var itemHeight = 
	ExpressionValues.Constant.CreateConstantScalar("itemHeight", 0.0f);
```

Next, let’s get a reference to that ManipulationPropertySet (specifically, the Translation.Y property). To do that, we need to:

- Get a reference to the PropertySet

- Use the static method to get the Translation.Y property

    - A Scalar property

    This can be done all in one line:

    ```csharp
    var yTranslation =
    _scrollProperties.GetSpecializedReference<ManipulationPropertySetReferenceNode>()
    .Translation.Y;
    ```

    (For walkthrough purposes, this is stored as a separate variable and then put into the final equation. This also could have been included directly in the final ExpressionNode parallax below.)

Now let’s build out the “A” component of the A\*Parallax – A format that was described earlier:

```csharp
var parallax = (yTranslation + startOffset - (0.5f * itemHeight));
```

Now we are ready to build out the full Expression pass it into the StartAnimation() function call!

```csharp
var parallaxExpression = parallax * parallaxValue - parallax;

// Later on in the code 
visual.StartAnimation("Offset.Y", parallaxExpression);
```

### <a name="parallaxing-final-code-snippet"></a>Final code snippet

```csharp
// Not necessary to define as variables, could put directly into parallax variable
var startOffset = 
	ExpressionValues.Constant.CreateConstantScalar("startOffset", 0.0f);
var parallaxValue = 
	ExpressionValues.Constant.CreateConstantScalar("parallaxValue", 0.5f);
var itemHeight = 
	ExpressionValues.Constant.CreateConstantScalar("itemHeight", 0.0f);
// Not necessary to define into variable, could put directly into parallax variable
var yTranslation =
_scrollProperties.GetSpecializedReference<ManipulationPropertySetReferenceNode>()
.Translation.Y;

var parallax = (yTranslation + startOffset - (0.5f * itemHeight));
var parallaxExpression = parallax * parallaxValue - parallax;

// This Expression is connected later in the code.
visual.StartAnimation("Offset.Y", parallaxExpression);
```

## <a name="propertysets"></a>PropertySets

([Github Project](https://github.com/Microsoft/WindowsUIDevLabs/tree/master/SampleGallery/Samples/SDK%2010586/PropertySets))

The second example we will walk through is the PropertySets sample on the Windows UI Dev Labs Sample Gallery Github project. In this sample, we want to make a UI experience where we want to have a colored ball orbit another that is moving up and down.

### <a name="propertysets-old-expression"></a>Old Expression

Let’s first look at the relevant code for how the Expression is built today using strings:

```csharp
ExpressionAnimation expressionAnimation = 
compositor.CreateExpressionAnimation(
	"visual.Offset + propertySet.CenterPointOffset + " + 	"Vector3(cos(ToRadians(propertySet.Rotation)) * 150," +                                                                                            	"sin(ToRadians(propertySet.Rotation)) * 75, 0)");

expressionAnimation.SetReferenceParameter("propertySet", propertySet);
expressionAnimation.SetReferenceParameter("visual", redSprite);

blueSprite.StartAnimation("Offset", expressionAnimation);
```

### <a name="propertysets-summary-of-expression-definition"></a>Summary of Expression definition

- At a high level, this Expression is simply the sum of three components: A Visual reference, a CompositionPropertySet reference and a Vector3 object construction.

- A scalar property in a CompositionPropertySet named “Rotation” that is being animated by a separate KeyFrameAnimation dictates the core behavior of this Expression.

    - This property “Rotation”, and another property  “CenterPointOffset, will need to be referenced in the equation.

- The Expression also constructs a Vector3 that takes the Cosine of the Radians-converted property “Rotation” in the CompositionPropertySet

### <a name="propertysets-building-with-expressionnodes"></a>Building with ExpressionNodes

Note that, as mentioned earlier in the Tips and Tricks section of the doc, this walkthrough uses a shorthand to refer to the ExpressionFunctionClass as EF:

```csharp
using EF = HelperClasses.ExpressionFunctions;
```

First, we get a reference to the PropertySet and its Rotation and CenterPointOffset properties:

```csharp
var rotation = 
	propertySet.GetReference().GetScalarProperty("Rotation");
var centerPointOffset = 	propertySet.GetReference().GetVector3Property("CenterPointOffset");
```

Now we are ready to put together the full Expression:

```csharp
var orbitExp = visual.GetReference().Offset + centerPointOffset +
               EF.Vector3(EF.Cos(EF.ToRadians(rotation)) * 150, 	
                          EF.Sin(EF.ToRadians(rotation)) * 75, 
                          0f);
```

### <a name="propertysets-final-code-snippet"></a>Final code snippet

```csharp
using EF = HelperClasses.ExpressionFunctions;

var rotation = 
	propertySet.GetReference().GetScalarProperty("Rotation");
var centerPointOffset = 	propertySet.GetReference().GetVector3Property("CenterPointOffset");
var orbitExp = visual.GetReference().Offset + centerPointOffset +
               EF.Vector3(EF.Cos(EF.ToRadians(rotation)) * 150, 	
                          EF.Sin(EF.ToRadians(rotation)) * 75, 
                          0f);
```

## <a name="curtain"></a>Curtain

([Github Project](https://github.com/Microsoft/WindowsUIDevLabs/tree/master/SampleGallery/Samples/SDK%2014393/Curtain))

The third example we will walk through is the Curtain sample on the Windows UI Dev Labs Sample Gallery Github project. Although there are a few instances where Expressions are used, we will focus on the Expression that defines the Spring motion of the curtain (the function named ActivateSpringForce()).

### <a name="curtain-old-expression"></a>Old Expression

Let’s look at the relevant code for how the Expression is built today with strings:

```csharp
var dampingConstant = 5;
var springConstant = 20;

var modifier = InteractionTrackerInertiaMotion.Create(_compositor);

// Set the condition to true (always)
modifier.Condition = _compositor.CreateExpressionAnimation("true");

// Define a spring-like force, anchored at position 0.
modifier.Motion = _compositor.CreateExpressionAnimation(
	@"(-(this.target.Position.Y) * springConstant) - (dampingConstant * 	this.target.PositionVelocityInPixelsPerSecond.Y)");

modifier.Motion.SetScalarParameter("dampingConstant", dampingConstant);
modifier.Motion.SetScalarParameter("springConstant", springConstant);

_tracker.ConfigurePositionYInertiaModifiers(
	new InteractionTrackerInertiaModifier[] { modifier });
```

### <a name="curtain-summary-expression-definition"></a>Summary Expression Definition

- The equation for this Expression is leveraging the force equation used for damped harmonic oscillators: kx – cv, where k is the Spring Constant, x is the displacement of the spring, c is the damping constant and v is the velocity of the spring.

- The main component of this equation is an InteractionTracker and the associated properties of it to drive the damped harmonic oscillator equation.

    - In particular, the properties Position.Y and PositionVelocityInPixelsPerSecond.Y

- Because this Expression is getting properties from the same InteractionTracker it is animating, the Target keyword will be used here.

### <a name="curtain-building-with-expressionnodes"></a>Building with ExpressionNodes

We’ll start with defining out the first Expression, which is the Condition portion of the InertiaMotion Modifier. This is done by using the CompositionExtensions.SetCondition(…) extension method, which is accessed via InterationTrackerInertiaMotion.SetCondition(…).

```csharp
modifier.SetCondition(true);
```

Next, we’ll use the Target keyword to get a reference to the InteractionTracker object that this Expression will be applied to.

```csharp
var target = ExpressionValues.Target.CreateInteractionTrackerTarget();
```

At this point, we are ready to build out the rest of the Expression and set the Motion component of the InertiaModifier, using another extension method CompositionExtensions.SetMotion(…):

```csharp
var motion = (-target.Position.Y * springConstant) - 
	(dampingConstant * target.PositionVelocityInPixelsPerSecond.Y);
modifier.SetMotion(motion);
```

### <a name="curtain-final-code-snippet"></a>Final code snippet

```csharp
var dampingConstant = 5;
var springConstant = 20;

var modifier = InteractionTrackerInertiaMotion.Create(_compositor);

// Set the condition to true (always)
modifier.SetCondition(true);

var target = ExpressionValues.Target.CreateInteractionTrackerTarget();
var motion = (-target.Position.Y * springConstant) - 
	(dampingConstant * target.PositionVelocityInPixelsPerSecond.Y);
modifier.SetMotion(motion);

_tracker.ConfigurePositionYInertiaModifiers(
	new InteractionTrackerInertiaModifier[] { modifier });
```

## <a name="requirements"></a>Requirements

| Device family | Universal, 10.0.15063.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## <a name="api"></a>API

* [Expressions source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI.Animations/Expressions)
