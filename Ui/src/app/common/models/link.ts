export class Link {

    private _id: number;
    private _name: string;
    private _url: string;
    private _type: LinkType;

    constructor(id = 0, name = '', url = '', type = LinkType.GENERAL) {
        this._id = id;
        this._url = url;
        this._name = name;
        this._type = type
    }

    get name(): string {
        return this._name;
    }

    set name(name: string) {
        this._name = name;
    }

    get id(): number {
        return this._id;
    }

    set id(id: number) {
        this._id = id;
    }

    get url(): string {
        return this._url;
    }

    set url(value: string) {
        this._url = value;
    }

    get type(): LinkType {
        return this._type;
    }

    set type(value: LinkType) {
        this._type = value;
    }

    toJson() {
        return {
            id: this._id,
            name: this._name,
            url: this._url,
            type: this._type,
        }
    }
}

export enum LinkType {
    GENERAL = 0,
    LITERATURE = 1
}