import type { AuthData } from "@/shared/models/auth-data.model.js";
import {BaseService} from "./base.service.js";
import type { Token } from "@/shared/models/token.model.js";
import { JSON_CONTENT_TYPE } from "@/shared/globals/content-type.globals.js";
import { TOKEN_STORAGE_KEY } from "@/shared/globals/storage.globals.js";
import { TOKEN_HEADER_KEY } from "@/shared/globals/http-headers.globals.js";

export const TOKEN_KEY              = 'token'
export const AUTHORIZATION_DATA_KEY = 'authorizationData'

export class AuthorizedService extends BaseService{
  private static token?: Token;

  protected getTokenHeaders(contentType: string = JSON_CONTENT_TYPE): any {
    let token   = this.getToken;

    let headers = this.getBaseHeaders(contentType);
        headers[TOKEN_HEADER_KEY] `Bearer ${token}`;
    return headers;
  }
  
  private loadToken(){
    if (!AuthorizedService.token) {
      let tokenJSON = sessionStorage.getItem(TOKEN_STORAGE_KEY) ?? 'null';
      AuthorizedService.token = JSON.parse(tokenJSON);
    }
  }
  
  protected get getToken(): string {
    this.loadToken();
    return AuthorizedService.token?.self ?? 'null';
  }
  
  public get getTokenObj(): Token | undefined {
    this.loadToken();
    return AuthorizedService.token;
  }
  
  public setToken(token: Token) {
    AuthorizedService.token = token;
    sessionStorage.setItem(TOKEN_STORAGE_KEY, JSON.stringify(token));
  }
  
  public setTokenStr(tokenStr: string){
    sessionStorage.setItem(TOKEN_STORAGE_KEY, tokenStr);
  }
  
  public clearAuth(){
    AuthorizedService.token = undefined;
    sessionStorage.removeItem(TOKEN_STORAGE_KEY);
  }

}
