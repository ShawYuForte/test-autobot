using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forte.devices.services
{
    public interface IRuntimeConfig
    {
        string LogPath { get; set; }
        string DataPath { get; set; }
    }
}
