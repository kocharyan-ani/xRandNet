import { Injectable } from '@angular/core';
import {ResearchEnum} from "../enums/research-enum";
import {GenerationTypeEnum} from "../enums/generation-type-enum";

@Injectable({
  providedIn: 'root'
})
export class GenerationTypeService {

  constructor() { }

  public get(research: ResearchEnum): GenerationTypeEnum[] {
    switch (research) {
      case ResearchEnum.BASIC:
        return [
          GenerationTypeEnum.Random,
          GenerationTypeEnum.Static
        ];
      case ResearchEnum.EVOLUTION:
        return [
          GenerationTypeEnum.Random,
          GenerationTypeEnum.Static
        ];
      case ResearchEnum.THRESHOLD:
        return [
          GenerationTypeEnum.Random,
          GenerationTypeEnum.Static
        ];
      case ResearchEnum.COLLECTION:
        return [
          GenerationTypeEnum.Static
        ];
      case ResearchEnum.STRUCTURAL:
        return [
          GenerationTypeEnum.Static
        ];
      case ResearchEnum.ACTIVATION:
        return [
          GenerationTypeEnum.Random,
          GenerationTypeEnum.Static
        ];
    }
  }
}
