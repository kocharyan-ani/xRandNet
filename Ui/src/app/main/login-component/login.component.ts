import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, NgForm} from '@angular/forms';
import {AuthenticationService} from "../services/authentication-service/authentication.service";
import {Title} from "@angular/platform-browser";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {User} from "../../common/models/user";
import {environment} from "../../../environments/environment";


@Component({
    selector: "login-page",
    templateUrl: 'login.component.html',
    styleUrls: ["login.component.css"]
})
export class LoginComponent implements OnInit {
    returnUrl: string;
    invalidLogin: boolean = false;

    constructor(
        private titleService: Title,
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private http: HttpClient,
    ) {
        // redirect to home if already logged in
        if (this.authenticationService.currentUserValue) {
            this.router.navigate(['/']);
        }
    }

    ngOnInit() {
        this.titleService.setTitle("Sign in");
        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }


    login(form: NgForm) {
        let credentials = JSON.stringify(form.value);
        this.http.post(environment.apiUrl + "/api/auth/login", credentials, {
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        }).subscribe(response => {
            let user = <User>response;
            localStorage.setItem("currentUser", JSON.stringify(user));
            this.authenticationService.currentUserSubject.next(user);
            this.invalidLogin = false;
            this.router.navigateByUrl(this.returnUrl);
        }, err => {
            this.invalidLogin = true;
        });
    }
}