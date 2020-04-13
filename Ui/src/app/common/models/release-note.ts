export class ReleaseNote {
  private _version: string;
  private _description: string;

  constructor(version, description) {
    this._description = description;
    this._version = version;
  }

  get version(): string {
    return this._version;
  }

  set version(value: string) {
    this._version = value;
  }

  get description(): string {
    return this._description;
  }

  set description(value: string) {
    this._description = value;
  }
}
