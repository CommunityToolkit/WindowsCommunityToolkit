# Contributing to UWP Community Toolkit

The foundation of **UWP Community Toolkit** is simplicity. 

A developer should be able to quickly and easily learn to use the API. 

Simplicity and a low barrier to entry are must-have features of every API. If you have any second thoughts about the complexity of a design, it is almost always much better to cut the feature from the current release and spend more time to get the design right for the next release. 

You can always add to an API, you cannot ever remove anything from one. If the design does not feel right, and you ship it anyway, you are likely to regret having done so.

That's why many of the guidelines of this document are obvious and serve only one purpose: Simplicity.

## A good pull request
Every contribution has to come with:

* Before starting coding, **you should open an issue** and start discussing with the community to see if your idea/feature is interesting enough
* A documentation page on the [documentation repo](https://github.com/Microsoft/UWPCommunityToolkit-docs)
* A sample for the [Sample app](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp) (If applicable)
* Unit tests (If applicable)
* You tested your code with SDK 10586 and SDK 14393
* PR has to target dev branch

PR has to be validated by at least two core members before being merged.

Once merged, you can get a pre-release package of the toolkit by adding this [Nuget repo](https://ci.appveyor.com/nuget/uwpcommunitytoolkit-dev) to your Visual Studio.

## General rules

* DO NOT require that users perform any extensive initialization before they can start programming basic scenarios.
* DO provide good defaults for all values associated with parameters, options, etc.
* DO ensure that APIs are intuitive and can be successfully used in basic scenarios without referring to the reference documentation.
* DO communicate incorrect usage of APIs as soon as possible. 
* DO design an API by writing code samples for the main scenarios. Only then, you define the object model that supports those code samples.
* DO NOT use regions. DO use partial classes instead.
* DO declare static dependency properties at the top of their file.
* DO NOT seal controls.
* DO use extension methods over static methods where possible.
* DO NOT return true or false to give sucess status. Throw exceptions if there was a failure.
* DO use verbs like GET.
* DO NOT use verbs that are not already used like fetch.

## Naming conventions
* We are following the coding guidelines of [.NET Core Foundational libraries](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md). 

## Documentation
* DO NOT expect that your API is so well designed that it needs no documentation. No API is that intuitive.
* DO provide great documentation with all APIs. 
* DO use readable and self-documenting identifier names. 
* DO use consistent naming and terminology.
* DO provide strongly typed APIs.
* DO use verbose identifier names.

## Files and folders
* DO associate no more than one class per file.
* DO use folders to group classes based on features.
