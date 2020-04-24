import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {AuthenticationService} from '../../services/authentication-service/authentication.service';
import {JwtHelperService} from "@auth0/angular-jwt";
import {Role} from "../../../common/models/role";

@Injectable({
    providedIn: 'root'
})
export class AdminGuard implements CanActivate {
    constructor(
        private router: Router,
        private jwtHelperService: JwtHelperService,
        private authenticationService: AuthenticationService
    ) {
    }


    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.authenticationService.currentUserValue;
        if (currentUser && currentUser.token
            && !this.jwtHelperService.isTokenExpired(currentUser.token)
            && this.jwtHelperService.decodeToken(currentUser.token).role == Role.ADMIN) {
            return true;
        }
        this.authenticationService.logout()
        this.router.navigate(['/login'], {queryParams: {returnUrl: state.url}});
        return false;

    }
}
