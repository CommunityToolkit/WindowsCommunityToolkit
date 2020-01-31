using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
	public partial class UnoPlatformShell : Control
	{
		private Button _UnoButton;
		private Button _appButton;
		private Button _authorButton;
		private Button _TwitterButton;
		private Button _GithubButton;
		private Button _openAboutButton;
		private Button _closeAboutButton;
		private Button _softDismissAboutButton;
		private Button _visitUnoWebsiteButton;

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_UnoButton = GetTemplateChild("UnoButton") as Button;
			_UnoButton.Click += openUnoWebsite;

			_appButton = GetTemplateChild("appButton") as Button;
			_appButton.Click += openOriginalGitHubProject;

			_authorButton = GetTemplateChild("authorButton") as Button;
			_authorButton.Click += openAuthorGitHubProfile;

			_TwitterButton = GetTemplateChild("TwitterButton") as Button;
			_TwitterButton.Click += openTwitterLink;

			_GithubButton = GetTemplateChild("GithubButton") as Button;
			_GithubButton.Click += openUnoGitHubProject;

			_openAboutButton = GetTemplateChild("openAboutButton") as Button;
			_openAboutButton.Click += showAbout;

			_closeAboutButton = GetTemplateChild("closeAboutButton") as Button;
			_closeAboutButton.Click += hideAbout;

			_softDismissAboutButton = GetTemplateChild("softDismissAboutButton") as Button;
			_softDismissAboutButton.Click += hideAbout;

			_visitUnoWebsiteButton = GetTemplateChild("visitUnoWebsiteButton") as Button;
			_visitUnoWebsiteButton.Click += openUnoWebsite;
		}

		// openOriginalGitHubProject
		private async void openOriginalGitHubProject(object sender, RoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri(LinkToOriginalApp));
		}

		// openUnoGitHubProject
		private async void openUnoGitHubProject(object sender, RoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri(LinkToUnoPlatformApp));
		}

		// openAuthorGitHubProfile
		private async void openAuthorGitHubProfile(object sender, RoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri(LinkToAppAuthor));
		}

		// openTwitterLink
		private async void openTwitterLink(object sender, RoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri("https://twitter.com/UnoPlatform"));
		}

		// showAbout
		private void showAbout(object sender, RoutedEventArgs e)
		{
			AboutVisibility = Visibility.Visible;
		}

		// hideAbout
		private void hideAbout(object sender, RoutedEventArgs e)
		{
			AboutVisibility = Visibility.Collapsed;
		}

		// openUnoWebsite
		private async void openUnoWebsite(object sender, RoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri("https://platform.uno"));
		}

		// App Name
		public string AppName
		{
			get { return (string)GetValue(AppNameProperty); }
			set { SetValue(AppNameProperty, value); }
		}

		public static readonly DependencyProperty AppNameProperty =
			DependencyProperty.Register("AppName", typeof(string), typeof(UnoPlatformShell), new PropertyMetadata(null));

		// App Author
		public string AppAuthor
		{
			get { return (string)GetValue(AppAuthorProperty); }
			set { SetValue(AppAuthorProperty, value); }
		}

		public static readonly DependencyProperty AppAuthorProperty =
			DependencyProperty.Register("AppAuthor", typeof(string), typeof(UnoPlatformShell), new PropertyMetadata(null));

		// Link to Original App
		public string LinkToOriginalApp
		{
			get { return (string)GetValue(LinkToOriginalAppProperty); }
			set { SetValue(LinkToOriginalAppProperty, value); }
		}

		public static readonly DependencyProperty LinkToOriginalAppProperty =
			DependencyProperty.Register("LinkToOriginalApp", typeof(string), typeof(UnoPlatformShell), new PropertyMetadata(null));

		// Link to Original App
		public string LinkToAppAuthor
		{
			get { return (string)GetValue(LinkToAppAuthorProperty); }
			set { SetValue(LinkToAppAuthorProperty, value); }
		}

		public static readonly DependencyProperty LinkToAppAuthorProperty =
			DependencyProperty.Register("LinkToAppAuthor", typeof(string), typeof(UnoPlatformShell), new PropertyMetadata(null));

		// Link to Uno Platforml App
		public string LinkToUnoPlatformApp
		{
			get { return (string)GetValue(LinkToUnoPlatformAppProperty); }
			set { SetValue(LinkToUnoPlatformAppProperty, value); }
		}

		public static readonly DependencyProperty LinkToUnoPlatformAppProperty =
			DependencyProperty.Register("LinkToUnoPlatformApp", typeof(string), typeof(UnoPlatformShell), new PropertyMetadata(null));

		// VersionNumber
		public string VersionNumber
		{
			get { return (string)GetValue(VersionNumberProperty); }
			set { SetValue(VersionNumberProperty, value); }
		}

		public static readonly DependencyProperty VersionNumberProperty =
			DependencyProperty.Register("VersionNumber", typeof(string), typeof(UnoPlatformShell), new PropertyMetadata(null));

		// About Content
		public object AboutContent
		{
			get { return (object)GetValue(AboutContentProperty); }
			set { SetValue(AboutContentProperty, value); }
		}

		public static readonly DependencyProperty AboutContentProperty =
			DependencyProperty.Register("AboutContent", typeof(object), typeof(UnoPlatformShell), new PropertyMetadata(null));

		// App Content
		public object AppContent
		{
			get { return (object)GetValue(AppContentProperty); }
			set { SetValue(AppContentProperty, value); }
		}

		public static readonly DependencyProperty AppContentProperty =
			DependencyProperty.Register("AppContent", typeof(object), typeof(UnoPlatformShell), new PropertyMetadata(null));


		// AboutIsVisible
		public Visibility AboutVisibility
		{
			get { return (Visibility)GetValue(AboutVisibilityProperty); }
			set { SetValue(AboutVisibilityProperty, value); }
		}

		public static readonly DependencyProperty AboutVisibilityProperty =
			DependencyProperty.Register("AboutVisibility", typeof(Visibility), typeof(UnoPlatformShell), new PropertyMetadata(Visibility.Collapsed));
	}
}
