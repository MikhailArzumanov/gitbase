import { AuthData } from "../models/auth-data.model";
import type { Page } from "../models/page.model";
import type { Repository } from "../models/repository.model";
import type { AuthResponse } from "../models/responses/auth.response";
import type { User } from "../models/user.model";
import { AuthorizedService } from "./base-services/authorized.service";

export class RepositoriesService extends AuthorizedService {
  CONTROLLER_NAME = 'repositories';
  CONTROLLER_URL  = this.SERVICE_URL+this.CONTROLLER_NAME;

  async getUsersRepositories(userId: number, page: number, pageSize: number) : 
  Promise<Page<Repository> | null>{
    let METHOD_NAME = `public/owned_by/${userId}`;
    let url = `${this.CONTROLLER_URL}/${METHOD_NAME}`;
    let headers = this.getTokenHeaders();
    let params = {
      pageNumber: page, 
      pageSize: pageSize
    };

    return await this.http.get(url, headers, params, false);
  }

  async createSelfRepository(rep: Repository) : Promise<Repository | null> {
    let METHOD_NAME = `self/create`;
    let url = `${this.CONTROLLER_URL}/${METHOD_NAME}`;
    let headers = this.getTokenHeaders();
    let params = {};
    let body = rep;

    return await this.http.post(url, headers, params, body, false);
  }

  async deleteSelfRepository(repId: number) : Promise<Repository | null> {
    let METHOD_NAME = `self/remove/${repId}`;
    let url = `${this.CONTROLLER_URL}/${METHOD_NAME}`;
    let headers = this.getTokenHeaders();
    let params = {};
    let body = {};

    return await this.http.delete(url, headers, params, body, false);
  }

}