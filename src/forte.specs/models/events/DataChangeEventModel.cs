using System;

namespace forte.models.events
{
    public class DataChangeEventModel : EventModel
    {
        private DataChanges _dataChange;
        private string _dataType;

        private string _event;

        public DataChangeEventModel(string dataType, DataChanges dataChange, Guid dataRecordId)
            : this()
        {
            DataType = dataType;
            DataChange = dataChange;
            SetData($"{dataType}.Id", dataRecordId);
        }

        public DataChangeEventModel(string dataType, DataChanges dataChange, string dataRecordId)
            : this()
        {
            DataType = dataType;
            DataChange = dataChange;
            SetData($"{dataType}.Id", dataRecordId);
        }

        public DataChangeEventModel()
        {
            TypeId = EventId;
        }

        public static Guid EventId => Guid.Parse("86B6F3F5-A7EA-48DF-A865-9AC783E3DAE4");

        /// <summary>
        ///     Data change type
        /// </summary>
        public DataChanges DataChange
        {
            get
            {
                return _dataChange;
            }

            set
            {
                _dataChange = value;
                SetEventDescription();
            }
        }

        /// <summary>
        ///     Data type
        /// </summary>
        public string DataType
        {
            get
            {
                return _dataType;
            }

            set
            {
                _dataType = value;
                SetEventDescription();
            }
        }

        public override string Event => _event;

        public ChangeTypeDetails ChangeTypeDetails { get; set; }

        public T GetDataRecordId<T>()
        {
            return GetData<T>($"{DataType}.Id");
        }

        private void SetEventDescription()
        {
            _event = string.IsNullOrWhiteSpace(DataType) ? "Data changed" : $"{DataType} {DataChange}";
        }
    }
}
