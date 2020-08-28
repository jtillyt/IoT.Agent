using IoT.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.DataTypes
{
    public class CommonTypes
    {
        public class IntervalInMS:DataTypeBase<int>{ public IntervalInMS(int intervalValue):base(intervalValue){ } }
        public class RepeatCount:DataTypeBase<int>{ public RepeatCount(int repeatCountValue):base(repeatCountValue){ } }
    }
}
