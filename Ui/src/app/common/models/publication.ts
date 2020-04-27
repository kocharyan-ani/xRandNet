export class Publication {

    private _id: number;
    private _title: string;
    private _authors: string;
    private _journal: string;
    private _file;
    private _editable: boolean;


    constructor(id?: number, title?: string, authors?: string, journal?: string) {
        this._id = id;
        this._title = title;
        this._authors = authors;
        this._journal = journal;
    }

    get editable(): boolean {
        return this._editable;
    }

    set editable(value: boolean) {
        this._editable = value;
    }
    get id(): number {
        return this._id;
    }

    set id(value: number) {
        this._id = value;
    }

    get title(): string {
        return this._title;
    }

    set title(value: string) {
        this._title = value;
    }

    get authors(): string {
        return this._authors;
    }

    set authors(value: string) {
        this._authors = value;
    }

    get journal(): string {
        return this._journal;
    }

    set journal(value: string) {
        this._journal = value;
    }

    get file() {
        return this._file;
    }

    set file(value) {
        this._file = value;
    }

    toJson(){
        return {
            id: this._id,
            title: this._title,
            authors: this._authors,
            journal: this._journal,
        }
    }
}