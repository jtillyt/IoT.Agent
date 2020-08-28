using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Messages
{
    public class DataTypeBase<TBaseType>
    {
        readonly TBaseType _value;

        public DataTypeBase(TBaseType value)
        {
            _value = value;
        }
        public static implicit operator TBaseType(DataTypeBase<TBaseType> item)
        {
            return item._value;
        }
        public static implicit operator DataTypeBase<TBaseType>(TBaseType value)
        {
            return new DataTypeBase<TBaseType>(value);
        }
    }
}
