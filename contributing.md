# Contributing to the Windows Community Toolkit :sparkles::sparkles:


The Windows Community Toolkit team is delighted to welcome you for exhibiting interest in contributing to one of the most exhilarating and growing projects. We are beyond excited for you to participate in the community. It is because of the valuable contributors like you that keep the Windows Community Toolkit thriving. Whether you fix any small bug or a major one, all contribution will go a long way to enhance the project.

In the next few steps you will be able to see the glimpse of ways to contribute in this amazing community but for complete and detail oriented content please visit [Windows Community Toolkit Wiki](https://github.com/windows-toolkit/WindowsCommunityToolkit/wiki) page :sparkles: :sparkles:

Here is a list of ways you can contribute to the Windows Community Toolkit.


## Questions :grey_question:
Windows Community Toolkit receives volumes of issues regularly and due to the large pool of requests it is extremely difficult to prioritize answers. Therefore, please do not open issues for general support questions and keep our GitHub issues for bug reports and feature requests. 
 
There is a much better chance of getting your question answered on [StackOverflow](https://stackoverflow.com/questions/tagged/windows-community-toolkit) where questions should be tagged with the tag windows-community-toolkit. 

For missing documentation related question, please file an issue at [Microsoft Docs](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/issues/new).

## Fix a Bug :bug:
If you find any bug, you can help the community by [submitting an issue](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/new?assignees=&labels=bug+%3Abug%3A&template=bug_report.md&title=). Once you submit the issue feel free to start working on the PR and submit a PR. Due to high quantity of requests our community still make certain that these issues are resolved as soon as possible. Therefore, It is imperative that these issues filed receives top priority and it would be even better if you can fix a bug :rocket:

### Things to consider before submitting any bug report :thinking:

Please make certain to reproduce the issue. It is essential to identify the issue and gather more data to be certain if it is a bug or not. It might be a simple fix and can be resolved earlier than expected.

It is essential to open the issue in an appropriate Windows Community Toolkit repository by clicking on [Windows Toolkit](https://github.com/windows-toolkit) page and identify the appropriate repository that the bug applies.

Do Not open a new issue if the Duplicate issue already exists in the repository. Go to the current [open issues](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues) of the repository, and perform a search by applying the keyword in the search area. This method should query the result and provide the results of relevant open issues.

The issue you file must fulfill the requirements of [Bug report Template](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/.github/ISSUE_TEMPLATE/bug_report.md) to provide greater insight and understanding of the issue. By providing detailed information the issue will be investigated and resolved in a timely manner.

### Good First Issue :ok_hand: 
If this is your first time contributing to the Windows Community Toolkit and do not have advanced level programing experience, we have got you covered :boom: WCT have a list of [good first issue](https://github.com/windows-toolkit/WindowsCommunityToolkit/labels/good%20first%20issue%20%3Aok_hand%3A) that can be a great entry way to find and fix any issues that best fits your expertise or technical background.


### Help Wanted :raising_hand: 
Besides working on any [issues](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues) that you find suitable and may fit your understanding there are also some issues that are labeled as [help wanted](https://github.com/windows-toolkit/WindowsCommunityToolkit/labels/help%20wanted%20%3Araising_hand%3A). The level of complexity in this list of issues can vary but if you have an advanced level of experience you can also jump in to solve these issues.


## Create Pull Request :rocket:
Anyone with write access can create a Pull Request by forking the Windows Community Toolkit Repository. Here is how you can [Create a Pull Request from fork](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request-from-a-fork).

Once you fork the Windows Community Toolkit repo, it is essential to create all changes in the feature branch of your forked repository. If you have the changes in the forked feature branch you can create a Pull Request in the main Windows Community Toolkit where your changes will be reviewed to be merged to the Master.

:warning: **We will not merge the PR to the main repo if your changes are not in the feature branch of your forked repository** :warning:


## Submit Pull Request :rocket:
For every contribution, you must:

*	Test your code with the [supported SDKs](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/readme.md#supported)
*	Follow the [quality guidance](#xaml), [general rules](#rules), and [naming convention](#naming)
*	Target master branch (or an appropriate release branch if appropriate for a bug fix)
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

## Contribute new Feature :mailbox_with_mail:
*	To contribute a new feature you can simply create a proposal by selecting [Feature Request](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/new?assignees=&labels=feature+request+%3Amailbox_with_mail%3A&template=feature_request.md&title=%5BFeature%5D) from the list.
*	Fill out the template by providing detailed information to express your proposal. Remember this is the place where the community is looking at something that might like and vote to implement in the project. 
*	Once the Feature Request is submitted, it will be open for discussion regarding what solution does this feature provide and how it lives up to the ideology of Windows Community Toolkit. If it gets approved by the team, Congratulations! you are just few steps to successfully contributing this amazing feature. 
*	Proceed to submit a PR of the proposed Feature. 
*	If the PR contains an error free code and reviewer signs off, the PR will be merged. You may continue to enhance this feature.

### Add Documentation :page_with_curl:
Documentation is **required** when adding, removing, or updating a control or an API. To update the documentation, you must submit a separate Pull Request in the [WindowsCommunityToolkitDocs](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs) repository (use the master branch). **Both Pull Requests (code and docs) must be approved by the core team before either one is merged**.

Make sure to update both Pull Requests with a link to each other.

If adding a new documentation page:

*	Copy the [documentation template](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/blob/master/docs/.template.md) and follow the same format.
*	Update the [Table of Contents](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/blob/master/docs/toc.md) to point to the new page

## Improve Documentation :page_with_curl:
Currently all the Windows Community Toolkit Docs are being hosted on [WindowsCommunityToolkitDocs](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs) repository and the [Doc PR template](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/blob/master/.github/PULL_REQUEST_TEMPLATE.md) is being followed by the contributors to propose any changes to update the docs for Windows Community Toolkit. 

### How does the Doc process works? 
When opening a Pull Request, start by forking this repository. Then, based on the type of change you're making you'll need to create a new branch from either the `master` or `live` branches:

For documentation for new features, please base your fork off the master branch.

If you have a typo or existing document improvement to an already shipped feature, please base your change off of the [live branch](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/tree/live). This will allow us to get the change to the published documentation between releases.

We will periodically merge updates from the live branch to master to keep master in-sync with the published docs. When we make a new release, we will push master to the live branch in order to publish documentation for new features.

After following the process above, submit the PR and depending on the improvement you are proposing the changes will eventually appear in these links.

- [Staging review from 'master' branch](https://review.docs.microsoft.com/windows/communitytoolkit/?branch=master) **This link is currently only available for Microsoft Employees**

- [Live site from 'live' branch](https://docs.microsoft.com/windows/communitytoolkit)

## Review PR :book:

Pre-requisites: Find the current list of PR’s [here](https://github.com/windows-toolkit/WindowsCommunityToolkit/pulls), Download Visual Studio 2017 or 2019, Install [Windows Community Toolkit Sample App](http://aka.ms/uwptoolkitapp), [Install Git](https://github.com/github/hub#installation), [Install Hub](https://hub.github.com/#install) 

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

Overall, it is essential to make sure the PR has an appropriate branch and it does not contradict with any other changes. If it has multiple changes then make certain it is clearly stated in the [PR Template](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/.github/PULL_REQUEST_TEMPLATE.md) with the detailed information and all the requirements of the PR checklist has been fulfilled.

:rotating_light:	Breaking changes are likely to be rejected. 

## Building XAML Controls
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

## Coding Style and Conventions :balance_scale: 
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
