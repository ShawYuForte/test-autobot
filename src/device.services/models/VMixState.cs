using System.Collections.Generic;

namespace forte.device.models
{
    public class VMixState
    {
        public string Version { get; set; }
        public List<VMixInput> Inputs { get; set; }
        public string PreviewNumber { get; set; }
        public VMixInput Preview { get; set; }
        public VMixInput Active { get; set; }
        public bool Recording { get; set; }
        public bool Streaming { get; set; }
        public bool Playlist { get; set; }
        public List<VMixAudio> Audio { get; set; }
    }
}