# Contributing to the Windows Community Toolkit :sparkles::sparkles:

Welcome to the Contributing page of the Windows Community Toolkit :tada:

The entire Windows Community Toolkit team would like to say Congratulations for reaching this far and contributing to one of the most exciting and growing projects you have probably encountered. We are beyond excited for you to participate in the community. It is because of the valuable contributors like you that keep the Windows Community Toolkit thriving. Whether you fix any small bug or a major one, any contribution will go a long way to enhance the project. Our team will always be here if you may require any assistance regarding steps to submit the PR, review your work and any project related questions that you are unclear about.

Once again Welcome aboard and have fun in this adventurous journey of Windows Community Toolkit :raised_hands:

In the next few steps you will be able to learn how to contribute to this amazing community, but before you proceed any further it is good to understand what Widows Community Toolkit is all about. 

The foundation of the **Windows Community Toolkit** is simplicity. 

A developer should be able to quickly and easily learn to use the API.

Simplicity and a low barrier to entry are must-have features of every API. If you have any second thoughts about the complexity of the design, it is almost always much better to cut the feature from the current release and spend more time to get the design right for the next release.

You can always add to an API, you cannot ever remove anything from one. If the design does not feel right, and you ship it anyway, you are likely to regret having done so.

That's why many of the guidelines of this document are obvious and serve only one purpose: Simplicity.

Here is how you can contribute to the Windows Community Toolkit. 

- [Questions](#question)
- [Find and Fix a Bug](#issue)
- [Creating a Pull Request](#Create-PR)
- [Submitting a pull request](#Submit-PR)
- [Contributing New Feature](#Feature)
- [Adding Documentation](#Adding-Docs)
- [Improving Documentation](#Improve-Docs)
- [Reviewing PR](#Review-PR)
- [Quality assurance for pull requests for XAML controls](#xaml)
- [General rules](#rules)
- [Accessibility](#accessibility)
- [Naming conventions](#naming)
- [Documentation](#documentation)
- [Files and folders](#files)

## <a name="question"></a> Questions :grey_question:
It is critical to understand the kind of question you might have. Since Windows Community Toolkit receives volumes of issues and feature requests regularly; we want to make sure that every question receives an answer as soon as possible. Therefore, please do not open issues for general support questions and keep our GitHub issues for bug reports and feature requests. 

There is a much better chance of getting your question answered on [StackOverflow](https://stackoverflow.com/questions/tagged/windows-community-toolkit) where questions should be tagged with the tag windows-community-toolkit. 

If there is any question related to missing documentation, please file an issue instead at [Microsoft Docs](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/issues/new).

## <a name="issue"></a> Find and Fix a Bug :bug:
Windows Community Toolkit receives lots of bug report regularly and one of the best ways to contribute to this amazing community is by finding and fixing bugs. If you find any bug, you can help the community by [submitting an issue](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/new/choose). Once you submit the issue feel free to start working on the PR and submit a PR. Due to high quantity of requests our community still make certain that these issues are resolved as soon as possible. Therefore, It is imperative that these issues filed receives top priority and it would be even better if you can fix a bug :rocket:

Your contribution of discovering and fixing the bug will be a great achievement to set your footprints in this incredible project. Therefore, any support from the community members will be greatly appreciated and contributed towards the growth of the project. 

### Avoid Roadblocks :construction:
The issue you file must fulfill the requirements of [Bug report](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/.github/ISSUE_TEMPLATE/bug_report.md) Template to provide greater insight and understanding of the issue. By providing detailed information the issue will be investigated and resolved in a timely manner. 

It is also essential to not create any duplicate issues by following “Things to consider before submitting any bug report requirement” This will mitigate the risk of duplicity and is critical. 

### Not sure where to start? 
If this is your first time contributing to the Windows Community Toolkit and do not have advanced level programing experience, we have got you covered :boom: WCT have a list of [good first issue](https://github.com/windows-toolkit/WindowsCommunityToolkit/labels/good%20first%20issue%20%3Aok_hand%3A) that can be a great entry way to find and fix any issues that best fits your expertise or technical background.

### Feeling Pro? 
Besides working on any [issues](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues) that you find suitable and may fit your understanding there are also some issues that are labeled as [help wanted](https://github.com/windows-toolkit/WindowsCommunityToolkit/labels/help%20wanted%20%3Araising_hand%3A). The level of complexity in this list of issues can vary but if you have an advanced level of experience you can also jump in to solve these issues.

It is significant to work collaboratively which makes this amazing platform so unique. This a place where you can meet new people and work together towards building and expanding WCT. The community is encouraged and motivated to fuel the WCT by looking into innovative ideas and be able to provide solutions.

### Things to consider before submitting any bug report :thinking:
:bug:	Please make certain to reproduce the issue. It is essential to Identify the issue and gather more data to be certain if it is a bug or not. It might be a simple fix and can be resolved earlier than expected. 

:bug:	It is essential to open the issue in an appropriate Windows Community Toolkit repository by clicking on [Windows Toolkit](https://github.com/windows-toolkit) page and identify the appropriate repository that the bug applies. 

:bug:	Do Not open a new issue if it already exists in the repository. Go to the current [open issues](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues) of the repository, and perform a search by applying the keyword in the search area. This method should query the result and provide the results of relevant open issues. 

## <a name="Create-PR"></a> Creating a Pull Request :rocket:
If this is your first time contributing towards Windows Community Toolkit and never created a PR before, do not worry because you are not in this alone. By following the guidelines below you will be able to create your first Pull Request. 

Anyone with write access can create a Pull Request by forking the Windows Community Toolkit Repository. Here is how you can [Create a Pull Request from fork](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request-from-a-fork).

Anyone with read access can create a Pull Request without forking the Windows Community Toolkit Repository. Here is how you can directly [Create a Pull Request](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request) on the main repository.

It is essential to create changes on different branch especially if it is major change. 

Once the PR is created and submitted, we will be able to see the associated PR issue and will assist and provide feedback if necessary.

If you are not certain about the Pull Request submitted; our team along with the community members are here to review the work and highlight any changes the PR might require to fulfill its obligation. 

## <a name="Submit-PR"></a> Submitting a Pull Request :rocket:
For every contribution, you must:

*	test your code with the [supported SDKs](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/readme.md#supported)
*	follow the [quality guidance](#xaml), [general rules](#rules), and [naming convention](#naming)
*	target master branch (or an appropriate release branch if appropriate for a bug fix)
*	Follow the Windows Community Toolkit [PR Template](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/.github/PULL_REQUEST_TEMPLATE.md) 
*	If adding a new feature
o	Before starting coding, you should open an [issue](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/new?assignees=&labels=feature+request+%3Amailbox_with_mail%3A&template=feature_request.md&title=%5BFeature%5D) and start discussing with the community to see if your idea/feature is interesting enough.
o	Add or update a sample for the [Sample app](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp)
	If creating a new sample, create a new icon by following the [Thumbnail Style Guide and templates](https://github.com/Microsoft/UWPCommunityToolkit-design-assets)
o	Add or update unit tests (if applicable)

PR must be validated by at least two core members before being merged.

Please make certain that the PR successfully passes all three status check requirements including Toolkit-CI, WIP, license/cla. If it fails and you are aware of the reasoning please fix the error. If you are unaware of the reasoning please escalate by commenting in the PR, and someone from the team will investigate the build error. 

If the PR passes all the requirements above and reviewers have signed off the PR will be merged. 

Once merged, you can get a pre-release package of the toolkit by adding this ([Nuget repo](https://dotnet.myget.org/F/uwpcommunitytoolkit/api/v3/index.json) | [Gallery](https://dotnet.myget.org/gallery/uwpcommunitytoolkit)) to your Visual Studio.

## <a name="Feature"></a> Contributing new Feature :mailbox_with_mail:
The best way to support the Windows Community Toolkit is by contributing a new feature. As Windows Community Toolkit is growing and being utilized by many partners and businesses; the demand for new features has increased tremendously. 

*	To contribute a new feature you can simply create a proposal by selecting [Feature Request](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/new/choose) from the list.
*	Fill out the template by providing detailed information to express your proposal. Remember this is the place where the community is looking at something that might like and vote to implement in the project. 
*	Once the Feature Request is submitted, it will be open for discussion regarding what solution does this feature provide and how it lives up to the ideology of Windows Community Toolkit. If it gets approved by the team, Congratulations! you are just few steps to successfully contributing this amazing feature. 
*	Proceed to submit a PR of the proposed Feature. 
*	If the PR contains an error free code and reviewer signs off, the PR will be merged. You may continue to enhance this feature.
*	Kudos :medal_sports: you are now officially a contributor to this amazing community. 

### <a name="Adding-Docs"></a> Adding Documentation :page_with_curl:
Documentation is **required** when adding, removing, or updating a control or an API. To update the documentation, you must submit a separate Pull Request in the [WindowsCommunityToolkitDocs](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs) repository (use the master branch). **Both Pull Requests (code and docs) must be approved by the core team before either one is merged**.

Make sure to update both Pull Requests with a link to each other.

If adding a new documentation page:

*	Copy the [documentation template](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/blob/master/docs/.template.md) and follow the same format.
*	Update the [Table of Contents](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/blob/master/docs/toc.md) to point to the new page

## <a name="Improve-Docs"></a> Improving Documentation :page_with_curl:
As the community is always working on improving Windows Community Toolkit and introducing new features, we always make certain that our documentation is also updated and meets the requirement of all the current features. Therefore, improving documentation will be a great way to contribute to this project. 

Currently all the Windows Community Toolkit Docs are being hosted on [WindowsCommunityToolkitDocs](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs) repository and the [Doc PR template](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/blob/master/.github/PULL_REQUEST_TEMPLATE.md) is being followed by the contributors to propose any changes to update the docs for Windows Community Toolkit. 

### How does the Doc process works? 
When opening a Pull Request, start by forking this repository. Then, based on the type of change you're making you'll need to create a new branch from either the `master` or `live` branches:

For documentation for new features, please base your fork off the master branch.

If you have a typo or existing document improvement to an already shipped feature, please base your change off of the [live branch](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/tree/live). This will allow us to get the change to the published documentation between releases.

We will periodically merge updates from the live branch to master to keep master in-sync with the published docs. When we make a new release, we will push master to the live branch in order to publish documentation for new features.

After following the process above, submit the PR and depending on the improvement you are proposing the changes will eventually appear in these links.

- [Staging review from 'master' branch](https://review.docs.microsoft.com/windows/communitytoolkit/?branch=master) **This link is currently only available for Microsoft Employees**

- [Live site from 'live' branch](https://docs.microsoft.com/windows/communitytoolkit)

## <a name="Review-PR"></a> Reviewing PR :book:
Reviewing PR is essential before merging any changes regarding bug fixes, features, doc improvements etc. You can find the current list of PR’s [here](https://github.com/windows-toolkit/WindowsCommunityToolkit/pulls). 

Pre-requisites: Download Visual Studio 2017 or 2019, Install [Windows Community Toolkit Sample App](http://aka.ms/uwptoolkitapp), [Install Git](https://github.com/github/hub#installation), [Install Hub](https://hub.github.com/#install) 

Steps to review PR
 
*	Go to [Windows Community Toolkit](https://github.com/windows-toolkit/WindowsCommunityToolkit) repository and click on **“Clone or Download”** button to copy the URL.
*	Open Command Prompt.
*	Locate the folder where you want the clone repo to appear by using cd path.
*	Paste the link in Command Prompt by following the [Hub Command](https://hub.github.com/#developer) to Clone Windows Community Toolkit.
*	[Fork](https://hub.github.com/#contributor) the repo by following the command.
*	Checkout the PR by using [hub pr checkout](https://hub.github.com/hub-pr.1.html#synopsis) Command.
*	Open the Visual Studio. 
*	Click on “Open a project or solution” 
*	Locate the cloned repository folder in the local machine and select .sln file to open the solution.
*	You will see the PR checkout branch in the bottom right corner of the Visual Studio screen. (By default it should have been on master branch but since the PR checkout command has been performed; therefore, it’s on the selected branch that is ready to be tested).
*	Now run the Microsoft.Toolkit.Uwp.SampleApp in Viusal Studio. 
* Open the downloaded [Windows Community Toolkit Sample App](http://aka.ms/uwptoolkitapp) as well. 
*	Review and test the changes side by side.
*	Once approved signoff by leaving feedback and results. 

  :bulb:	List of important [Hub Commands](https://hub.github.com/hub.1.html#commands) that can be useful to manage any Pull Request. 

Overall, it is essential to make sure the PR has an appropriate branch and it does not contradict with any other changes. If it has multiple changes then make certain it is clearly stated in the [PR Template](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/.github/PULL_REQUEST_TEMPLATE.md) with the detailed information and all the requirements of the PR checklist has been fulfilled.

It is also significant to watch if the PR contains any breaking changes or not. If the PR contains a breaking change, check for the detailed description of the impact and migration path for existing application.

:rotating_light:	Breaking changes are likely to be rejected. 

## <a name="xaml"></a> Quality assurance for pull requests for XAML controls
We encourage developers to follow the following guidelines when submitting pull requests for controls:

*	Your control must be usable and efficient with the keyboard only.
*	Tab order must be logical.
*	Focused controls must be visible.
*	Action must be triggered when hitting the Enter key.
*	Do not use custom colors but instead rely on theme colors so high contrasts themes can be used with your control.
*	Add AutomationProperties.Name on all controls to define the control’s purpose (Name is minimum, but there are some other things too that can help the screen reader).
*	Don't use the same Name on two different elements unless they have different control types.
*	Use Narrator Dev mode (Launch Narrator [WinKey+Enter], then CTRL+F12) to test the screen reader experience. Is the information sufficient, meaningful and helps the user navigate and understand your control.
*	Ensure that you have run your XAML file changes through Xaml Styler (version 2.3+), which can be downloaded from [here](https://visualstudiogallery.msdn.microsoft.com/3de2a3c6-def5-42c4-924d-cc13a29ff5b7). Do not worry about the settings for this as they are set at the project level (settings.xamlstyler).

You can find more information about these topics [here](https://blogs.msdn.microsoft.com/winuiautomation/2015/07/14/building-accessible-windows-universal-apps-introduction).

This is to help as part of our effort to build an accessible toolkit (starting with 1.2)

## <a name="rules"></a> General rules :warning: 
*	DO NOT require that users perform any extensive initialization before they can start programming basic scenarios.
*	DO provide good defaults for all values associated with parameters, options, etc.
*	DO ensure that APIs are intuitive and can be successfully used in basic scenarios without referring to the reference documentation.
*	DO communicate incorrect usage of APIs as soon as possible.
*	DO design an API by writing code samples for the main scenarios. Only then, you define the object model that supports those code samples.
*	DO NOT use regions. DO use partial classes instead.
*	DO declare static dependency properties at the top of their file.
*	DO NOT seal controls.
*	DO use extension methods over static methods where possible.
*	DO NOT return true or false to give success status. Throw exceptions if there was a failure.
*	DO use verbs like GET.
*	DO NOT use verbs that are not already used like fetch.

## <a name="accessibility"></a> Accessibility Guideline :wheelchair:
We'll follow this guideline to ensure the basic accessibility features for each control.

### Color & High Contrast themes
*	Controls must support the 4 high contrast themes by default on Windows, in addition to changing the theme while the app is running.
*	Controls must have a contrast ratio of at least 4.5:1 between the text (and images with text) and the background behind it.

### Keyboard
*	Controls must support keyboard navigation (tabs and arrow keys), the tab order must be the same as the UI and non-interactive elements mustn't be focusable.
*	Composite elements must ensure proper inner navigation among the contained elements
*	Clickable UI elements must be invokable with the keyboard (The trigger keys are enter and space).
*	Focusable elements must have a visual focus indicator. It's usually a rectangle shape around the control's normal bounding rectangle.

### Narrator
*	Controls must support the narrator.

## <a name="naming"></a> Naming conventions
*	We are following the coding guidelines of [.NET Core Foundational libraries](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md).

## <a name="documentation"></a> Documentation :page_with_curl:
*	DO NOT expect that your API is so well designed that it needs no documentation. No API is that intuitive.
*	DO provide great documentation with all APIs.
*	DO use readable and self-documenting identifier names.
*	DO use consistent naming and terminology.
*	DO provide strongly typed APIs.
*	DO use verbose identifier names.

## <a name="files"></a> Files and folders :card_file_box:
*	DO associate no more than one class per file.
*	DO use folders to group classes based on features.

