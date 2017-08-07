using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class QRCodeModel
    {
        public bool Enabled { get; set; }
        public bool IsScene { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string SwitchIDX { get; set; }
        public string SwitchName { get; set; }
        public string SwitchPassword { get; set; }
        public string Value { get; set; }

        public String SwitchDescription
        {
            get
            {
                return  String.IsNullOrEmpty(SwitchName) ? AppResources.connectedSwitch + ": -" : AppResources.connectedSwitch + ": " + SwitchName;
            }
        }
    }
}
