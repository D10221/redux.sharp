import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import * as app from './app';

const reducers = {
  [app.STOKE_KEY]: app.reducer
};

const middleware = [
  thunk  
];

// In development, use the browser's Redux dev tools extension if installed
const enhancers = [];
const isDevelopment = process.env.NODE_ENV === 'development';
if (isDevelopment && typeof window !== 'undefined' && (window as any).devToolsExtension) {
  enhancers.push((window as any).devToolsExtension());
}

const store = createStore(
  combineReducers({
    ...reducers,  
  }),
  {},
  compose(applyMiddleware(...middleware), ...enhancers)
);

export default store;