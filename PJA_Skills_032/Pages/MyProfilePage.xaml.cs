using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Parse;
using PJA_Skills_032.Model;
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
            ViewModel = new MyProfileViewModel();
            this.InitializeComponent();
            //GridViewLearn.ItemsSource = ViewModel.CurrentUser.SkillsWantToLearn;
        }


        private async void MyProfilePage_OnLoading(FrameworkElement sender, object args)
        {
            await ViewModel.CurrentUser.GetSkills();
        }


        private async Task ParseUserCreate()
        {
            // TODO: move login user to App.cs and add the login screen
            // Test SignUp
            //await SignUpUser();

            // Login
            if (ParseUser.CurrentUser == null) // check if user sesions is in cache
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
