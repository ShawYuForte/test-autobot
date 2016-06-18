using System;

namespace forte.device.services
{
    public abstract class Service
    {
        private readonly string _name;

        protected Service(string name)
        {
            _name = name;
        }

        public delegate void LogDelegate(string message);

        public event LogDelegate OnLog;

        protected void Log(string message)
        {
            OnLog?.Invoke($"({_name}) => {message}");
        }

        protected bool TryExecute(Action action, string actionDescription = null)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception exception)
            {
                Log(string.IsNullOrWhiteSpace(actionDescription)
                    ? $"ERROR: {exception.Message}"
                    : $"ERROR: '{actionDescription}' failed because of '{exception.Message}'");
            }
            return false;
        }
    }
}