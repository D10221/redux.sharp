import { applyMiddleware, combineReducers, compose, createStore } from "redux";
import thunk from "redux-thunk";
import * as app from "./app";

const reducers = {
  [app.STOKE_KEY]: app.reducer,
};

const middleware = [thunk];

declare const __REDUX_DEVTOOLS_EXTENSION_COMPOSE__: any;
const c =
  process.env.NODE_ENV !== "production" &&
  window !== void 0 &&
  __REDUX_DEVTOOLS_EXTENSION_COMPOSE__
    ? __REDUX_DEVTOOLS_EXTENSION_COMPOSE__({
        // Specify extension’s options like name, actionsBlacklist, actionsCreators, serialize...
      })
    : compose;

const store = createStore(
  combineReducers({
    ...reducers,
  }),
  {},
  c(applyMiddleware(...middleware)),
);

export default store;
