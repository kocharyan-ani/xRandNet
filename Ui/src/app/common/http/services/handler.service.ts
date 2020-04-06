import {Injectable} from '@angular/core';
import {HttpOptionsService} from './http-options.service';
import {NotificationType} from '../../notification/enums/notification-type';
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable()
export class HandlerService {

    constructor(
        public httpOptionsService: HttpOptionsService,
        public notificationService: MatSnackBar
    ) {
    }

    public addNotification(text: string, type: NotificationType = NotificationType.ERROR): void {
        this.notificationService.open(text);
    }
}
