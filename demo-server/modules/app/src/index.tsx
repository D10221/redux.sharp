/// <reference path="./globals.d.ts" />
import "./styles.css";
import * as React from "react";
import { render } from "react-dom";
import { Provider } from "react-redux";
import store from "./store";
import App from "./App";

render(
    <Provider store={store}>
        <App/>
    </Provider>,
    document.getElementById("root"));

// import("./registerServiceWorker").then(({default: registerServiceWorker})=> registerServiceWorker());
