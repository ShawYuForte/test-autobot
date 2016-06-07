using System.Linq;
using System.Windows;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        public Workflow WorkflowState { get; set; }

        public enum Workflow
        {
            /// <summary>
            ///     Not started, not sure what the state of vMix is
            /// </summary>
            NotStarted,

            /// <summary>
            ///     vMix preset has been loaded and verified
            /// </summary>
            PresetLoadVerified,

            /// <summary>
            ///     Static image loaded, ready for Azure Program to be started
            /// </summary>
            ReadyForAzure,

            /// <summary>
            ///     Started streaming
            /// </summary>
            Streaming,

            /// <summary>
            ///     Streaming was completed successfully
            /// </summary>
            CompletedSession
        }

        private void SetWorkflowStep(Workflow step)
        {
            WorkflowState = step;
            Step0Visible = step == Workflow.NotStarted;
            Step1Visible = step == Workflow.PresetLoadVerified;
            Step2Visible = step == Workflow.ReadyForAzure;
            Step3Visible = step == Workflow.Streaming;
            Step4Visible = step == Workflow.CompletedSession;
        }

        private void CalculateWorkflowStep()
        {
            var step = Workflow.NotStarted;
            if (AmAtStep2()) step = Workflow.PresetLoadVerified;
            if (AmAtStep3()) step = Workflow.ReadyForAzure;
            if (AmAtStep4()) step = Workflow.Streaming;
            SetWorkflowStep(step);
        }

        private bool AmAtStep2()
        {
            return _vmixService.PresetLoaded();
        }

        private bool AmAtStep3()
        {
            var iAm = State.Active != null &&
                       State.Active.Key ==
                       State.Inputs.Single(input => input.Role == forte.device.models.InputRole.OpeninStaticImage).Key;

            iAm = iAm && State.Preview != null &&
                  State.Preview.Key ==
                  State.Inputs.Single(input => input.Role == forte.device.models.InputRole.OpeningVideo).Key;

            iAm = iAm && State.Streaming;

            return iAm;
        }

        private bool AmAtStep4()
        {
            // TODO complete
            return State.Playlist && State.Streaming && false;
        }
    }
}