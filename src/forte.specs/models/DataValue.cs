using System;
using System.Linq;
using Newtonsoft.Json;

namespace forte.models
{
    public class DataValue
    {
        private Guid? _guidValue;
        private string _stringValue;
        private int? _intValue;
        private DateTime? _dateTimeValue;
        private bool? _boolValue;
        private byte[] _byteArrayValue;
        private string _enumValue;
        private string _enumType;

        public DataValue(object value)
        {
            Set(value);
        }

        public DataValue()
        {
        }

        [JsonConstructor]
        private DataValue(
            Guid? guidValue,
            string stringValue,
            int? intValue,
            DateTime? dateTimeValue,
            bool? boolValue,
            byte[] byteArrayValue,
            string enumValue,
            string enumType)
        {
            _guidValue = guidValue;
            _stringValue = stringValue;
            _intValue = intValue;
            _dateTimeValue = dateTimeValue;
            _boolValue = boolValue;
            _byteArrayValue = byteArrayValue;
            _enumValue = enumValue;
            _enumType = enumType;
        }

        public Guid? GuidValue
        {
            get
            {
                return _guidValue;
            }

            set
            {
                if (value != null)
                {
                    ClearValues();
                }
                _guidValue = value;
            }
        }

        public string StringValue
        {
            get
            {
                return _stringValue;
            }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    ClearValues();
                }
                _stringValue = value;
            }
        }

        public int? IntValue
        {
            get
            {
                return _intValue;
            }

            set
            {
                if (value != null)
                {
                    ClearValues();
                }
                _intValue = value;
            }
        }

        public DateTime? DateTimeValue
        {
            get
            {
                return _dateTimeValue;
            }

            set
            {
                if (value != null)
                {
                    ClearValues();
                }
                _dateTimeValue = value;
            }
        }

        public bool? BoolValue
        {
            get
            {
                return _boolValue;
            }

            set
            {
                if (value != null)
                {
                    ClearValues();
                }
                _boolValue = value;
            }
        }

        public byte[] ByteArrayValue
        {
            get
            {
                return _byteArrayValue;
            }

            set
            {
                if (value != null)
                {
                    ClearValues();
                }
                _byteArrayValue = value;
            }
        }

        public string EnumValue
        {
            get
            {
                return _enumValue;
            }

            set
            {
                if (value != null)
                {
                    ClearValues();
                }
                _enumValue = value;
            }
        }

        public string EnumType
        {
            get
            {
                return _enumType;
            }

            set
            {
                if (value != null)
                {
                    ClearValues();
                }
                _enumType = value;
            }
        }

        public void Set<T>(T value)
        {
            ClearValues();
            if (value == null)
            {
                return;
            }

            var typeOfT = value.GetType();

            if (typeOfT == typeof(Guid))
            {
                _guidValue = value as Guid?;
            }
            else if (typeOfT == typeof(string))
            {
                _stringValue = value as string;
            }
            else if (typeOfT == typeof(int))
            {
                _intValue = value as int?;
            }
            else if (typeOfT == typeof(bool))
            {
                _boolValue = value as bool?;
            }
            else if (typeOfT == typeof(DateTime))
            {
                _dateTimeValue = value as DateTime?;
            }
            else if (typeOfT == typeof(byte[]))
            {
                _byteArrayValue = value as byte[];
            }
            else if (value is Enum)
            {
                _enumValue = value.ToString();
                _enumType = value.GetType().AssemblyQualifiedName;
            }
            else if (typeOfT.IsClass)
            {
                Set(JsonConvert.SerializeObject(value));
            }
            else
            {
                throw new NotSupportedException($"Type {typeOfT} is not supported by {nameof(DataValue)}");
            }
        }

        public T Get<T>()
        {
            var typeOfT = typeof(T);

            if (typeOfT == typeof(Guid))
            {
                return (T)(_guidValue.HasValue ? _guidValue.Value as object : Guid.Empty as object);
            }
            if (typeOfT == typeof(string))
            {
                return (T)((object)_stringValue);
            }
            if (typeOfT == typeof(int))
            {
                return (T)(_intValue.HasValue ? _intValue.Value as object : 0 as object);
            }
            if (typeOfT == typeof(bool))
            {
                return (T)(_boolValue.HasValue ? _boolValue.Value as object : false as object);
            }
            if (typeOfT == typeof(DateTime))
            {
                return (T)(_dateTimeValue.HasValue ? _dateTimeValue.Value as object : DateTime.MinValue as object);
            }
            if (typeOfT == typeof(byte[]))
            {
                return (T)(object)_byteArrayValue;
            }
            if (typeOfT.IsEnum)
            {
                return (T)ToEnum(_enumType, _enumValue);
            }
            if (typeOfT.IsClass)
            {
                return JsonConvert.DeserializeObject<T>(_stringValue);
            }
            throw new NotSupportedException($"Type {typeOfT} is not supported by {nameof(DataValue)}");
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as DataValue;
            if (otherObj == null)
            {
                return false;
            }

            var equal = AreEqual(_guidValue, otherObj.GuidValue);
            equal = equal && _stringValue == StringValue;
            equal = equal && AreEqual(_intValue, otherObj.IntValue);
            equal = equal && AreEqual(_dateTimeValue, otherObj.DateTimeValue);
            equal = equal && AreEqual(_boolValue, otherObj.BoolValue);
            equal = equal && AreEqual(_byteArrayValue, otherObj.ByteArrayValue);
            equal = equal && _enumValue == otherObj.EnumValue;
            equal = equal && _enumType == otherObj.EnumType;

            return equal;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _guidValue.GetHashCode();
                hashCode = (hashCode * 397) ^ (_stringValue?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ _intValue.GetHashCode();
                hashCode = (hashCode * 397) ^ _dateTimeValue.GetHashCode();
                hashCode = (hashCode * 397) ^ _boolValue.GetHashCode();
                hashCode = (hashCode * 397) ^ (_byteArrayValue?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (_enumValue?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (_enumType?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public DataValue Clone()
        {
            var clone = new DataValue
            {
                _boolValue = _boolValue,
                _guidValue = _guidValue,
                _stringValue = _stringValue,
                _byteArrayValue = _byteArrayValue,
                _intValue = _intValue,
                _dateTimeValue = _dateTimeValue,
                _enumValue = _enumValue,
                _enumType = _enumType,
            };

            return clone;
        }

        public object Get()
        {
            if (_boolValue.HasValue)
            {
                return _boolValue.Value;
            }

            if (_guidValue.HasValue)
            {
                return _guidValue.Value;
            }

            if (!string.IsNullOrWhiteSpace(_stringValue))
            {
                return _stringValue;
            }

            if (_byteArrayValue != null)
            {
                return _byteArrayValue;
            }

            if (_intValue.HasValue)
            {
                return _intValue.Value;
            }

            if (_enumValue != null)
            {
                return ToEnum(_enumType, _enumValue);
            }

            return _dateTimeValue;
        }

        private static object ToEnum(string type, string value)
        {
            try
            {
                return Enum.Parse(Type.GetType(type), value);
            }
            catch
            {
                return null;
            }
        }

        private static bool AreEqual<T>(T? obj1, T? obj2)
            where T : struct
        {
            return (!obj1.HasValue && !obj2.HasValue) ||
                   (obj1.HasValue && obj2.HasValue && obj1.Value.Equals(obj2.Value));
        }

        private static bool AreEqual(byte[] arr1, byte[] arr2)
        {
            if (arr1 == null && arr2 == null)
            {
                return true;
            }

            var equal = arr1 != null && arr2 != null && arr1.Length == arr2.Length;

            if (!equal)
            {
                return false;
            }

            return !arr1.Where((t, index) => t != arr2[index]).Any();
        }

        private void ClearValues()
        {
            _guidValue = null;
            _stringValue = null;
            _intValue = null;
            _dateTimeValue = null;
            _boolValue = null;
            _byteArrayValue = null;
            _enumValue = null;
            _enumType = null;
        }
    }
}
