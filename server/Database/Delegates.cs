using System.Threading.Tasks;
using System.Collections.Generic;

namespace server.Database
{
    public delegate Task<object> Add<T>(T entity);
    public delegate Task<T> Get<T>(int id);
    public delegate Task<IEnumerable<T>> All<T>();
    public delegate Task Update<T>(T entity);
    public delegate Task Delete<T>(T entity);
    public delegate Task<int> Count<T>(params object[] args);
    //?
    public delegate Task<IEnumerable<T>> GetWhere<T>(string @where);
}