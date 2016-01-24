using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PJA_Skills_032.Model;
using PJA_Skills_032.ParseObjects;
using PJA_Skills_032.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserPage : Page
    {
        private TestUser _user;
        private MyProfileViewModel ViewModel { get; set; }
        public UserPage()
        {
            this.InitializeComponent();
            //ViewModel = new MyProfileViewModel();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _user = e.Parameter as TestUser;
            ViewModel = new MyProfileViewModel(_user);
            base.OnNavigatedTo(e);
        }


        private async void UserPage_OnLoading(FrameworkElement sender, object args)
        {
            await ViewModel.CurrentUser.GetSkills();
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
            if (!string.IsNullOrWhiteSpace(ViewModel.CurrentUser.FacebookLink))
            {
                Uri articleLinkUri = new Uri(uriString: ViewModel.CurrentUser.GooglePlusLink, uriKind: UriKind.Absolute);
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
