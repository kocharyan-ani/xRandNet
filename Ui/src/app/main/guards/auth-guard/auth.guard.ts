import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {AuthenticationService} from "../../services/authentication-service/authentication.service";
import {JwtHelperService} from "@auth0/angular-jwt";


@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService,
        private jwtHelper: JwtHelperService
    ) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.authenticationService.currentUserValue;
        if (currentUser && currentUser.token && !this.jwtHelper.isTokenExpired(currentUser.token)) {
            return true;
        }
        this.authenticationService.logout()
        this.router.navigate(['/login'], {queryParams: {returnUrl: state.url}});
        return false;
    }
}