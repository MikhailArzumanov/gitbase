import { API_HOSTNAME, API_PORT, API_PROTOCOL } from "@/config";
import { JSON_CONTENT_TYPE } from "@/shared/globals/content-type.globals";
import { CONTENT_TYPE_HEADER_KEY } from "@/shared/globals/http-headers.globals";
import type { HttpClient } from "@/shared/modules/http/http";

export class BaseService{
  protected http: HttpClient;
  protected SERVICE_URL = 
    `${API_PROTOCOL}://${API_HOSTNAME}:${API_PORT}/api/`;
  
  constructor(http: HttpClient){
    this.http = http;
  }

  getBaseHeaders(contentType: string = JSON_CONTENT_TYPE) : any {
    let headers: any = {};
        headers[CONTENT_TYPE_HEADER_KEY] = contentType;
    return headers;
  }
}
