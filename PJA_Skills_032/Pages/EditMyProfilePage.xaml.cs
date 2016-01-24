using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
using PJA_Skills_032.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PJA_Skills_032.Pages
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditMyProfilePage : Page
    {
        public ObservableCollection<Skill> SearchSuggestionsList = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> ResultsSearchSuggestions = new ObservableCollection<Skill>();

        // Learn
        public List<Skill> UserLearnList = new List<Skill>(); // keep added skills. saved after tapping "save changes"
        public List<Skill> UserRemoveLearnList = new List<Skill>();

        // Teach
        public List<Skill> UserTeachList = new List<Skill>();
        public List<Skill> UserRemoveTeachList = new List<Skill>();

        // Korking
        public List<Skill> UserKorkingList = new List<Skill>();
        public List<Skill> UserRemoveKorkingList = new List<Skill>();


        //private  _allSkillsList;

        public MyProfileViewModel ViewModel { get; set; }
        public EditMyProfilePage()
        {
            this.InitializeComponent();

            //PopulateSearchSuggestions();

            AutoSuggestBox.ItemsSource = ResultsSearchSuggestions;
            AutoSuggestBoxTeach.ItemsSource = ResultsSearchSuggestions;
        }
        private async void EditMyProfilePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            List<Skill> allSkillsList = await ParseHelper.GetAllSkillsList();
            SearchSuggestionsList = new ObservableCollection<Skill>(allSkillsList);
        }

        //public void PopulateSearchSuggestions()
        //{
        //    SearchSuggestionsList.Add(
        //        new SearchSuggestion("PRM", 1)
        //        );

        //    SearchSuggestionsList.Add(
        //        new SearchSuggestion("PRI", 2)
        //        );

        //    SearchSuggestionsList.Add(
        //        new SearchSuggestion("BSI", 3)
        //        );
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //MyProfileViewModel myProfileViewModel = e.Parameter as MyProfileViewModel;
            ViewModel = new MyProfileViewModel();

            base.OnNavigatedTo(e);
        }

        private async void EditMyProfilePage_OnLoading(FrameworkElement sender, object args)
        {
            await ViewModel.CurrentUser.GetSkills();
        }



        private void GridViewLearn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridViewSkillSelected(GridViewLearn.SelectedItem, UserRemoveLearnList, ViewModel.CurrentUser.SkillsWantToLearn);
        }
        private void GridViewTeach_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridViewSkillSelected(GridViewTeach.SelectedItem, UserRemoveTeachList, ViewModel.CurrentUser.SkillsWantToTeach);
        }

        // Remove skill on tap
        private void GridViewSkillSelected(object selectedItem, List<Skill> userRemoveList, ObservableCollection<Skill> viewModelSkillsList)
        {
            Skill selectedSkill = selectedItem as Skill;
            if (selectedSkill != null)
            {
                Skill tmpSkill = new Skill(selectedSkill.getBackingObject);
                userRemoveList.Add(tmpSkill);

                viewModelSkillsList.Remove(selectedSkill);
            }
        }

        private async void ButtonSaveChanges_OnClick(object sender, RoutedEventArgs e)
        {
            // 1. Add new Skills that user added
            //LEARN
            if (UserLearnList.Count > 0)
            {
                foreach (Skill skill in UserLearnList)
                {
                    await ParseHelper.AddSkillToUser(ParseUser.CurrentUser, skill.getBackingObject);
                }
            }
            //TEACH
            if (UserTeachList.Count > 0)
            {
                foreach (Skill skill in UserTeachList)
                {
                    await ParseHelper.AddSkillTeachToUser(ParseUser.CurrentUser, skill.getBackingObject);
                }
            }

            // 2. Remove skills checked by user
            // LEARN
            if (UserRemoveLearnList.Count > 0)
            {
                foreach (Skill skill in UserRemoveLearnList)
                {
                    await ParseHelper.RemoveSkillFromUser(skill.getBackingObject);
                }
            }
            // TEACH
            if (UserRemoveTeachList.Count > 0)
            {
                foreach (Skill skill in UserRemoveTeachList)
                {
                    await ParseHelper.RemoveSkillTeachFromUser(skill.getBackingObject);
                }
            }

            // 3. Change name
            if (!ViewModel.CurrentUser.Name.Equals(TextBoxFullName.Text))
            {
                // if text changed
                ViewModel.CurrentUser.Name = TextBoxFullName.Text;
                ViewModel.CurrentUser.BackingObject[ParseHelper.OBJECT_TEST_USER_NAME] = TextBoxFullName.Text;
                ParseUser.CurrentUser[ParseHelper.OBJECT_TEST_USER_NAME] = TextBoxFullName.Text;
                await ViewModel.CurrentUser.BackingObject.SaveAsync();
            }

            // 4. Go to the profile page
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }


        // Listeners:
        // LEARN
        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AutoSuggestionChosen(args, ViewModel.CurrentUser.SkillsWantToLearn, UserLearnList, AutoSuggestBox);
        }
        private void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestionBoxTextChanged(AutoSuggestBox, ViewModel.CurrentUser.SkillsWantToLearn);
        }

        // TEACH
        private void AutoSuggestBoxTeach_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AutoSuggestionChosen(args, ViewModel.CurrentUser.SkillsWantToTeach, UserTeachList, AutoSuggestBoxTeach);
        }
        private void AutoSuggestBoxTeach_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestionBoxTextChanged(AutoSuggestBoxTeach, ViewModel.CurrentUser.SkillsWantToTeach);
        }

        // My listeners: 
        private void AutoSuggestionBoxTextChanged(AutoSuggestBox autoSuggestBox, ObservableCollection<Skill> userSkillsList)
        {
            List<Skill> searchSuggestions =
                new List<Skill>(SearchSuggestionsList.
                    Where(item => (item.Name).ToLower().Contains
                        (autoSuggestBox.Text.ToLower())));

            ResultsSearchSuggestions.Clear();

            // Remove already added by user (to avoid dublicates)
            foreach (Skill skill in searchSuggestions.ToList())
            {
                if (skill.IsContainsInOtherList(userSkillsList))
                    searchSuggestions.Remove(skill);
            }

            // Add all matched skills
            foreach (Skill skill in searchSuggestions)
            {
                ResultsSearchSuggestions.Add(skill);
            }
        }
        private void AutoSuggestionChosen(AutoSuggestBoxSuggestionChosenEventArgs args, ObservableCollection<Skill> viewmodelSkills, List<Skill> userLearnList, AutoSuggestBox autoSuggestBox)
        {
            Skill selectedSkill = args.SelectedItem as Skill;
            if (selectedSkill != null)
            {
                viewmodelSkills.Add(selectedSkill);
                userLearnList.Add(selectedSkill);
            }

            autoSuggestBox.Text = "";
        }


    }
}
