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
        public async Task<ObservableCollection<TestUser>> GetAllContacts()
        {
            var allItems = await GetUsersParseObject();
            var itemList = new ObservableCollection<TestUser>(allItems);


            return itemList;
        }

        private async Task<IEnumerable<TestUser>> GetUsersParseObject()
        {
            ParseQuery<ParseObject> query2 = ParseObject.GetQuery(ParseHelper.OBJECT_TEST_USER);
            var findAsync = query2.FindAsync();
            //ParseQuery<ParseObject> whereExists = query2.WhereExists("objectId");

            var query = from item in ParseObject.GetQuery(ParseHelper.OBJECT_TEST_USER)
                        orderby item.CreatedAt
                        select item;


            var allItems = from item in await query.FindAsync()
                           select new TestUser(item);
            return allItems;
        }


        public async Task AddDownloadedUsers()
        {
            var downloadedUsers = await GetAllContacts();
            foreach (var user in downloadedUsers)
            {
                Users.Add(user);
            }

        }

        #endregion

    }
}
