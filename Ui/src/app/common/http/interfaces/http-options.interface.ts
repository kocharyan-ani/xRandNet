import {HttpHeaders} from '@angular/common/http';
import {HttpSimpleParams} from './http-simple-params.interface';

export interface HttpOptions {
    headers?: HttpHeaders;
    observe?: 'body';
    params?: HttpSimpleParams;
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
}

