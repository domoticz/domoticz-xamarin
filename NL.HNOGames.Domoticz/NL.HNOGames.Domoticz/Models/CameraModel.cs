using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class CameraModel
    {
        public Camera[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class Camera
    {
        public Stream ImageStream { get; set; }
        public string Address { get; set; }
        public string Enabled { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int Protocol { get; set; }
        public string Username { get; set; }
        public string idx { get; set; }
    }
}
