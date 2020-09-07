namespace forte.devices.data.enums
{
	public enum WorkflowState
	{
		Idle = 0,
		VmixLoaded = 5,
		LinkedToAzure = 10,
		StreamingPublish = 15,
		StreamingAgora = 17,
		StreamingServer = 20,
		StreamingClient = 25,
		ProgramRunning = 30,
		Processed = 40,
		CancelPending = 100
	}
}
