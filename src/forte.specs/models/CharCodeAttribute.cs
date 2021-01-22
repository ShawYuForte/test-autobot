using System;

namespace forte.models
{
    /// <summary>
    ///     Assign a character code to an enumertion
    /// </summary>
    public class CharCodeAttribute : Attribute
    {
        public CharCodeAttribute(char code)
        {
            Code = code;
        }

        private char Code { get; set; }

        public string GetCode()
        {
            return $"{Code}";
        }
    }
}
