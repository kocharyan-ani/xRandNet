import {Injectable} from '@angular/core';
import {HttpOptions} from '../interfaces/http-options.interface';
import {HttpHeaders} from '@angular/common/http';
import {HttpSimpleParams} from '../interfaces/http-simple-params.interface';
import {Options} from '../interfaces/options.interface';
import {DataType} from '../enums/data-type';

@Injectable()
export class HttpOptionsService {
    private options: HttpOptions = {
        headers: null,
        observe: null,
        params: null,
        reportProgress: null,
        responseType: 'json',
        withCredentials: null
    };

    public handleError: boolean;
    public model: any;
    public dataType: DataType;
    public handleSuccessResponse: boolean;

    constructor() {
        this.options.headers = new HttpHeaders({
            'Accept': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': '*',
            'Content-Type': 'application/json; charset=UTF-8'
        });

    }

    public getOptions(params?: HttpSimpleParams, options: Options = {}): HttpOptions {
        this.mergeOptions(options);

        this.setParams(params);

        return this.options;
    }

    public setOptions(options: HttpOptions): void {
        this.options = options;
    }

    private setHeader(name: string, value: string): void {
        this.options.headers = this.options.headers.set(name, value);
    }

    public setAuth(token: string): void {
        this.setHeader('Authorization', `Bearer ${token}`);
    }

    private mergeOptions(options: Options): void {
        for (const i in options) {
            if (this.options.hasOwnProperty(i)) {
                this.options[i] = options[i];
            }
        }
        this.handleError = options.handleError;
        this.handleSuccessResponse = options.handleSuccessResponse;
        this.model = options.model;
        this.dataType = options.dataType;
    }

    private setParams(params: HttpSimpleParams): void {
        this.options.params = params || null;
    }

    public removeAuth(): void {
        this.options.headers = this.options.headers.delete('Authorization');
    }
}
