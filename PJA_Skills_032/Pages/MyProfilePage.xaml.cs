
using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Parse;
using PJA_Skills_032.ParseObjects;
using PJA_Skills_032.ViewModel;

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyProfilePage : Page
    {
        public MyProfileViewModel ViewModel { get; set; }
        public MyProfilePage()
        {
            this.InitializeComponent();

            

            //ViewModel = new MyProfileViewModel();

            //GridViewLearn.ItemsSource = ViewModel.CurrentUser.SkillsWantToLearn;
        }


        private async void MyProfilePage_OnLoading(FrameworkElement sender, object args)
        {
            // Bind viemodel to view 
            //if (ViewModel.CurrentUser == null)
            if (ParseUser.CurrentUser == null)
            {
                Frame.Navigate(typeof(LoginPage));
                return;
            }

            ViewModel = new MyProfileViewModel();
            await ViewModel.CurrentUser.GetSkills();
            if (ParseUser.CurrentUser.ContainsKey(ParseHelper.OBJECT_TEST_USER_AVATAR))
            {
                ParseFile avatarFile = ParseUser.CurrentUser.Get<ParseFile>(ParseHelper.OBJECT_TEST_USER_AVATAR);
                if (avatarFile.Url != null) UserAvatar.Source = new BitmapImage(avatarFile.Url);
            }
        }


        private void AppBarButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditMyProfilePage), ViewModel.CurrentUser);

        }

        private async void FacebookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.CurrentUser.FacebookLink))
            {
                Uri articleLinkUri = new Uri(uriString: ViewModel.CurrentUser.FacebookLink, uriKind: UriKind.Absolute);
                await Launcher.LaunchUriAsync(articleLinkUri);
            }
        }

        private async void GplusBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.CurrentUser.GooglePlusLink))
            {
                Uri articleLinkUri = new Uri(ViewModel.CurrentUser.GooglePlusLink, UriKind.Absolute);
                await Launcher.LaunchUriAsync(articleLinkUri);
            }
        }


        private async void SkypeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.CurrentUser.SkypeLink))
                return;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
                            dataPackage.SetText(ViewModel.CurrentUser.SkypeLink);
                            Clipboard.SetContent(dataPackage);
                        });

            var dialog = new MessageDialog(content: ViewModel.CurrentUser.SkypeLink, title: "Skype nick is copied to clipboard");
            await dialog.ShowAsync();
        }

        private void EmailBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string email = ViewModel.CurrentUser.Email;
            ParseHelper.ComposeEmail(email, "Hi, found you in the app PJA Skills. Nice to meet you, mate :)", null);

        }
    }
}
