using IoT.Shared.Entities;
using System.Collections.Generic;

namespace IoT.Agent.Repos
{
    public interface IUserRepo
    {
        void Add(UserInfo userInfo);
        List<UserInfo> ListAll();
    }
}