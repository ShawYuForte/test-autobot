using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forte.devices.services
{
    public class RuntimeConfig : IRuntimeConfig
    {
        public string LogPath { get; set; }
        public string DataPath { get; set; }
    }
}
