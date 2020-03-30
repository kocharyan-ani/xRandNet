import {Bug} from "./bug";

export class Software {
    get label(): string {
        return this._label;
    }

    set label(value: string) {
        this._label = value;
    }
    private _version: string;
    private _label: string;
    private _releaseNotes: string;
    private _releaseDate: string;
    private _bugs: Array<Bug>;

    constructor(version ?: string, releaseNotes ?: string, releaseDate ?: string, bugs ?: Array<Bug>) {
        this._version = version;
        this._label = version
        this._releaseNotes = releaseNotes;
        this._releaseDate = releaseDate;
        this._bugs = bugs;
    }


    get version(): string {
        return this._version;
    }

    set version(value: string) {
        this._version = value;
    }

    get releaseDate(): string {
        return this._releaseDate;
    }

    set releaseDate(value: string) {
        this._releaseDate = value;
    }

    get releaseNotes(): string {
        return this._releaseNotes;
    }

    set releaseNotes(value: string) {
        this._releaseNotes = value;
    }

    get bugs(): Array<Bug> {
        return this._bugs;
    }

    set bugs(value: Array<Bug>) {
        this._bugs = value;
    }

    public toString = (): string => {
        return JSON.stringify({"version": this.version, "releaseNotes": this.releaseNotes})
    }
}
