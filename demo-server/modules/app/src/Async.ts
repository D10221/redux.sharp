import React, { ComponentType, SFC, useState, useEffect } from "react";
type DefaultModule<T> = { default: T };
const Async = <T = {}>(
  importComponent: () => Promise<DefaultModule<ComponentType<T>>>,
  loading?: ComponentType<T> | null | undefined,
): SFC<T> => {
  return props => {
    loading = loading || (() => null);
    const [component, setComponent] = useState<ComponentType<T> | null>(null);
    function load() {
      if (component) return;
      importComponent().then(({ default: c }) => setComponent(c));
    }
    useEffect(load);
    return React.createElement(component || loading, props);
  };
};
/** */
export default Async;
