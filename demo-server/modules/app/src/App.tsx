import { SFC } from "react";
import React from "react";
import { withRouter, RouteComponentProps } from "react-router";
interface Props extends RouteComponentProps {

}
const App: SFC<Props> = ({ children, location }) => {
  return (
    <div>
      <div>{location.pathname}</div>
      <hr/>
      {children}
    </div>
  );
};
export default withRouter(App);
