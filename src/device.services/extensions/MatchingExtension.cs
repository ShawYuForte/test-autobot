using System.Linq;
using forte.device.models;

namespace forte.device.extensions
{
    public static class MatchingExtension
    {
        /// <summary>
        ///     Determines if the inputs are the same based on the type and title
        /// </summary>
        /// <param name="thisInput"></param>
        /// <param name="thatInput"></param>
        /// <returns></returns>
        public static bool SameAs(this VMixInput thisInput, VMixInput thatInput)
        {
            var result = thisInput != null && thatInput != null &&
                   (thisInput == thatInput ||
                    (thisInput.Title.Replace("Offline - ", "") == thatInput.Title.Replace("Offline - ", "") &&
                     thisInput.Type == thatInput.Type));
            return result;
        }

        /// <summary>
        ///     Determines if the states are the same, purely based on their inputs
        /// </summary>
        /// <param name="thisState"></param>
        /// <param name="thatState"></param>
        /// <returns></returns>
        public static bool SameAs(this VMixState thisState, VMixState thatState)
        {
            if (thisState == null || thatState == null) return false;
            var result = thisState == thatState;
            result = result || 
                (thisState.Inputs.All(thisInput => thatState.Inputs.Any(thatInput => thatInput.SameAs(thisInput))) &&
                thatState.Inputs.All(thatInput => thisState.Inputs.Any(thisInput => thisInput.SameAs(thatInput))));
            return result;
        }
    }
}