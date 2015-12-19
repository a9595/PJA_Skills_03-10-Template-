using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace PJA_Skills_032.ParseObjects
{
    class ParseHelper
    {

        #region filed

        //USER:
        public static readonly string OBJECT_TEST_USER = "TestUser";
        public static readonly string OBJECT_TEST_USER_NAME = "Name";
        public static readonly string OBJECT_TEST_USER_FACULTY = "Faculty";
        public static readonly string OBJECT_TEST_USER_SKILLS_WANT_TO_LEARN = "SkillsWantToLearn2";


        //SKILL:
        public static readonly string OBJECT_SKILL_NAME = "Name";
        public static readonly string OBJECT_SKILL = "Skill";

        // General:
        public static readonly string OBJECT_ID = "ObjectId";

        // Default dummy data
        public static readonly string DEFAULT_LOGIN = "tieorange";
        public static readonly string DEFAULT_PASSWORD = "admin";

        #endregion

        #region method
        public static string GetParseObject(string parseObjectKey, ParseObject backingObject)
        {
            return backingObject != null
                    && backingObject.ContainsKey(parseObjectKey)
                    ? backingObject.Get<string>(parseObjectKey)
                    : null;
        }
        public static void SetParseObject(string parseObjectKey, ParseObject backingObject, string value)
        {
            if (backingObject != null
                   && backingObject.ContainsKey(parseObjectKey))
                backingObject[parseObjectKey] = value;
        }
        #endregion
    }
}
