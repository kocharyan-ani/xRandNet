import {BugStatus} from "./bug-status";

export class Bug {

    private _id: number;
    private _summary: string;
    private _description: string;
    private _reporter: string;
    private _reportDate: string;
    private _version: string;
    private _status: BugStatus;

    constructor(id, summary, description, reporter, version, status,reportDate) {
        this._description = description;
        this._summary = summary;
        this._id = id;
        this._reporter = reporter;
        this._version = version;
        this._status = status;
        this._reportDate = reportDate;
    }

    get reportDate(): string {
        return this._reportDate;
    }

    set reportDate(value: string) {
        this._reportDate = value;
    }

    get status(): BugStatus {
        return this._status;
    }

    set status(value: BugStatus) {
        this._status = value;
    }

    get version(): string {
        return this._version;
    }

    set version(value: string) {
        this._version = value;
    }

    get id(): number {
        return this._id;
    }

    set id(value: number) {
        this._id = value;
    }

    get description(): string {
        return this._description;
    }

    set description(value: string) {
        this._description = value;
    }

    get summary(): string {
        return this._summary;
    }

    set summary(value: string) {
        this._summary = value;
    }

    get reporter(): string {
        return this._reporter;
    }

    set reporter(value: string) {
        this._reporter = value;
    }

    toJson() {
        return {
            id: this._id,
            summary: this._summary,
            description: this._description,
            reporter: this._reporter,
            status: this._status.valueOf(),
            version: this._version
        }
    }
}
