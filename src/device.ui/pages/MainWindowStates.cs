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
    }
}