namespace forte.models.classes
{
    public class SwitchAttendanceStatusRequestModel
    {
        /// <summary>
        /// The session attendance status to switch onto.
        /// </summary>
        //[EnumValueValidation]
        public EventAttendanceStatus Status { get; set; }
    }
}
