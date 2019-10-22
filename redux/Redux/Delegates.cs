using System;

namespace Redux
{
    public delegate object Dispatch(object action);
    public delegate object GetState();
    public delegate Func<Dispatch, Dispatch> Middleware((Dispatch dispatch, GetState getState) api);
    public delegate object Reducer(object state, object action);
    public delegate IDisposable Subscribe(Action subscriber);
    public delegate (Dispatch dispatch, GetState getState, Subscribe susbscribe) Enhance((Dispatch dispatch, GetState getState, Subscribe) store);
}