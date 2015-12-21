using System;
using Parse;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.Model
{
    public class Skill
    {
        private readonly ParseObject _backingObject;

        public string Name
        {
            get
            {
                return ParseHelper.GetParseObject(ParseHelper.OBJECT_SKILL_NAME, _backingObject);
            }

            set
            {
                ParseHelper.SetParseObject(ParseHelper.OBJECT_SKILL_NAME, _backingObject, value);
            }
        }

        public Skill(ParseObject backingParseObject)
        {
            if (backingParseObject == null) throw new ArgumentNullException(nameof(backingParseObject));
            this._backingObject = backingParseObject;
        }

        /*
                public Skill(string name)
                {
                    if (name == null) throw new ArgumentNullException(nameof(name));
                    this.Name = name;
                }
        */

        public override string ToString()
        {
            return Name;
        }
    }
}