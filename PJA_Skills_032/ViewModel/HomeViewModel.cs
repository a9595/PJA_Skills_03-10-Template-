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

namespace PJA_Skills_032.ViewModel
{
    public class HomeViewModel : BindableBase
    {
        private ObservableCollection<TestUser> _users = new ObservableCollection<TestUser>();

        public ObservableCollection<TestUser> Users
        {
            get { return this._users; }
            set { this.SetProperty(ref this._users, value); }
        }

        public HomeViewModel()
        {
            TestUser testUser = new TestUser("Tim Cook", "Informatyka");
            TestUser testUser2 = new TestUser("Richard Brenson", "SNM");

            Users.Add(testUser);
            Users.Add(testUser2);

        }


        public async Task<ObservableCollection<TestUser>> GetAllContacts()
        {
            ParseQuery<ParseObject> query2 = ParseObject.GetQuery("TestUser");
            var findAsync = query2.FindAsync();
            //ParseQuery<ParseObject> whereExists = query2.WhereExists("objectId");

            var query = from item in ParseObject.GetQuery("TestUser")
                        orderby item.CreatedAt
                        select item;


            var allItems = from item in await query.FindAsync()
                           select new TestUser(item);
            var itemList = new ObservableCollection<TestUser>(allItems);


            return itemList;
        }

        public async Task AddDownloadedUsers()
        {
            var downloadedUsers = await GetAllContacts();
            foreach (var user in downloadedUsers)
            {
                Users.Add(user);
            }

        }
    }
}
