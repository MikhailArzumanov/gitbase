import { TOKEN_STORAGE_KEY, USER_STORAGE_KEY } from "../globals/storage.globals";

export function isAuthorized() : boolean {
  if(localStorage.getItem(USER_STORAGE_KEY) 
    && localStorage.getItem(TOKEN_STORAGE_KEY)
  ){
    return true;
  }
  else return false;
}