export class Page<T> {
  pageNumber : number;
  pagesCount : number;
  pageSize   : number;
  entries : T[];
  constructor(page: number, pagesCount: number, pageSize: number, entries: T[]){
    this.pageNumber = page       ;
    this.pagesCount = pagesCount ;
    this.pageSize   = pageSize   ;
    this.entries    = entries    ;
  }
}