import type { Token } from "../token.model";
import type { User } from "../user.model";

export class AuthResponse {
  token : Token ;
  user  : User  ;
  constructor(token: Token, user: User) {
    this.token = token ;
    this.user  = user  ;
  }
}