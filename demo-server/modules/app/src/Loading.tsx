import React, { SFC } from "react";

const Loading: (route: string) => SFC<{}> = route => () => {
  return <div> ... loading {route}</div>;
};

export default Loading;