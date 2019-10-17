import * as React from 'react';
import { Route, Switch, BrowserRouter } from 'react-router-dom';
import Home from './Home';

export default () => (
    <BrowserRouter>
        <Switch>
            <Route path='*' component={Home} />
        </Switch>
    </BrowserRouter>
);
