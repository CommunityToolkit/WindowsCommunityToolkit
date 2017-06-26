# ExpressionBuilder 
Welcome to the ExpressionBuilder classes! The ExpressionBuilder classes are a C#-only alternative to building Expressions with type safety. Below is a quick introduction to using the ExpressionBuilder classes with your application. For complete documentation and walkthroughs, please see the Word Document in the project folder titled "ExpressionBuilder_Documentation.docx".

## Setting up the ExpressionBuilder classes with your app
To use the ExpressionBuilder in your app, download a copy of the source, add the project into your solution and update the references for your app project. Next, within your app project, make sure to add the using statement to leverage the ExpressionBuilder classes:

```
using  ExpressionBuilder;
``` 

Once you have the classes added to your solution and referenced in your project, you are all set to start using the ExpressionBuilder classes!

## Getting started with ExpressionBuilder classes
### ExpressionAnimation Overview
A brief recap of ExpressionAnimations:
- ExpressionAnimations are a type of CompositionAnimations used to create mathematical relationships between Composition Objects. Simple examples include making a relationship such that one object will move relative to another. 
- Like other CompositionAnimations, ExpressionAnimations are templates, meaning you can create an Expression and use it to animate multiple objects. You can also change aspects of the animation and have those changes take effect the next time you animate an object (without affecting any previously connected animations). 
- For more information on ExpressionAnimations, [please check our documentation](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Composition.ExpressionAnimation).

ExpressionAnimations can create some very powerful and unique experiences, but can be a bit combersome to author. One of the big pain points with ExpressionAnimations is that the equation or mathematical relationship that defines the animation is written as a string, e.g.:

```
_parallaxExpression = compositor.CreateExpressionAnimation(
	"(ScrollManipulation.Translation.Y + StartOffset - (0.5 * ItemHeight)) * ParallaxValue - 	(ScrollManipulation.Translation.Y + StartOffset - (0.5 * ItemHeight))");
``` 
This creates a series of challenges when authoring Expressions in this manner:
- No type safety checks
- No intellisense or autocomplete
- Semantic errors with the equation appear at runtime, not compile time

Thus, the ExpressionBuilder classes were created to help alleviate these challenges and present an alternative way to create ExpressionAnimations.

## Using the ExpressionBuilder classes
For full documentation on how to use the ExpressionBuilder classes, please refer to the Word document that is included within the project folder.

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

### Extension Methods (GetReference(), StartAnimation())
Prior to ExpressionBuilder, in order to reference a CompositionObject property, a SetReferenceParameter on the ExpressionAnimation must always be called:
```
var expression = _compositor.CreateExpressionAnimation("visualA.Offset.X + 50");
expression.SetReferenceParameter("visualA", _visualA);
_visualB.StartAnimation("Offset.X", expression);
```
With ExpressionBuilder, you can use the GetReference() extension method that performs this same behavior if you don't need to template, but in a type safe manner:
```
_visualB.StartAnimation("Offset.X", _visualA.GetReference().Offset.X + 50f);
```
Also notice in the above code snippet, the CompositionObject.StartAnimation() extension method  was used to pass in an ExpressionNode instead of an ExpressionAnimation.

### How to template with ExpressionBuilder
Templating is a big value prop of CompositionAnimations. As a developer, you define a template for an animation that you then can create multiple instances of later when binding to CompositionObjects via StartAnimation(). In some cases, when templating, you want to change the value of parameters you define. For example, changing which Visual you want to reference, or changing the value of a constant. This means that parameters must be able to be referenced later on so their reference or value can be changed; for this reason, parameters are defined with a string property name.

In the following code snippet, we update the Expression defined earlier:
- Make the Visual we reference a parameter so it can be changed at a later time
- Create a constant parameter instead of hardcoding the value “50f”, so this can easily be changed at a later time

```
var additionOffset = ExpressionValues.Constant.CreateScalarConstant("addOffset", 50f);
var expressionNode = ExpressionValues.Reference.CreateVisualReference("visualA", _visualA) + addOffset;
[...]
// If want to change what "visualA" references and value of "addOffset" in the Expression template ...
expressionNode.SetReferenceParameter("visualA", _visualC);
expressionNode.SetScalarParameter("addOffset", 100f);
```  
### E2E Example
Let's walk through the expression used in the PullToAnimate sample to animate Opacity with InteractionTracker
```
// Expression written with strings
var progressExp = _compositor.CreateExpressionAnimation();
progressExp.Expression = "Clamp(tracker.Position.Y / tracker.MaxPosition.Y, 0, 1)";
progressExp.SetReferenceParameter("tracker", _tracker);
visual.StartAnimation("Opacity", progressExp);
```
Now let's show what this looks like with ExpressionBuilder:

```
// Expression written with ExpressionBuilder
var trackerNode = _tracker.GetReference();
var progressExp = EF.Clamp(trackerNode.Position.Y / trackerNode.MaxPosition.Y, 0, 1);
_propertySet.StartAnimation("progress", progressExp);
```

### Things to Note
If you are familiar with how Expressions were built with Strings, there are a few things to note:
- The ternary operator (condition ? ifTrue : ifFalse) is now represented by ExpressionFunctions..Conditional(condition, ifTrue, ifFalse)
- The "And" and "Or" operators (“&&” and “||”) are now represented by the & and | operators.
- If using ExpressionBuilder to create expressions for use with InteractionTracker’s InertiaModifiers, the following extensions methods are available:
	
    - InteractionTrackerInertiaRestingValue.SetCondition
	- InteractionTrackerInertiaRestingValue.SetRestingValue
	- InteractionTrackerInertiaMotion.SetCondition
	- InteractionTrackerInertiaMotion.SetMotion
- Referencing ExpressionValues and ExpressionFunctions in your code can be a bit verbose, so you can define shortened versions in the Using section of your app:

	```
	using EF = ExpressionBuilder.ExpressionFunctions;
	using EV = ExpressionBuilder.ExpressionValues;
	```
