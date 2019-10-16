using Redux;
using System.Threading.Tasks;
using System;
using Dispatch = System.Func<object, object>;
namespace redux.test
{
    using ThunkFuncAsync = Func<Dispatch, Func<object>, Task<object>>;
    using static Selectors;

    /// <summary>
    /// OO Store's Slice
    /// </summary>
    class Slice
  {
    public interface IState
    {
      string Name { get; set; }
      Exception Error { get; set; }
    }
    class State : IState
    {
      public string Name { get; set; }
      public Exception Error { get; set; }
    };
    public readonly IState DefaultState = new State { Name = "?" };
   
    public (string Rename, string Error ) ActionTypes;
    
    private Func<object, IState> _selector;

    public string StoreKey { get; }
    public Slice(string storeKey)
    {
      StoreKey = storeKey;
      ActionTypes = (Rename: $"{storeKey}/Renamne", Error: $"{storeKey}/Error");
      _selector = CreateGenericSelector(storeKey, DefaultState);
    }
    public IState Selector(object state)
    {
      return _selector.Invoke(state);
    }
    public object Reducer(object state, object action)
    {
      state = state ?? DefaultState;
      if (Redux.Actions.IsActionType(action, ActionTypes.Rename))
      {
        // Do somethihng with the state
        return new State
        {
          Name = (action as IAction)?.Payload as string,
          Error = (state as IState)?.Error
        };
      }
      if (Redux.Actions.IsActionType(action, ActionTypes.Error))
      {
        return new State
        {
          Name = (state as IState)?.Name,
          Error = (action as IAction).Payload as Exception
        };
      }
      return state;
    }
    // Action Creator 
    public object Rename(string newName)
    {
      return Redux.Actions.CreateAction(ActionTypes.Rename, newName);
    }

    public ThunkFuncAsync RenameAsync(string name, int wait = 100)
    {
      return async (dispatch, getState) =>
      {
        await Task.Delay(wait);
        return dispatch(Rename(name));
      };
    }

  }
}