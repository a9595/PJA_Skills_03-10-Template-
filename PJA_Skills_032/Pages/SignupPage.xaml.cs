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
using PJA_Skills_032.ParseObjects;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignupPage : Page
    {
        public SignupPage()
        {
            this.InitializeComponent();
        }

        private async void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
        {
            ParseUser user = new ParseUser()
            {
                Username = TxtEmail.Text,
                Password = TxtPassword.Password,
                Email = TxtEmail.Text
            };

            // other fields can be set just like with ParseObject
            user[ParseHelper.OBJECT_TEST_USER_FACULTY] = "Informatyka"; // TODO: mock
            user[ParseHelper.OBJECT_TEST_USER_NAME] = TxtFullName.Text; // TODO: mock

            try
            {
                await user.SignUpAsync();
            }
            catch (Exception)
            {
                var dialog = new MessageDialog("Please, fill all forms or try another email");
                await dialog.ShowAsync();
                return;

            }

            try
            {
                await ParseUser.LogInAsync(TxtEmail.Text, TxtPassword.Password);
                // Login was successful.
            }
            catch (Exception)
            {
                var dialog = new MessageDialog("Please, try another email");
                await dialog.ShowAsync();
                return;
            }

            Frame.Navigate(typeof(MyProfilePage));
        }
    }
}
