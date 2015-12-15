using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace PJA_Skills_032.Model
{
    public class TestUser
    {
        #region property
        private readonly ParseObject _backingObject;

        public string Name
        {
            get
            {
                return _backingObject != null && _backingObject.ContainsKey("Name") ? _backingObject.Get<string>("Name") : null;
            }
            set { if (_backingObject != null) _backingObject["Name"] = value; }
            //get; set;
        }


        public string Faculty
        {
            get { return _backingObject != null && _backingObject.ContainsKey("Faculty") ? _backingObject.Get<string>("Faculty") : null; }
            set { if (_backingObject != null) _backingObject["Faculty"] = value; }
            //get; set;
        }

        #endregion


        #region contructor

        public TestUser(ParseObject backingParseObject)
        {
            this._backingObject = backingParseObject;
        }


        public TestUser(string name, string faculty)
        {
            this.Name = name;
            this.Faculty = faculty;
        }

        #endregion

    }
}
