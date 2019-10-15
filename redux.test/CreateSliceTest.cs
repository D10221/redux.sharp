using FluentAssertions;
using Redux;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace redux.test
{
    using static Redux.Factories;
    using static Redux.Thunks;
    using static Redux.Actions;

    public class CreateSliceTest
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
        public void ItCreatesSlice()
        {
            var slice = CreateSlice(() =>
            {
                var storeKey = "@myslice";

                (string name, Exception error, bool busy, bool success) initialState = (name: null, error: null, busy: false, success: false);

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
                                return (name: (string)a.Payload, error, busy, success);
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
                var (storeKey, actionTypes, initialState, actions, selector, reducer, middleware)
                  = slice();
                storeKey.Should().Be("@myslice");

                // TODO
            }
        }
    }
}