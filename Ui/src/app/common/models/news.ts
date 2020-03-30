export class News {
    private _datePosted: Date;
    private _content: string;
    private _title: string;

    constructor(title, datePosted, content) {
        this._title = title;
        this._content = content;
        this._datePosted = datePosted;
    }

    get datePosted(): Date {
        return this._datePosted;
    }

    get title(): string {
        return this._title;
    }

    set title(value: string) {
        this._title = value;
    }

    set datePosted(value: Date) {
        this._datePosted = value;
    }

    get content(): string {
        return this._content;
    }

    set content(value: string) {
        this._content = value;
    }
}
