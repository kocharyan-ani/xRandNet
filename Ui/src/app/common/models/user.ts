export class User {
    private _id: number;
    private _username: string;
    private _password: string;
    private _firstName: string;
    private _lastName: string;
    private _token: string;

    constructor(username: string, password: string, firstName: string, lastName: string) {
        this._username = username;
        this._password = password;
        this._firstName = firstName;
        this._lastName = lastName;
    }

    get id(): number {
        return this._id;
    }

    set id(value: number) {
        this._id = value;
    }

    get username(): string {
        return this._username;
    }

    set username(value: string) {
        this._username = value;
    }

    get password(): string {
        return this._password;
    }

    set password(value: string) {
        this._password = value;
    }

    get firstName(): string {
        return this._firstName;
    }

    set firstName(value: string) {
        this._firstName = value;
    }

    get lastName(): string {
        return this._lastName;
    }

    set lastName(value: string) {
        this._lastName = value;
    }

    get token(): string {
        return this._token;
    }

    set token(value: string) {
        this._token = value;
    }

}