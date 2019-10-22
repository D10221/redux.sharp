using FluentAssertions;
using Redux;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace redux.test
{
    using static Redux.Utils;
    using static Redux.Thunks;
    using static Redux.Actions;
    using static Redux.Store;
    using static Redux.Selectors;
    using static Redux.Reducers;
    using static Redux.Middlewares;

    public class ThunkTest
    {
        [Fact]
        public void ItCanThunk()
        {
            var Slice = Fty((string storeKey) =>
            {
                (string Name, Exception Error) initialState = (Name: "", Error: null);

                var selector = CreateSelector(state => state as (string Name, Exception Error)?);

                var actionTypes = (
                    Rename: $"{storeKey}/RENAME",
                    SetError: $"{storeKey}/SET_ERROR"
                );

                // untyped reducer
                var reducer = CreateReducer((state, action) =>
                {
                    switch (action)
                    {
                        case IAction a when Equals(actionTypes.Rename, a.Type):
                            {
                                var (Name, Error) = (((string Name, Exception Error))state);
                                return (Name: a.Payload as string, Error);
                            }
                        case IAction a when Equals(actionTypes.SetError, a.Type):
                            {
                                var (Name, Error) = (((string Name, Exception Error))state);
                                return (Name, Error: a.Payload as Exception);
                            }
                        default: return state;
                    }
                }, initialState);

                var actions = new
                {
                    Rename = ThunkFty<string>(newName => (dispatch, getState) =>
              {
                  try
                  {
                      if (selector(getState())?.Name == newName)
                      {
                          throw new Exception($"Name {newName} is already {newName}");
                      }
                      return dispatch(CreateAction(actionTypes.Rename, newName));
                  }
                  catch (Exception ex)
                  {
                      return dispatch(CreateAction(actionTypes.SetError, ex));
                  }
              })
                };
                Middleware middleware = null;
                return (storeKey, selector, reducer, actions, actionTypes, initialState, middleware);
            });

            {
                var (storeKey, selector, reducer, actions, actionTypes, initialState, _) = Slice("X");

                var store = CreateStore(reducer, initialState, ApplyMiddleware(Thunks.Middleware));

                store.dispatch(actions.Rename("Bob"));
                selector(store.getState())?.Name.Should().Be("Bob");

                store.dispatch(actions.Rename("Bob"));
                selector(store.getState())?.Name.Should().Be("Bob");
                var error = selector(store.getState())?.Error;
                error.Should().NotBeNull();
                error.Should().BeOfType<Exception>();
                error.Message.Should().Be("Name Bob is already Bob");
            }
        }

        [Fact]
        public async void Fact1()
        {
            var slice = new Slice("x");

            var reducers = new Dictionary<string, Reducer>
            {
                [slice.StoreKey] = slice.Reducer,
            };

            var store = CreateStore(
                reducers.Combine(),
                new { },
                ApplyMiddleware(
                    Thunks.Middleware,
                    Async.Middleware
                )
            );

            Dispatch dispatch = store.dispatch;

            Slice.IState getState()
            {
                return slice.Selector(store.getState());
            }
            // 
            var state = getState();
            state.Should().NotBeNull();
            state.Name.Should().Be(slice.DefaultState.Name);

            var _next =await  (dispatch(slice.RenameAsync("bob")) as Task<object>);
            getState().Name.Should().Be("bob");
        }
    }
}