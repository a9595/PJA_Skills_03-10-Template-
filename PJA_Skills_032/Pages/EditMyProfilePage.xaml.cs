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
        public class SearchSuggestion
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public SearchSuggestion(string name, int id)
            {
                Id = id;
                Name = name;
            }

            //public override string ToString()
            //{
            //    return Name;

            //}
        }

        public ObservableCollection<Skill> SearchSuggestionsList = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> ResultsSearchSuggestions = new ObservableCollection<Skill>();
        public List<Skill> SkillsUserWantsToLearnAdded = new List<Skill>(); // keep added skills. saved after tapping "save changes"
        public List<Skill> SkillsUserWantsToLearnRemoved = new List<Skill>();

        //private  _allSkillsList;

        public MyProfileViewModel ViewModel { get; set; }
        public EditMyProfilePage()
        {
            this.InitializeComponent();

            //PopulateSearchSuggestions();

            AutoSuggestBox.ItemsSource = ResultsSearchSuggestions;
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

        private void TextBoxAddWantToLearn_OnTextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonAddWantToLearn_OnClick(object sender, RoutedEventArgs e)
        {
            //string addedSkillName = TextBoxAddWantToLearn.Text;
            //if (string.IsNullOrWhiteSpace(addedSkillName))
            //{
            //    Skill addedSkill = new Skill(addedSkillName);
            //    ViewModel.CurrentUser.SkillsWantToLearn.Add(addedSkill);


            //    ParseObject addedSkillParseObject = new ParseObject(ParseHelper.OBJECT_SKILL);

            //    // Add skill to user
            //    addedSkillParseObject.Add(ParseHelper.OBJECT_SKILL_NAME, addedSkillName);

            //    var skillUsersRelation = addedSkillParseObject.GetRelation<ParseObject>(ParseHelper.OBJECT_SKILL_USERS);
            //    skillUsersRelation.Add(ViewModel.CurrentUser.BackingObject); // add current user to relation
            //    addedSkillParseObject.SaveAsync();

            //}
        }

        private void GridViewLearn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Skill selectedSkill = GridViewLearn.SelectedItem as Skill;
            if (selectedSkill != null)
            {
                Skill tmpSkill = new Skill(selectedSkill.getBackingObject);
                SkillsUserWantsToLearnRemoved.Add(tmpSkill);


                ViewModel.CurrentUser.SkillsWantToLearn.Remove(selectedSkill);
            }


        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            List<Skill> searchSuggestions =
                new List<Skill>(SearchSuggestionsList.
                Where(item => (item.Name).ToLower().Contains
                (args.QueryText.ToLower())));

            ResultsSearchSuggestions.Clear();




            // Remove already added by users
            foreach (Skill skill in searchSuggestions.ToList())
            {
                if (skill.IsContainsInOtherList(ViewModel.CurrentUser.SkillsWantToLearn))
                    searchSuggestions.Remove(skill);
            }

            // Add all skills
            foreach (Skill skill in searchSuggestions)
            {
                ResultsSearchSuggestions.Add(skill);
            }
        }

        //public Boolean isContain(Skill skillA, Skill skillB)
        //{

        //}

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Skill selectedSearchSuggestionSkill = args.SelectedItem as Skill;
            if (selectedSearchSuggestionSkill != null)
            {
                string id = selectedSearchSuggestionSkill.getBackingObject.ObjectId;
                ViewModel.CurrentUser.SkillsWantToLearn.Add(selectedSearchSuggestionSkill);

                SkillsUserWantsToLearnAdded.Add(selectedSearchSuggestionSkill);

                //await ParseHelper.AddSkillToUser(ParseUser.CurrentUser, selectedSearchSuggestionSkill.getBackingObject); //Add to Parse

            }

        }

        private async void ButtonSaveChanges_OnClick(object sender, RoutedEventArgs e)
        {
            // 1. Add new Skills that user added
            if (SkillsUserWantsToLearnAdded.Count > 0)
            {
                foreach (Skill skill in SkillsUserWantsToLearnAdded)
                {
                    await ParseHelper.AddSkillToUser(ParseUser.CurrentUser, skill.getBackingObject);
                }
            }






            // 2. Remove skills checked by user
            if (SkillsUserWantsToLearnRemoved.Count > 0)
            {
                foreach (Skill skill in SkillsUserWantsToLearnRemoved)
                {
                    await ParseHelper.RemoveSkillFromUser(skill.getBackingObject);
                }
            }

            // 3. Change name
            if (!ViewModel.CurrentUser.Name.Equals(TextBoxFullName.Text))
            {
                // if text changed
                ViewModel.CurrentUser.Name = TextBoxFullName.Text;
                ViewModel.CurrentUser.BackingObject[ParseHelper.OBJECT_TEST_USER_NAME] = TextBoxFullName.Text;
                await ViewModel.CurrentUser.BackingObject.SaveAsync();
            }


            // 4. Go to the profile page
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }

        }


    }
}
