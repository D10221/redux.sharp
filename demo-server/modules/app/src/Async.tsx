import React, { Component } from "react";

export default function Async(
  importComponent: () => Promise<{ default: any }>,
) {
  class AsyncComponent extends Component {
    state = { component: null as any };
    async componentDidMount() {
      const { default: component } = await importComponent();
      this.setState({
        component: component,
      });
    }

    render() {
      const C = this.state.component;
      return C ? <C {...this.props} /> : null;
    }
  }

  return AsyncComponent;
}
