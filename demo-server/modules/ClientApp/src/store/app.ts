import { Reducer } from "redux";

export const STOKE_KEY = "@app";
export const actionTypes = {
  NONE: `${STOKE_KEY}/NONE`
}

export interface State {};

const initialState: State = {};

export const actions = {
  none: () => ({
    type: actionTypes.NONE
  })
};

export type Actions = typeof actions;

export const reducer: Reducer = (state = initialState, action) => {  
  switch (action.type) {
    case actionTypes.NONE: {
      return state;
    }
    default: return state;
  }
}

export const selector = (state: any): State => state[STOKE_KEY];

