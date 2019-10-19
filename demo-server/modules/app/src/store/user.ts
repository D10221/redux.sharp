export const STORE_KEY = "user";

function parseCookie(cookie: string) {
  return cookie.split(";").reduce(
    (prev, next) => {
      const parts = next.split("=");
      const key = parts[0];
      if (key !== "user") return prev;
      const value = parts[1];
      try {
        return JSON.parse(atob(decodeURIComponent(value)));
      } catch (e) {
        console.error(e, value);
      }
      return prev;
    },
    {} as { [key: string]: any },
  );
}

const initialState =
  typeof document != "undefined" &&
  document.cookie &&
  parseCookie(document.cookie);

export const reducer = (state: any = initialState, action: any) => state;
export const selector = (state: any) => state[STORE_KEY];
