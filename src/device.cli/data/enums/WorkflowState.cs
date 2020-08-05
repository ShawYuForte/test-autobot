﻿namespace forte.devices.data.enums
{
	public enum WorkflowState
	{
		Idle = 0,
		VmixLoaded = 5,
		Linked = 10,
		StreamingPublish = 15,
		StreamingServer = 20,
		StreamingClient = 25,
		Program = 30,
		Processed = 40,
		CancelPending = 100
	}
}
