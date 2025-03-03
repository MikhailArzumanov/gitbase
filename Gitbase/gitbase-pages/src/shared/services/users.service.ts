import { AuthData } from "../models/auth-data.model";
import type { AuthResponse } from "../models/responses/auth.response";
import type { User } from "../models/user.model";
import { AuthorizedService } from "./base-services/authorized.service";

export class UsersService extends AuthorizedService {
  CONTROLLER_NAME = 'users';
  CONTROLLER_URL  = this.SERVICE_URL+this.CONTROLLER_NAME;

  async auth(authname: string, password: string) : Promise<AuthResponse | null>{
    let METHOD_NAME = 'auth';
    let url = `${this.CONTROLLER_URL}/${METHOD_NAME}`;
    let headers = this.getBaseHeaders();
    let params = {};
    let body = new AuthData(authname, password);

    return await this.http.post(url, headers, params, body, false);
  }

  async register(user: User) : Promise<User | null>{
    let METHOD_NAME = 'create';
    let url = `${this.CONTROLLER_URL}/${METHOD_NAME}`;
    let headers = this.getBaseHeaders();
    let params = {};
    let body = user;

    return await this.http.post(url, headers, params, body, false);
  }
}