using System;
using System.ComponentModel.DataAnnotations;
using forte.devices.data.enums;
using forte.models;

namespace forte.devices.entities
{
    public class SessionState: ISessionState
    {
		[Key]
		public int Id { get; set; }

		public WorkflowState Status { get; set; }

		public Guid SessioId { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public string Permalink { get; set; }

		public string VmixPreset { get; set; }

        public string PrimaryIngestUrl { get; set; }

		public string PrimaryIngestKey { get; set; }

		public bool VmixUsed { get; set; }

		public int RetryCount { get; set; }

		public SessionType SessionType { get; set; }

		public bool? IsCancelled { get; set; }
	}
}