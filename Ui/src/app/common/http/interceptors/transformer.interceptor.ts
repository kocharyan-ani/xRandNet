import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse} from '@angular/common/http';
import {map} from 'rxjs/operators';
import {HttpOptionsService} from '../services/http-options.service';
import {TransformParams} from '../interfaces/transform-params.interface';
import {Observable} from 'rxjs';
import {DataType} from '../enums/data-type';
import {BaseModel} from '../models/base-model';

@Injectable()
export class TransformerInterceptor implements HttpInterceptor {
    private params: TransformParams;

    constructor(
        private httpOptions: HttpOptionsService,
    ) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<BaseModel>> {
        const params: TransformParams = this.getParams();
        return next.handle(request)
            .pipe(
                map((res: HttpResponse<any> | { type: number }) => this.transform(res, params)),
            );
    }

    private getParams(): TransformParams {
        this.params = {
            model: this.httpOptions.model,
            dataType: this.httpOptions.dataType,
        };

        return this.params;
    }

    private transform(res: HttpResponse<any> | { type: number }, params: TransformParams): HttpEvent<BaseModel> {
        if (!params.model) {
            return res;
        }

        if (res instanceof HttpResponse) {
            let body = res.body;
            switch (params.dataType) {
                case DataType.LISTING:
                    body = params.model.transformCollection(body);
                    break;
                case DataType.PAGINATION:
                    body.items = params.model.transformCollection(body.items);
                    break;
                default:
                    body = params.model.transform(body);
                    break;
            }
            return res.clone<BaseModel>({body: body});
        } else {
            return res;
        }
    }
}
