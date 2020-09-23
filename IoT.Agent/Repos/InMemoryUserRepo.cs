using IoT.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Agent.Repos
{
    public class InMemoryUserRepo : IUserRepo
    {
        private List<UserInfo> _users = new List<UserInfo>();

        public void Add(UserInfo userInfo)
        {
            _users.Add(userInfo);
        }

        public List<UserInfo> ListAll()
        {
            return _users.ToList();
        }
    }
}
