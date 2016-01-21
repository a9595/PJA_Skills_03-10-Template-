using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.Model
{
    public class Offer
    {
        public ParseObject BackingObject { get; }

        public string Content
        {
            get
            {
                return ParseHelper.GetParseObject(ParseHelper.OBJECT_OFFER_CONTENT, BackingObject);
            }
            set { ParseHelper.SetParseObject(ParseHelper.OBJECT_OFFER_CONTENT, BackingObject, value); }
        }

        public TestUser User
        {
            get
            {
                ParseObject userParseObject = ParseHelper.GetParseObjectObject(ParseHelper.OBJECT_OFFER_USER, BackingObject);
                return new TestUser(userParseObject);
            }
            set
            {
                ParseHelper.SetParseObjectObject(ParseHelper.OBJECT_OFFER_USER, BackingObject, value.BackingObject);
            }
        }


        public Offer(ParseObject backingObject)
        {
            // TODO
            this.BackingObject = backingObject;
        }


    }
}
