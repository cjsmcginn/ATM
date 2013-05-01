using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services
{
    public interface IDataService
    {
        void Save<T>(T entity);
        T Execute<Q, T>(Func<Q, T> operation);
        T Fetch<T>(Func<T, bool> predicate);
        List<T> FetchAll<T>(Func<T, bool> predicate);
        T Load<T>(Guid id);
        
    }
}
