import {Injectable} from '@angular/core';
import {ResearchEnum} from "../enums/research-enum";
import {ModelTypeEnum} from "../enums/model-type-enum";

@Injectable({
    providedIn: 'root'
})
export class ModelTypeService {

    constructor() {
    }

    public get(research: ResearchEnum): ModelTypeEnum[] {
        switch (research) {
            case ResearchEnum.BASIC:
                return [
                    ModelTypeEnum.BA,
                    ModelTypeEnum.BB,
                    ModelTypeEnum.ConnectedRegularHierarchic,
                    ModelTypeEnum.ConnectedNonRegularHierarchic,
                    ModelTypeEnum.ER,
                    ModelTypeEnum.HMN,
                    ModelTypeEnum.NonRegularHierarchic,
                    ModelTypeEnum.RegularHierarchic,
                    ModelTypeEnum.WS
                ];
            case ResearchEnum.EVOLUTION:
                return [
                    ModelTypeEnum.ER
                ];
            case ResearchEnum.THRESHOLD:
                return [
                    ModelTypeEnum.ER,
                    ModelTypeEnum.RegularHierarchic
                ];
            case ResearchEnum.COLLECTION:
                return [
                    ModelTypeEnum.BA,
                    ModelTypeEnum.ER,
                    ModelTypeEnum.HMN,
                    ModelTypeEnum.NonRegularHierarchic,
                    ModelTypeEnum.RegularHierarchic,
                    ModelTypeEnum.WS
                ];
            case ResearchEnum.STRUCTURAL:
                return [
                    ModelTypeEnum.ER
                ];
            case ResearchEnum.ACTIVATION:
                return [
                    ModelTypeEnum.BA,
                    ModelTypeEnum.ER,
                    ModelTypeEnum.HMN,
                    ModelTypeEnum.WS
                ];
        }
    }
}
