import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {share, tap} from 'rxjs/operators';
import {environment} from '../../../../environments/environment';
import {HttpOptionsService} from './http-options.service';
import {SuccessResponseHandlerService} from './success-response-handler.service';
import {Options} from '../interfaces/options.interface';
import {isBoolean, isNumber} from 'util';
import {HttpSimpleParams} from '../interfaces/http-simple-params.interface';

@Injectable()
export class HttpService {

    constructor(
        protected http: HttpClient,
        protected successHandler: SuccessResponseHandlerService,
        protected httpOptionsService: HttpOptionsService
    ) {
    }

    public get(url: string, options: Options = {}, params?: any): Observable<any> {
        return this.http.get(this.getFullUrl(url), this.httpOptionsService.getOptions(this.prepareParams(params), options))
            .pipe(
                tap(this.handleSuccessResponse.bind(this))
            );
    }

    public post(url: string, options: Options = {}, params?: any): Observable<any> {
        return this.http.post(this.getFullUrl(url), params, this.httpOptionsService.getOptions(null, options))
            .pipe(
                tap(this.handleSuccessResponse.bind(this)),
            );
    }

    public put(url: string, options: Options = {}, params?: any | any): Observable<any> {
        return this.http.put(this.getFullUrl(url), params, this.httpOptionsService.getOptions(null, options))
            .pipe(
                tap(this.handleSuccessResponse.bind(this))
            );
    }

    public delete(url: string, options: Options = {}, params?: any): Observable<any> {
        return this.http.delete(this.getFullUrl(url), this.httpOptionsService.getOptions(this.prepareParams(params), options))
            .pipe(
                tap(this.handleSuccessResponse.bind(this)),
            );
    }

    public blob(url: string, params?: any): Observable<any> {
        return this.http.get(this.getFullUrl(url), {
            ...this.httpOptionsService.getOptions(params),
            responseType: 'blob'
        }).pipe(
            tap(this.handleSuccessResponse.bind(this)),
            share()
        );
    }

    private get restClientUrl() {
        return environment.apiUrl;
    }

    private getFullUrl(url: string): string {
        return `${this.restClientUrl}/${url}`;
    }

    private handleSuccessResponse(res: any): void {
        this.successHandler.handle(res);
    }

    private prepareParams(params: any | HttpParams): HttpSimpleParams {
        for (const i in params) {
            if (isBoolean(params[i]) || isNumber(params[i])) {
                params[i] = params[i].toString();
            }
            params[i] = JSON.stringify(params[i]);
        }

        return <HttpSimpleParams>params;
    }
}
