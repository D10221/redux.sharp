import {
  createStore,
  combineReducers,
  applyMiddleware,
  compose as reduxCompose,
} from "redux";
import thunk from "redux-thunk";
import * as user from "./user";
import * as root from "./root";
/** */
const reducers = {
  [root.STORE_KEY]: root.reducer,
  [user.STORE_KEY]: user.reducer,
};
/** */
const compose =
  (process.env.NODE_ENV !== "production" &&
    (window as any).__REDUX_DEVTOOLS_EXTENSION_COMPOSE__) ||
  reduxCompose;
/** */
const store = createStore(
  combineReducers(reducers),
  (typeof window !== "undefined" && (window as any).INITIAL_STATE) || {},
  compose(applyMiddleware(thunk)),
);
export default store;
