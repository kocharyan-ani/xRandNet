export class Person {

    private _id: number;
    private _firstName: string;
    private _lastName: string;
    private _fbLink: string;
    private _imageLink: string;
    private _emailAddress: string;
    private _linkedInLink: string;
    private _info: string;

    get id(): number {
        return this._id;
    }

    set id(value: number) {
        this._id = value;
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

    get info(): string {
        return this._info;
    }

    set info(value: string) {
        this._info = value;
    }

    get fbLink(): string {
        return this._fbLink;
    }

    set fbLink(value: string) {
        this._fbLink = value;
    }

    get imageLink(): string {
        return this._imageLink;
    }

    set imageLink(value: string) {
        this._imageLink = value;
    }

    get emailAddress(): string {
        return this._emailAddress;
    }

    set emailAddress(value: string) {
        this._emailAddress = value;
    }

    get linkedInLink(): string {
        return this._linkedInLink;
    }

    set linkedInLink(value: string) {
        this._linkedInLink = value;
    }

    toJson() {
        return {
            id: this._id,
            firstName: this._firstName,
            lastName: this._lastName,
            info: this._info,
            fbLink: this._fbLink,
            imageLink: this._imageLink,
            linkedInLink: this._linkedInLink,
            emailAddress: this._emailAddress,
        }
    }
}
