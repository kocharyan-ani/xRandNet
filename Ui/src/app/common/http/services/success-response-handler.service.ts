import {Injectable} from '@angular/core';
import {HandlerService} from './handler.service';
import {NotificationType} from '../../notification/enums/notification-type';

@Injectable({
    providedIn: 'root'
})
export class SuccessResponseHandlerService {

    constructor(private handlerService: HandlerService) {
    }

    public handle(res: any): void {
        if (this.handlerService.httpOptionsService.handleSuccessResponse) {
            this.handlerService.addNotification(res[0], NotificationType.SUCCESS);
        }
    }
}
