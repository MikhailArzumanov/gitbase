import type { Repository } from "./repository.model";
import type { SshKey } from "./ssh-key.model";

export class User {
  [key: string] : any;

  id       : number;
  authname : string;
  password : string;

  username : string;
  email    : string;
  about    : string;
  company  : string;
  links    : string;

  isAdmin  : boolean;
  
  sshKeys : SshKey[];
  
  ownedRepositories        : Repository[];
  participatedRepositories : Repository[];

  constructor(
    id       : number,
    authname : string,
    password : string,
    username : string,
    email    : string,
    about    : string,
    company  : string,
    links    : string,
    isAdmin  : boolean,
    sshKeys  : SshKey[],
    ownedRepositories        : Repository[],
    participatedRepositories : Repository[]){
      this.id       = id      ;
      this.authname = authname;
      this.password = password;
      this.username = username;
      this.email    = email   ;
      this.about    = about   ;
      this.company  = company ;
      this.links    = links   ;

      this.isAdmin  = isAdmin ;
  
      this.sshKeys  = sshKeys ;
  
      this.ownedRepositories        = ownedRepositories       ;
      this.participatedRepositories = participatedRepositories;
    }
}

export let getEmptyUser = () => new User(0,'','','','','','','',false,[],[],[]);