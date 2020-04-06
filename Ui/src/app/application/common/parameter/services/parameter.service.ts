import {Injectable} from '@angular/core';
import {ResearchEnum} from "../../research/enums/research-enum";
import {ParameterEnum} from "../enums/parameter-enum";
import {ParameterTypeEnum} from "../../research/enums/parameter-type-enum";
import {Parameter} from "../interfaces/parameter";

@Injectable({
    providedIn: 'root'
})
export class ParameterService {

    constructor() {
    }

    public get(research: ResearchEnum): Parameter[] {
        switch (research) {
            case ResearchEnum.BASIC:
                return [
                    {
                        name: ParameterEnum.Vertices,
                        type: ParameterTypeEnum.INPUT,
                        default: 24
                    },
                    {
                        name: ParameterEnum.Probability,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    },
                    {
                        name: ParameterEnum.Edges,
                        type: ParameterTypeEnum.INPUT,
                        default: 1
                    },
                    {
                        name: ParameterEnum.StepCount,
                        type: ParameterTypeEnum.INPUT,
                        default: 1
                    }
                ];
            case ResearchEnum.EVOLUTION:
                return [
                    {
                        name: ParameterEnum.EvolutionStepCount,
                        type: ParameterTypeEnum.INPUT,
                        default: 1
                    },
                    {
                        name: ParameterEnum.Omega,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    },
                    {
                        name: ParameterEnum.TracingStepIncrement,
                        type: ParameterTypeEnum.INPUT,
                        default: 0
                    },
                    {
                        name: ParameterEnum.PermanentDistribution,
                        type: ParameterTypeEnum.CHECKBOX,
                        default: true
                    },
                    {
                        name: ParameterEnum.Vertices,
                        type: ParameterTypeEnum.INPUT,
                        default: 24
                    },
                    {
                        name: ParameterEnum.Probability,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    }
                ];
            case ResearchEnum.THRESHOLD:
                return [
                    {
                        name: ParameterEnum.ProbabilityMax,
                        type: ParameterTypeEnum.INPUT,
                        default: 1
                    },
                    {
                        name: ParameterEnum.ProbabilityDelta,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.01
                    },
                    {
                        name: ParameterEnum.Vertices,
                        type: ParameterTypeEnum.INPUT,
                        default: 24
                    },
                    {
                        name: ParameterEnum.Probability,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    }
                ];
            case ResearchEnum.COLLECTION:
                return [
                    {
                        name: ParameterEnum.TracingStepIncrement,
                        type: ParameterTypeEnum.INPUT,
                        default: 0
                    },
                    {
                        name: ParameterEnum.DeactivationSpeed,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    },
                    {
                        name: ParameterEnum.ActivationSpeed,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    },
                    {
                        name: ParameterEnum.ActivationStepCount,
                        type: ParameterTypeEnum.INPUT,
                        default: 10
                    },
                    {
                        name: ParameterEnum.InitialActivationProbability,
                        type: ParameterTypeEnum.INPUT,
                        default: 1
                    }
                ];
            case ResearchEnum.STRUCTURAL:
                return [];
            case ResearchEnum.ACTIVATION:
                return [
                    {
                        name: ParameterEnum.TracingStepIncrement,
                        type: ParameterTypeEnum.INPUT,
                        default: 0
                    },
                    {
                        name: ParameterEnum.DeactivationSpeed,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    },
                    {
                        name: ParameterEnum.ActivationSpeed,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    },
                    {
                        name: ParameterEnum.ActivationStepCount,
                        type: ParameterTypeEnum.INPUT,
                        default: 10
                    },
                    {
                        name: ParameterEnum.InitialActivationProbability,
                        type: ParameterTypeEnum.INPUT,
                        default: 1
                    },
                    {
                        name: ParameterEnum.Vertices,
                        type: ParameterTypeEnum.INPUT,
                        default: 24
                    },
                    {
                        name: ParameterEnum.Probability,
                        type: ParameterTypeEnum.INPUT,
                        default: 0.1
                    },
                    {
                        name: ParameterEnum.Edges,
                        type: ParameterTypeEnum.INPUT,
                        default: 1
                    },
                    {
                        name: ParameterEnum.StepCount,
                        type: ParameterTypeEnum.INPUT,
                        default: 1
                    }
                ]
        }
    }
}
