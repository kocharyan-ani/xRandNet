import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {FormBuilder, NgForm} from '@angular/forms';
import {AuthenticationService} from "../services/authentication-service/authentication.service";
import {Title} from "@angular/platform-browser";
import {User} from "../../common/models/user";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {JwtHelperService} from "@auth0/angular-jwt";
import {environment} from "../../../environments/environment";


@Component({
    selector: "register-page",
    templateUrl: 'register.component.html',
    styleUrls: ["register.component.css"]
})
export class RegisterComponent implements OnInit {

    error: boolean = false;
    errorMessage: string = '';

    constructor(
        private titleService: Title,
        private formBuilder: FormBuilder,
        private router: Router,
        private http: HttpClient,
        private jwtHelperService: JwtHelperService,
        private authenticationService: AuthenticationService,
    ) {
        if (this.authenticationService.currentUserValue) {
            this.router.navigate(['/']);
        }
    }

    ngOnInit() {
        this.titleService.setTitle("Sign up");
    }

    register(form: NgForm) {
        let credentials = JSON.stringify(form.value);
        let user = new User(form.value["username"], form.value["password"], form.value["firstName"], form.value["lastName"]);
        this.http.post(environment.apiUrl + "/api/auth/register", credentials, {
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        }).subscribe(response => {
            let user = <User>response;
            localStorage.setItem("currentUser", JSON.stringify(user));
            this.authenticationService.currentUserSubject.next(user);
            this.router.navigate(["/"]);
        }, err => {
        });
    }
}