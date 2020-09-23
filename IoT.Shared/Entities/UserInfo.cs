using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Entities
{
    public class UserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClientId { get; set; }
        public string SignalrConnectionId { get; set; }
    }
}
