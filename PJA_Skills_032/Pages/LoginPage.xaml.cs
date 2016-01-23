using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Parse;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();

        }

        private void LoginPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            ParseUser.LogOut();
        }

        private async void BtnLogin_OnClick(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text;
            string password = TxtPassword.Password;

            try
            {
                await ParseUser.LogInAsync(login, password);

            }
            catch (Exception)
            {
                var dialog = new MessageDialog("Wrong Username or Password");
                await dialog.ShowAsync();
                return;
            }

            Frame.Navigate(typeof(MyProfilePage));
        }

        private async void BtnFacebook_OnClick(object sender, RoutedEventArgs e)
        {
            StackPanelLoginFields.Visibility = Visibility.Collapsed;
            WebView.Visibility = Visibility.Visible;

            ParseUser user = await ParseFacebookUtils.LogInAsync(WebView, null);

            WebView.Visibility = Visibility.Collapsed;
            StackPanelLoginFields.Visibility = Visibility.Visible;

            ParseUser currentUser = ParseUser.CurrentUser;


        }

       

        private void WebView_OnDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            
        }

        private void BtnSignup_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (SignupPage));
        }
    }
}
