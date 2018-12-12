using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{

    public class UserVariableModel
    {
        public UserVariable[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class UserVariable
    {
        public string Data
        {
            get
            {
                return string.Format("{0} ({1})", Value, TypeValue);
            }
        }

        public string TypeValue
        {
            get
            {
                switch (Type)
                {
                    case "0":
                        return "Integer";
                    case "1":
                        return "Float";
                    case "2":
                        return "String";
                    case "3":
                        return "Date";
                    case "4":
                        return "Time";
                }
                return null;
            }
        }

        public string LastUpdate { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string idx { get; set; }
    }

}
