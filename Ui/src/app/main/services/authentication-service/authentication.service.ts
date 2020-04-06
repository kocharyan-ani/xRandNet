import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {BehaviorSubject, Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import {User} from "../../../common/models/user";
import {environment} from "../../../../environments/environment";

@Injectable({providedIn: 'root'})
export class AuthenticationService {
    public currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    isAdmin() {
        return this.http.get<any>(`${environment.apiUrl}/api/auth/isAdmin`)
            .pipe(map(isAdmin => {
                return isAdmin
            }));
    }

    login(username: string, password: string) {
        return this.http.post<any>(`${environment.apiUrl}/api/auth/login`, {username, password})
            .pipe(map(user => {
                // login successful if there's a jwt token in the response
                if (user && user.token) {
                    user.roles = JSON.parse(user.roles);
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                    this.currentUserSubject.next(user);
                }
                return user;
            }));
    }

    register(user: User) {
        let headers = new HttpHeaders();
        headers.append('Content-Type', 'application/json');
        return this.http.post<any>(environment.apiUrl + '/api/auth/register',
            {
                firstName: user.firstName,
                lastName: user.lastName,
                username: user.username,
                password: user.password
            })
            .pipe(map(user => {
                if (user && user.token) {
                    console.log(user);
                }
                return user;
            }));
    }


    logout() {
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}