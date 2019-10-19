import React, { ComponentType, SFC, useState, useEffect } from "react";
type DefaultModule<T> = { default: T };
/** */
const AsyncComponent = <T = {}>(
  loading?: ComponentType<T> | null | undefined,
) => (
  importComponent: () => Promise<DefaultModule<ComponentType<T>>>,
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
export default AsyncComponent;
