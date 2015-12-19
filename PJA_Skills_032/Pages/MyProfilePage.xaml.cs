using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Parse;
using PJA_Skills_032.Model;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyProfilePage : Page
    {
        public MyProfilePage()
        {
            this.InitializeComponent();
        }


        private async void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ParseUserCreate();  // TODO: login user my Profile

        }

        private async Task ParseUserCreate()
        {
            // Test SignUp
            await SignUpUser();

            // Login
            await TestUser.Login();
            ParseUser currentUser = Parse.ParseUser.CurrentUser;
            string Name = currentUser.Get<string>(ParseHelper.OBJECT_TEST_USER_NAME);


        }

        private async Task SignUpUser()
        {
            var user = new ParseUser()
            {
                Username = "my name",
                Password = "my pass",
                Email = "email@example.com"
            };

            // other fields can be set just like with ParseObject
            //user["phone"] = "415-392-0202";

            await user.SignUpAsync();
        }


    }
}
