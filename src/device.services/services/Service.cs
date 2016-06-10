using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forte.device.services
{
    public abstract class Service
    {
        public event LogDelegate OnLog;

        public delegate void LogDelegate(string message);

        protected void Log(string message)
        {
            OnLog?.Invoke(message);
        }
    }
}
