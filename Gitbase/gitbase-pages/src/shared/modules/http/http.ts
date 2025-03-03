import {ParamValidator} from "./ParamValidator";

export class HttpClient {
  errorsService: any;

  constructor(errorsService: any) {
    this.errorsService = errorsService;
  }

  getParamsStr(params : any) : string {
    let res = '?'
    Object.keys(params).forEach(key => {
      let theKey = ParamValidator.validateParamsStr(key);
      let value  = ParamValidator.validateParamsStr(params[key]);
      res += theKey+'='+value+'&'
    });
    let l = res.length
    res = res.substring(0,l-1)
    return res
  }

  getHeadersObj(headers : any) : any {
    let headersObj : any = {};
    Object.keys(headers).forEach(key => {
      headersObj[key] = headers[key];
    });
    return headersObj;
  }

  baseRequest(plainUrl : string, method : string, 
      theHeaders = {}, params = {}, theBody = null) : Promise<Response>{
    let url = plainUrl+this.getParamsStr(params);
    let options : any = {
      method: method,
      headers: theHeaders,
    };
    if(theBody != null){
      options.body = JSON.stringify(theBody);
    }
    return fetch(url, options);
  }

  fileRequest(plainUrl : string, method : string, 
      params : any, theBody : any) : Promise<Response>{
    let url = plainUrl+this.getParamsStr(params);
    let options = {
      method: method,
      body: theBody,
    };
    return fetch(url, options);
  }
  

  async wrapper(method: string, url: string, headers: any, params: any, 
        plain: boolean, body?: any, baseRequest? : any): 
      Promise<string | any | null>{
    const reqFn = baseRequest ? baseRequest : this.baseRequest.bind(this);
    const response = await reqFn(url, method, headers, params, body)
    if(response.ok == true){
      const data = plain 
        ? await response.text() as string 
        : await response.json() as any;
      return data
    }
    else {
      const errorMsg = await response.text() as string;
      this.errorsService.pushError(errorMsg);
      console.log(`${method}: FETCH ERROR, URL: ${url}`)
    }
    return null;
  }

  async get(url : string, headers: any = {}, params: any = {}, plain = false): 
      Promise<string | any | null>{
    let method = 'GET';
    return this.wrapper(method, url, headers, params, plain);
  }

  async post(url : string, headers: any = {}, params: any = {}, body: any = null, plain = false): 
      Promise<string | any | null>{
    let method = 'POST';
    return this.wrapper(method, url, headers, params, plain, body);
  }
  async put(url: string, headers: any = {}, params: any = {}, body: any = null, plain = false): 
      Promise<string | any | null>{
    let method = 'PUT';
    return this.wrapper(method, url, headers, params, plain, body);
  }
  async delete(url: string, headers: any = {}, params: any = {}, body: any = null, plain = false): 
      Promise<string | any | null>{
    let method = 'DELETE';
    return this.wrapper(method, url, headers, params, plain, body);
  }
    
  async file(url: string, method = 'POST', headers: any = {}, params: any = {}, body: any = null, plain = false): 
      Promise<string | any | null>{
    return this.wrapper(method, url, headers, params, plain, body, this.fileRequest.bind(this));
  }
}