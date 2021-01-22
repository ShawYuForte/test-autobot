using System;

namespace forte.models.classes
{
    public class ClassRequest
    {
        private Guid? _classId;
        private string _permalink;

        /// <summary>
        /// Class identifier, it might be a GUID (representing class id) or a simple string (representing the class permalink)
        /// </summary>
        public string ClassIdentifier { get; set; }

        /// <summary>
        /// Request extended model
        /// </summary>
        public bool Extended { get; set; }

        /// <summary>
        /// Class identifier, null if permalink is provided
        /// </summary>
        public Guid? ClassId
        {
            get
            {
                if (_classId != null)
                {
                    return _classId;
                }

                Guid classId;
                if (Guid.TryParse(ClassIdentifier, out classId))
                {
                    _classId = classId;
                }

                return _classId;
            }

            set
            {
                _classId = value;
            }
        }

        /// <summary>
        /// Class permalink
        /// </summary>
        public string Permalink
        {
            get { return string.IsNullOrWhiteSpace(_permalink) ? ClassIdentifier : _permalink; }
            set { _permalink = value; }
        }
    }
}
