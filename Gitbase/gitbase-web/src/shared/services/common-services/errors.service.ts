export class ErrorsService{
  errorStack : string[] = [];

  pushError(errorMsg : string) : void {
    this.errorStack.push(errorMsg);
  }

  getLastError() : string | null {
    let error = this.errorStack.pop();
    return error ? error : null;
  }
}