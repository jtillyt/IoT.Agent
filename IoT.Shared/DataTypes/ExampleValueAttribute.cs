using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.DataTypes
{
    public class ExampleValueAttribute:Attribute
    {
        public ExampleValueAttribute(string exampleValue)
        {
            ExampleValue = exampleValue;
        }

        public string ExampleValue {get;set; }
    }
}
