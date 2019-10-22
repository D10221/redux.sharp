using FluentAssertions;
using Newtonsoft.Json;
using Redux;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace redux.test
{
    using static Redux.Slices;
    using static Redux.Selectors;
    using static Redux.Reducers;
    using static Redux.Middlewares;
    using static Redux.Store;
    using static Redux.ActionCreators;    

    public partial class CreateSliceTest
    {
        class Api
        {
            public static async Task<string> SetName()
            {
                await Task.Delay(500);
                return "Remote bob!";
            }
        }

        [Fact]
        public void ItWorks()
        {
            var slice = CreateSlice(() =>
            {
                var storeKey = "myslice";

                MyRecord initialState = new MyRecord();

                var actionTypes = (
                rename: $"{storeKey}/rename",
                error: $"{storeKey}/error",
                start: $"{storeKey}/start",
                started: $"{storeKey}/started",
                completed: $"{storeKey}/completed",
                success: $"{storeKey}/success"
                );

                var actions = (
                  rename: CreateActionCreator<string>(actionTypes.rename),
                  error: CreateActionCreator<Exception>(actionTypes.error),
                  start: CreateActionCreator(actionTypes.start), // Starts middleware
                  started: CreateActionCreator(actionTypes.started),
                  success: CreateActionCreator<object>(actionTypes.success),
                  completed: CreateActionCreator(actionTypes.completed)
                  );

                var selector = CreateGenericSelector(storeKey, initialState);

                var reducer = CreateReducer(initialState, (state, action) =>
                {
                    switch (action)
                    {
                        case IAction a when Equals(actionTypes.rename, a.Type):
                            {
                                var (name, error, busy, success) = selector(state);
                                return (name: a.Payload as string, error, busy, success);
                            }
                        case IAction a when Equals(actionTypes.error, a.Type):
                            {
                                var (name, error, busy, success) = selector(state);
                                return (name, error: (Exception)a.Payload, busy, success);
                            }
                        case IAction a when Equals(actionTypes.started, a.Type):
                            {
                                var (name, error, busy, success) = selector(state);
                                return (name, error: null, busy: true, success: false);
                            }
                        case IAction a when Equals(actionTypes.completed, a.Type):
                            {
                                var (name, error, busy, success) = selector(state);
                                return (name, error, busy: false, success);
                            }
                        case IAction a when Equals(actionTypes.success, a.Type):
                            {
                                var (name, error, busy, success) = selector(state);
                                return (name: (string)a.Payload, error, busy, success);
                            }
                        default: return state;
                    }
                });

                var middleware = CreateMiddleware(state => next => action =>
                {
                    switch (action)
                    {
                        case IAction a when Equals(actionTypes.start, a.Type):
                            return Task.Factory.StartNew(async () =>
                      {
                          try
                          {
                              next(actions.start);
                              var newName = await Api.SetName();
                              next(actions.success(newName));
                          }
                          catch (Exception ex)
                          {
                              next(actions.error(ex));
                          }
                          return next(actions.completed());
                      });
                        default: return next(action);
                    }
                });

                return (storeKey, actionTypes, initialState, actions, selector, reducer, middleware);
            });

            {
                var (storeKey, actionTypes, initialState, actions, selector, reducer, middleware) = slice();

                var store = CreateStore(
                    CombineReducer(new Dictionary<string, Reducer> { { storeKey, reducer } }),
                    null
                    );

                var rename = BindActionCreator(actions.rename)(store.dispatch);
                rename("?");
                var (name, _, _, _) = selector(store.getState());
                name.Should().Be("?");

                var (rename1, rename2) = BindActionCreators(actions.rename, actions.rename)(store.dispatch);
                rename1("x1");
                selector(store.getState()).Name.Should().Be("x1");
                rename1("x2");
                selector(store.getState()).Name.Should().Be("x2");

                var moreActions = (actions.rename, actions.rename);
                var (f1, f2) = BindActionCreators(moreActions)(store.dispatch);

                f1("x3");
                selector(store.getState()).Name.Should().Be("x3");
                f1("x");
                selector(store.getState()).Name.Should().Be("x");

                MyJsonSettings.Configure();
                var json = JsonConvert.SerializeObject(store.getState());
                var expected = @"{""myslice"":{""name"":""x"",""error"":null,""busy"":false,""success"":false}}";
                json.Should().Be(expected);

                var deserialized = JsonConvert.DeserializeAnonymousType(expected, new { myslice = new MyRecord() });
                deserialized.myslice.Should().NotBeNull();
                deserialized.myslice.Name.Should().Be("x");
            }
        }
    }
}