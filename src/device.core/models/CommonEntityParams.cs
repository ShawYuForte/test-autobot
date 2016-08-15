namespace forte.devices.models
{
    public static class CommonEntityParams
    {
        public static string VideoStreamId
            => $"{nameof(VideoStreamModel).Replace("Model", "")}.{nameof(VideoStreamModel.Id)}";

        public static string VideoStreamStartTime
            => $"{nameof(VideoStreamModel).Replace("Model", "")}.{nameof(VideoStreamModel.StartTime)}";

        public static string VideoStreamEndTime
            => $"{nameof(VideoStreamModel).Replace("Model", "")}.{nameof(VideoStreamModel.EndTime)}";
    }
}