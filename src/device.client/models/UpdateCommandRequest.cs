using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forte.devices.models
{
    public class UpdateCommandRequest
    {
        public DateTime? ExecutedOn { get; set; }
        public bool ExecutionSucceeded { get; set; }
        public string ExecutionMessages { get; set; }
    }
}
