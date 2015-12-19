using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Parse;
using PJA_Skills_032.Model;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.ViewModel
{
    public class HomeViewModel : BindableBase
    {
        private ObservableCollection<TestUser> _users = new ObservableCollection<TestUser>();
        public string Width { get; set; } = "777";

        public ObservableCollection<TestUser> Users
        {
            get { return this._users; }
            set { this.SetProperty(ref this._users, value); }
        }

        public HomeViewModel()
        {
            //TestUser testUser = new TestUser("Tim Cook", "Informatyka");
            //TestUser testUser2 = new TestUser("Richard Brenson", "SNM");

            //Users.Add(testUser);
            //Users.Add(testUser2);

        }

        #region methods
        //public async Task<ObservableCollection<TestUser>> GetAllUsers()
        //{
        //    var allItems = await GetUsersParseObject();
        //    var itemList = new ObservableCollection<TestUser>(allItems);

        //    return itemList;
        //}


        /// <summary>
        /// download ParseObject of all users
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<TestUser>> GetAllUsers()
        {
            ParseQuery<ParseObject> query = from item in ParseObject.GetQuery(ParseHelper.OBJECT_TEST_USER)
                                            orderby item.CreatedAt
                                            select item;

            IEnumerable<TestUser> allItems = from item in await query.FindAsync()
                                             select new TestUser(item);

            var itemList = new ObservableCollection<TestUser>(allItems);
            return itemList;
        }


        /// <summary>
        /// Add users to list after downloading
        /// </summary>
        /// <returns></returns>
        public async Task DownloadUsers()
        {
            var downloadedUsers = await GetAllUsers();
            foreach (var user in downloadedUsers)
            {
                await user.GetSkillsL(); // get skills relationship items
                Users.Add(user);
            }
        }



        #endregion

    }
}
