using System.ComponentModel;
using AutoMapper;
using forte.device.Properties;
using Newtonsoft.Json;
using PostSharp.Patterns.Model;

namespace forte.device.models
{
    [NotifyPropertyChanged]
    public class AppState
    {
        static AppState()
        {
            Mapper.CreateMap<AppState, AppState>();
            Instance = JsonConvert.DeserializeObject<AppState>(Settings.Default.State);
        }

        AppState()
        {
            ((INotifyPropertyChanged)this).PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var stateString = JsonConvert.SerializeObject(this);
            Settings.Default.State = stateString;
            Settings.Default.Save();
        }

        public static AppState Instance { get; }

        public string AmsAccountName { get; set; }
        public string AmsAccountKey { get; set; }
        public string AmsEncoding { get; set; }
        public string ChannelName { get; set; }
        public string ProgramName { get; set; }
        public string TrainerName { get; set; }
        public string VmixPresetFilePath { get; set; }
        public Workflow WorkflowState { get; set; }
    }
}
