import React from "react";
import { Route, Switch } from "react-router-dom";
import AsyncComponent from "./AsyncComponent";
import Loading from "./Loading";

const Home = () => import(/* webpackChunkName: "home" */ "./Home");

export default function Routes() {
  return (
    <Switch>
      <Route path="*" component={AsyncComponent(Loading("Home"))(Home)} />
    </Switch>
  );
}
