import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HttpService} from './services/http.service';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {SuccessResponseHandlerService} from './services/success-response-handler.service';
import {ErrorHandlerService} from './services/error-handler.service';
import {HttpOptionsService} from './services/http-options.service';
import {HandlerService} from './services/handler.service';
import {ErrorHandlerInterceptor} from './interceptors/error-handler.interceptor';
import {TransformerInterceptor} from './interceptors/transformer.interceptor';
import {NotificationModule} from "../notification/notification.module";
import {JwtInterceptor} from "./interceptors/jwt-interceptor";

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        NotificationModule
    ],
    declarations: [],
})
export class HttpModule {
    static forRoot() {
        return {
            ngModule: HttpModule,
            providers: [
                HttpService,
                SuccessResponseHandlerService,
                ErrorHandlerService,
                HttpOptionsService,
                HandlerService,
                {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
                // {provide: HTTP_INTERCEPTORS, useClass: AuthErrorInterceptor, multi: true},
                {provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true},
                {provide: HTTP_INTERCEPTORS, useClass: TransformerInterceptor, multi: true}
            ]
        };
    }
}
