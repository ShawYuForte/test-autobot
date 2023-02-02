using System;
using forte.devices.data.enums;
using forte.models;

namespace forte.devices
{
	public interface ISessionState
	{
		int Id { get; set; }
		WorkflowState Status { get; set; }
		Guid SessioId { get; set; }
		DateTime StartTime { get; set; }
		DateTime EndTime { get; set; }
		string Permalink { get; set; }
		string VmixPreset { get; set; }
		string PrimaryIngestUrl { get; set; }
		string PrimaryIngestKey { get; set; }
		bool VmixUsed { get; set; }
		int RetryCount { get; set; }
		SessionType SessionType { get; set; }
		bool? IsCancelled { get; set; }
	}
}
