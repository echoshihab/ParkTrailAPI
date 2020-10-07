using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkApi.Models.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUniqueUser(string username);
        User Authenticate(string username, string password);
        User Register(string username, string password);
    }
}
