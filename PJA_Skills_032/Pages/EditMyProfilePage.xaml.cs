using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
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
        private ParseFile _avatarParseFile;
        private BitmapImage _avatarBitmapImage;
        private byte[] _avatarBytesArray;
        private StorageFile _avatarFile;


        //private  _allSkillsList;

        public MyProfileViewModel ViewModel { get; set; }
        public EditMyProfilePage()
        {
            InitializeComponent();

            //PopulateSearchSuggestions();

            AutoSuggestBox.ItemsSource = ResultsSearchSuggestions;
            AutoSuggestBoxTeach.ItemsSource = ResultsSearchSuggestions;
            AutoSuggestBoxKorking.ItemsSource = ResultsSearchSuggestions;
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
        private void GridViewKorking_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridViewSkillSelected(GridViewKorking.SelectedItem, UserRemoveKorkingList, ViewModel.CurrentUser.SkillsWantToKorking);
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
            //KORKING
            if (UserKorkingList.Count > 0)
            {
                foreach (Skill skill in UserKorkingList)
                {
                    await ParseHelper.AddSkillKorkingToUser(ParseUser.CurrentUser, skill.getBackingObject);
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
            // KORKING
            if (UserRemoveKorkingList.Count > 0)
            {
                foreach (Skill skill in UserRemoveKorkingList)
                {
                    await ParseHelper.RemoveSkillKorkingFromUser(skill.getBackingObject);
                }
            }

            // 3. Change name
            if (!ViewModel.CurrentUser.Name.Equals(TextBoxFullName.Text) && !string.IsNullOrWhiteSpace(TextBoxFullName.Text))
            {
                ParseUser.CurrentUser[ParseHelper.OBJECT_TEST_USER_NAME] = TextBoxFullName.Text;
                ViewModel.CurrentUser.Name = TextBoxFullName.Text;
                await ParseUser.CurrentUser.SaveAsync();
            }

            // 4. Avatar
            if (_avatarBytesArray != null)
            {
                // save to Parse
                _avatarParseFile = new ParseFile("avatar" + _avatarFile.FileType, _avatarBytesArray);

                ParseUser.CurrentUser[ParseHelper.OBJECT_TEST_USER_AVATAR] = _avatarParseFile;
                await ParseUser.CurrentUser.SaveAsync();
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

        // KORKING
        private void AutoSuggestBoxKorking_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AutoSuggestionChosen(args, ViewModel.CurrentUser.SkillsWantToKorking, UserKorkingList, AutoSuggestBoxKorking);
        }
        private void AutoSuggestBoxKorking_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestionBoxTextChanged(AutoSuggestBoxKorking, ViewModel.CurrentUser.SkillsWantToKorking);
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


        private async void BtnChangeAvatar_OnClick(object sender, RoutedEventArgs e)
        {
            // Set up the file picker.
            FileOpenPicker openPicker =
                new FileOpenPicker();
            openPicker.SuggestedStartLocation =
                PickerLocationId.PicturesLibrary;
            openPicker.ViewMode =
                PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types.
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");

            // Open the file picker.
            _avatarFile = await openPicker.PickSingleFileAsync();

            // 'file' is null if user cancels the file picker.
            if (_avatarFile != null)
            {
                // Open a stream for the selected file.
                // The 'using' block ensures the stream is disposed
                // after the image is loaded.
                using (IRandomAccessStream fileStream =
                    await _avatarFile.OpenAsync(FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap.
                    _avatarBitmapImage = new BitmapImage();

                    _avatarBitmapImage.SetSource(fileStream);
                    userImage.Source = _avatarBitmapImage;


                    // stream to []byte
                    DataReader reader = new DataReader(fileStream.GetInputStreamAt(0));
                    _avatarBytesArray = new byte[fileStream.Size];
                    await reader.LoadAsync((uint)fileStream.Size);
                    reader.ReadBytes(_avatarBytesArray);

                    //// save to Parse
                    //_avatarParseFile = new ParseFile("avatar" + file.FileType, _avatarBytesArray);

                    //ParseUser.CurrentUser[ParseHelper.OBJECT_TEST_USER_AVATAR] = _avatarParseFile;
                    //await ParseUser.CurrentUser.SaveAsync();

                }
            }
        }
    }
}
