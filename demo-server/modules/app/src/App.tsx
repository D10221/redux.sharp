import * as React from 'react';
import { Route, Switch, BrowserRouter } from 'react-router-dom';
import Async from './Async';

export default () => (
    <BrowserRouter>
        <Switch>
            <Route path='*' component={Async(()=> import(  /* webpackChunkName: "home" */ "./Home"))} />
        </Switch>
    </BrowserRouter>
);
