export class News {
    get id(): number {
        return this._id;
    }

    set id(value: number) {
        this._id = value;
    }

    private _id: number;
    private _datePosted: Date;
    private _content: string;
    private _title: string;
    private _editable: boolean;

    constructor(title?, datePosted?, content?,id?) {
        this._title = title;
        this._content = content;
        this._datePosted = datePosted;
        this._editable = false;
        this._id = id;
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

    get editable(): boolean {
        return this._editable;
    }

    set editable(value: boolean) {
        this._editable = value;
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

    toJson() {
        return {
            "content": this.content,
            "datePosted": this.datePosted,
            "id": this.id,
            "title": this.title
        }
    }
}
