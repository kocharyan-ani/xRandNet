import {Injectable} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {HandledError} from '../classes/handled-error';
import {HandlerService} from './handler.service';
import {NotificationType} from '../../notification/enums/notification-type';

@Injectable()
export class ErrorHandlerService {
    public validStatusCodes: number[] = [404, 403, 401, 402, 422, 415, 491];

    constructor(private handlerService: HandlerService) {
    }

    public handle(error: HttpErrorResponse | Error): Observable<HandledError> {

        let res: HandledError;
        if (error instanceof HttpErrorResponse) {
            res = this.handleHttpError(error);
        } else {
            res = this.handleTypeError(error);
        }

        if (this.handlerService.httpOptionsService.handleError) {
            this.handlerService.addNotification(res.toString(), NotificationType.ERROR);
        }

        return throwError(res);
    }

    protected handleHttpError(response: HttpErrorResponse): HandledError {
        const err: HandledError = new HandledError(response.status);

        if (response.error == null) {
            err.text = 'No Message';
        } else if (response.error.Message) {
            if (this.validStatusCodes.indexOf(response.status) !== -1) {
                err.text = response.error.Message;
            }
        } else if (typeof response.error === 'string') {
            err.text = response.error;
        } else {
            if (response.error.errorMessage) {
                err.text = response.error.errorMessage;
            }
        }
        return err;
    }

    protected handleTypeError(error: Error): HandledError {
        console.error(error);
        return new HandledError();
    }
}