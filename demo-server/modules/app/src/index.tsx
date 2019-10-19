import "./styles.css";
import * as React from "react";
import { render } from "react-dom";
import { Provider } from "react-redux";
import store from "./store";
import Routes from "./Routes";
import App from "./App";
import { BrowserRouter } from "react-router-dom";
render(
  <BrowserRouter>
    <Provider store={store}>
      <App>
        <Routes />
      </App>
    </Provider>
  </BrowserRouter>,
  document.getElementById("root"),
);

// import("./registerServiceWorker").then(({default: registerServiceWorker})=> registerServiceWorker());
