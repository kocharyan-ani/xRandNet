export class Person {

    private _id: number;
    private _firstName: string;
    private _lastName: string;
    private _facebookUrl: string;
    private _imageUrl: string;
    private _email: string;
    private _linkedInUrl: string;
    private _description: string;
    private _editable: boolean;

    constructor(id?: number, firstName?: string, lastName?: string, facebookUrl?: string, imageUrl?: string, email?: string, linkedInUrl?: string, description?: string) {
        this._id = id;
        this._firstName = firstName;
        this._lastName = lastName;
        this._facebookUrl = facebookUrl;
        this._imageUrl = imageUrl;
        this._email = email;
        this._linkedInUrl = linkedInUrl;
        this._description = description;
    }

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

    get description(): string {
        return this._description;
    }

    set description(value: string) {
        this._description = value;
    }

    set info(value: string) {
        this._description = value;
    }

    get facebookUrl(): string {
        return this._facebookUrl;
    }

    set facebookUrl(value: string) {
        this._facebookUrl = value;
    }

    get imageUrl(): string {
        return this._imageUrl;
    }

    set imageUrl(value: string) {
        this._imageUrl = value;
    }

    get email(): string {
        return this._email;
    }

    set email(value: string) {
        this._email = value;
    }

    get linkedInUrl(): string {
        return this._linkedInUrl;
    }

    set linkedInUrl(value: string) {
        this._linkedInUrl = value;
    }

    get editable(): boolean {
        return this._editable;
    }

    set editable(value: boolean) {
        this._editable = value;
    }

    toJson() {
        return {
            id: this._id,
            firstName: this._firstName,
            lastName: this._lastName,
            description: this._description,
            facebookUrl: this._facebookUrl,
            imageURL: this._imageUrl,
            linkedInURL: this._linkedInUrl,
            email: this._email,
        }
    }

}
