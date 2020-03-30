export class BaseModel {

    constructor(data: any) {
    }

    public static transformCollection(data: Array<any>): Array<any> {
        return data.map(item => this.transform(item));
    }

    public static transform(data: any): any {
        return data ? new this(data) : null;
    }
}
