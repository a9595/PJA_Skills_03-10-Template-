using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace PJA_Skills_032.Model
{
    class Contacts
    {
        public static List<Contact> ContactsList { get; set; }

        public static async Task<ObservableCollection<Contact>> GetAllContacts()
        {
            ParseQuery<ParseObject> query2 = ParseObject.GetQuery("TestUser");
            var findAsync = query2.FindAsync();
            ParseQuery<ParseObject> whereExists = query2.WhereExists("objectId");

            var query = from item in ParseObject.GetQuery("TestUser")
                        orderby item.CreatedAt
                        select item;


            var allItems = from item in await query.FindAsync()
                           select new Contact(item);
            var itemList = new ObservableCollection<Contact>(allItems);


            return itemList;
        }

        public static ObservableCollection<GroupInfoList> GetContactsGrouped()
        {
            ObservableCollection<GroupInfoList> groups = new ObservableCollection<GroupInfoList>();

            Task<ObservableCollection<Contact>> allContactsTask = GetAllContacts();
            allContactsTask.Wait();
            var allContactsResult = allContactsTask.Result;
            var query = from item in allContactsResult
                        group item by item.LastName[0] into g
                        orderby g.Key
                        select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoList info = new GroupInfoList();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
            }

            return groups;
        }
    }
}
