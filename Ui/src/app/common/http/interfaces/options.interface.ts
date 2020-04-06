import {HttpHeaders, HttpParams} from '@angular/common/http';
import {HttpSimpleParams} from './http-simple-params.interface';
import {DataType} from '../enums/data-type';

export interface Options {
    headers?: HttpHeaders;
    observe?: 'body';
    params?: HttpParams | HttpSimpleParams;
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
    handleError?: boolean;
    handleSuccessResponse?: boolean;
    model?: any;
    dataType?: DataType;
}
