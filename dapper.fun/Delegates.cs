using System.Data;
using System.Threading.Tasks;

namespace dapper.fun
{
    public delegate Task<R> Selector<P, R>(P param);
    public delegate Task<R> Selector<R>();

    public delegate Selector<P, R> Select<P, R>(IDbConnection connection, IDbTransaction transaction);
    public delegate Selector<R> Select<R>(IDbConnection connection, IDbTransaction transaction);

    public delegate Select<P, R> SelectFty<P, R>(QueryString query);
    public delegate Select<R> SelectFty<R>(QueryString query);
}