using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Parse;
using PJA_Skills_032.Model;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.ViewModel
{
    public class MyProfileViewModel : BindableBase
    {
        private TestUser _currentUser;

        public TestUser CurrentUser
        {
            get { return this._currentUser; }
            set { this.SetProperty(ref this._currentUser, value); }
        }

        public MyProfileViewModel()
        {
            _currentUser = new TestUser(ParseUser.CurrentUser);
        }

        #region methods





        #endregion

    }
}
