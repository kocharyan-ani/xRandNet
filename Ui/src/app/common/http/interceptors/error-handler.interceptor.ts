import {Injectable} from '@angular/core';
import {HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse} from '@angular/common/http';
import {catchError} from 'rxjs/operators';
import {HttpOptionsService} from '../services/http-options.service';
import {ErrorHandlerService} from '../services/error-handler.service';
import {HandledError} from '../classes/handled-error';
import {Observable} from 'rxjs';

@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {

    constructor(
        private httpOptions: HttpOptionsService,
        private errorHandler: ErrorHandlerService,
    ) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<HandledError>> {
        return next.handle(req)
            .pipe(
                catchError(this.handleError.bind(this))
            );
    }

    private handleError(err: HttpErrorResponse): Observable<HandledError> {
        return this.errorHandler.handle(err);
    }
}
