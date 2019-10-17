import React, { SFC } from 'react';

const App: SFC<{}> = ({ children }) => (
  <>
    <div>Hello</div>
    {children}
  </>
);
export default App;