export class AboutInfo {

    private _id: number;
    private _content: String;
    
    constructor(id?: number, content?: String) {
        this._id = id;
        this._content = content;
    }

    get content(): String {
        return this._content;
    }

    set content(value: String) {
        this._content = value;
    }

    get id(): number {
        return this._id;
    }

    set id(value: number) {
        this._id = value;
    }

    toJson() {
        return {
            id: this._id,
            content: this._content,
        }
    }

}
