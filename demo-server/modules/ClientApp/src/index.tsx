
import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";
import store from "./store";
import App from "./App";
import { BrowserRouter, Switch, Route } from "react-router-dom";
import Home from "./Home";

const rootElement = document.getElementById("root");

ReactDOM.render(
  <Provider store={store}>
    <BrowserRouter >
      <App >
        <Switch>
          <Route path="*" component={Home} />
        </Switch>
      </App>
    </BrowserRouter>
  </Provider>,
  rootElement);

// import("./registerServiceWorker").then(({default: registerServiceWorker})=> registerServiceWorker());
