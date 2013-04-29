using ATM.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services
{
    public interface IQuoteService
    {
        void SaveQuote(Quote quote);
        Quote GetById(string id);
        //User GetByUsername(string username);
        //ICollection<User> GetAllUsers();
        //User Authenticate(string username, string password);
        //User CreateUser();
    }
}
