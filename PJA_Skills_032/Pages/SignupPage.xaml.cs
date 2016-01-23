using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using PJA_Skills_032.Model;
using PJA_Skills_032.ParseObjects;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignupPage : Page
    {
        // User fields:
        public ObservableCollection<Skill> UserLearnList = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> UserTeachList = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> UserKorkingList = new ObservableCollection<Skill>();


        public ObservableCollection<Skill> SearchSuggestionsList = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> ResultsSearchSuggestions = new ObservableCollection<Skill>();
        private ParseUser _user = new ParseUser();

        public SignupPage()
        {
            this.InitializeComponent();

            AutoSuggestBoxLearn.ItemsSource = ResultsSearchSuggestions;

            GridViewLearn.ItemsSource = UserLearnList;
        }

        private async void SignupPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            List<Skill> allSkillsList = await ParseHelper.GetAllSkillsList();
            SearchSuggestionsList = new ObservableCollection<Skill>(allSkillsList);
        }

        private async void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
        {
            _user.Username = TxtEmail.Text;
            _user.Password = TxtPassword.Password;
            _user.Email = TxtEmail.Text;


            // other fields can be set just like with ParseObject
            _user[ParseHelper.OBJECT_TEST_USER_FACULTY] = "Informatyka"; // TODO: mock
            _user[ParseHelper.OBJECT_TEST_USER_NAME] = TxtFullName.Text;


            // Sign up
            try
            {
                await _user.SignUpAsync();
            }
            catch (Exception)
            {
                var dialog = new MessageDialog("Please, fill all forms or try another email");
                await dialog.ShowAsync();
                return;

            }


            // Login
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

            // Add skill to created user

            await AddSkillsToUser();

            Frame.Navigate(typeof(MyProfilePage));
        }

        public async Task AddSkillsToUser()
        {
            // Add learn skills
            foreach (Skill skill in UserLearnList)
            {
                await ParseHelper.AddSkillToUser(_user, skill.getBackingObject);
            }

            // Add teach skills


            // Add korking skills
        }

        private void AutoSuggestBoxLearn_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            List<Skill> searchSuggestions =
                new List<Skill>(SearchSuggestionsList.
                Where(item => (item.Name).ToLower().Contains
                (AutoSuggestBoxLearn.Text.ToLower())));

            ResultsSearchSuggestions.Clear();

            // Remove already added by user (to avoid dublicates)
            foreach (Skill skill in searchSuggestions.ToList())
            {
                if (skill.IsContainsInOtherList(UserLearnList))
                    searchSuggestions.Remove(skill);
            }

            // Add all matched skills
            foreach (Skill skill in searchSuggestions)
            {
                ResultsSearchSuggestions.Add(skill);
            }
        }

        private void AutoSuggestBoxLearn_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Skill selectedSkill = args.SelectedItem as Skill;
            if (selectedSkill != null)
            {
                UserLearnList.Add(selectedSkill);

            }

            AutoSuggestBoxLearn.Text = "";
        }

    }
}
