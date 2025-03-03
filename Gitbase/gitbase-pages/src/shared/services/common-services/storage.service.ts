import { USER_STORAGE_KEY } from "@/shared/globals/storage.globals";
import type { User } from "@/shared/models/user.model";

export class StorageService {
  private user: User | null = null;

  setUser(user : User) : void {
    this.user = user;
    let userJson = JSON.stringify(user);
    localStorage.setItem(USER_STORAGE_KEY, userJson);
  }

  getUser() : User | null {
    let userJson = localStorage.getItem(USER_STORAGE_KEY);
    this.user = userJson ? JSON.parse(userJson) as User : null;
    return this.user;
  }
}