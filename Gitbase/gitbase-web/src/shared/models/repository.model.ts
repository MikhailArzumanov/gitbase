export class Repository {
  id         : number ;
  ownerId    : number ;
  isPrivate  : boolean;
  name       : string ;
  description : string ;
  constructor(
    id         : number ,
    ownerId    : number ,
    isPrivate  : boolean,
    name       : string ,
    desciption : string ,
  ){
    this.id         = id         ;
    this.ownerId    = ownerId    ;
    this.isPrivate  = isPrivate  ;
    this.name       = name       ;
    this.description = desciption ;
  }
}