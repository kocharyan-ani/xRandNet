import {Injectable} from '@angular/core';
import {HttpRequest, HttpHandler, HttpEvent, HttpInterceptor} from '@angular/common/http';
import {Observable, of, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';
import {AuthenticationService} from "../../../main/services/authentication-service/authentication.service";
import {Router} from "@angular/router";


@Injectable()
export class AuthErrorInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService, private  router: Router) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((err, caught) => {
                if (err.code === 401) {
                    this.handleAuthError();
                    return of(err);
                }
                throw err;
            }));
    }

    private handleAuthError() {
        this.authenticationService.logout()
        this.router.navigateByUrl('login');
    }
}