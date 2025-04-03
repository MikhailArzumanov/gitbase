import type { Token } from "../token.model";
import type { User } from "../user.model";

export class AuthResponse {
  token : string ;
  user  : User  ;
  constructor(token: string, user: User) {
    this.token = token ;
    this.user  = user  ;
  }
}