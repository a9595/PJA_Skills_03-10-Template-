using System;
using Parse;

namespace PJA_Skills_032.Model
{
    public class Skill
    {
        private ParseObject _backingObject;

        //public string Name
        //{
        //    get
        //    {
        //        return _backingObject != null && _backingObject.ContainsKey("Name") ? _backingObject.Get<string>("Name") : null;
        //    }
        //    set { if (_backingObject != null) _backingObject["Name"] = value; }
        //}
        public string Name { get; set; }


        public Skill(ParseObject backingParseObject)
        {
            if (backingParseObject == null) throw new ArgumentNullException(nameof(backingParseObject));
            this._backingObject = backingParseObject;
        }

        public Skill(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}