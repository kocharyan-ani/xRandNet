import {ParameterTypeEnum} from "../../research/enums/parameter-type-enum";

export interface Parameter {
     name: string;
     type: ParameterTypeEnum;
     default: any;
}
