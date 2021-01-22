using System;
using System.Collections.Generic;

namespace forte.models.events
{
    /// <summary>
    ///     Event descriptor
    /// </summary>
    public class EventModel
    {
        private string _type;
        private Guid _typeId;

        protected EventModel()
        {
            Id = Guid.NewGuid();
            Data = new Dictionary<string, DataValue>();
        }

        /// <summary>
        ///     Event instance identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Event type unique identifier.
        /// </summary>
        public Guid TypeId
        {
            get
            {
                if (_typeId == Guid.Empty)
                {
                    _typeId = GetData<Guid>(nameof(TypeId));
                }
                return _typeId;
            }

            protected set
            {
                _typeId = value;
                SetData(nameof(TypeId), _typeId);
            }
        }

        /// <summary>
        ///     Event class type
        /// </summary>
        public string Type
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_type))
                {
                    _type = GetType().AssemblyQualifiedName;
                }

                return _type;
            }
        }

        /// <summary>
        ///     Event human friendly name (for logging and similar purposes)
        /// </summary>
        public virtual string Event { get; }

        /// <summary>
        ///     Event data.
        /// </summary>
        public Dictionary<string, DataValue> Data { get; protected set; }

        /// <summary>
        ///     Event metadata, use for things like JSON
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        ///     Gets the data item by name.
        /// </summary>
        /// <param name="dataPropertyName">Name of the data item.</param>
        /// <returns>The data item value as string.</returns>
        public T GetData<T>(string dataPropertyName)
        {
            if (Data == null || !Data.ContainsKey(dataPropertyName))
            {
                return default(T);
            }

            return Data[dataPropertyName].Get<T>();
        }

        /// <summary>
        ///     Set the data item by name
        /// </summary>
        /// <param name="dataPropertyName"></param>
        /// <param name="data"></param>
        public void SetData(string dataPropertyName, object data)
        {
            if (Data == null)
            {
                Data = new Dictionary<string, DataValue>();
            }

            if (Data.ContainsKey(dataPropertyName))
            {
                Data[dataPropertyName].Set(data);
            }
            else
            {
                Data.Add(dataPropertyName, new DataValue(data));
            }
        }
    }
}
