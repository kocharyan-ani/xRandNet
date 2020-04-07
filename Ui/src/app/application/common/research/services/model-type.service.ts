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
                    ModelTypeEnum.ER,
                    ModelTypeEnum.BA,
                    ModelTypeEnum.BB,
                    ModelTypeEnum.WS,
                    ModelTypeEnum.RegularHierarchic,
                    ModelTypeEnum.NonRegularHierarchic,
                    ModelTypeEnum.HMN,
                    ModelTypeEnum.ConnectedRegularHierarchic,
                    ModelTypeEnum.ConnectedNonRegularHierarchic
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
                    ModelTypeEnum.ER,
                    ModelTypeEnum.BA,
                    ModelTypeEnum.WS,
                    ModelTypeEnum.RegularHierarchic,
                    ModelTypeEnum.NonRegularHierarchic,
                    ModelTypeEnum.HMN
                ];
            case ResearchEnum.STRUCTURAL:
                return [
                    ModelTypeEnum.ER
                ];
            case ResearchEnum.ACTIVATION:
                return [
                    ModelTypeEnum.BB,
                    ModelTypeEnum.ER,
                    ModelTypeEnum.HMN,
                    ModelTypeEnum.WS
                ];
        }
    }
}
