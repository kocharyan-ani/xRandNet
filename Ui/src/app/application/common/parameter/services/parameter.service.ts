import {Injectable} from '@angular/core';
import {ResearchEnum} from "../../research/enums/research-enum";
import {ParameterEnum} from "../enums/parameter-enum";
import {ParameterTypeEnum} from "../../research/enums/parameter-type-enum";
import {Parameter} from "../interfaces/parameter";
import {ModelTypeEnum} from "../../research/enums/model-type-enum";

@Injectable({
    providedIn: 'root'
})
export class ParameterService {

    private vertices = {name: ParameterEnum.Vertices, type: ParameterTypeEnum.INPUT, default: 24};
    private probability = {name: ParameterEnum.Probability, type: ParameterTypeEnum.INPUT, default: 0.1};
    private edges = {name: ParameterEnum.Edges, type: ParameterTypeEnum.INPUT, default: 1};
    private stepCount = {name: ParameterEnum.StepCount, type: ParameterTypeEnum.INPUT, default: 1};
    private evolutionStepCount = {name: ParameterEnum.EvolutionStepCount, type: ParameterTypeEnum.INPUT, default: 1};
    private omega = {name: ParameterEnum.Omega, type: ParameterTypeEnum.INPUT, default: 0.1};
    private permanentDistribution = {
        name: ParameterEnum.PermanentDistribution,
        type: ParameterTypeEnum.CHECKBOX,
        default: true
    };
    private probabilityMax = {name: ParameterEnum.ProbabilityMax, type: ParameterTypeEnum.INPUT, default: 1};
    private probabilityDelta = {name: ParameterEnum.ProbabilityDelta, type: ParameterTypeEnum.INPUT, default: 0.01};
    private tracingStepIncrement = {
        name: ParameterEnum.TracingStepIncrement,
        type: ParameterTypeEnum.INPUT,
        default: 0
    };
    private deactivationSpeed = {name: ParameterEnum.DeactivationSpeed, type: ParameterTypeEnum.INPUT, default: 0.1};
    private activationSpeed = {name: ParameterEnum.ActivationSpeed, type: ParameterTypeEnum.INPUT, default: 0.1};
    private activationStepCount = {name: ParameterEnum.ActivationStepCount, type: ParameterTypeEnum.INPUT, default: 10};
    private initialActivationProbability = {
        name: ParameterEnum.InitialActivationProbability,
        type: ParameterTypeEnum.INPUT,
        default: 1
    };
    private fitnessDensityFunction = {
        name: ParameterEnum.FitnessDensityFunction,
        type: ParameterTypeEnum.INPUT,
        default: 1
    };
    private branchingIndex = {name: ParameterEnum.BranchingIndex, type: ParameterTypeEnum.INPUT, default: 2};
    private mu = {name: ParameterEnum.Mu, type: ParameterTypeEnum.INPUT, default: 0.1};
    private level = {name: ParameterEnum.Level, type: ParameterTypeEnum.INPUT, default: 1};
    private zeroLevelNodesCount = {name: ParameterEnum.ZeroLevelNodesCount, type: ParameterTypeEnum.INPUT, default: 3};
    private alpha = {name: ParameterEnum.Alpha, type: ParameterTypeEnum.INPUT, default: 0.1};
    private blocksCount = {name: ParameterEnum.BlocksCount, type: ParameterTypeEnum.INPUT, default: 2};
    private makeConnected = {name: ParameterEnum.MakeConnected, type: ParameterTypeEnum.CHECKBOX, default: true};

    constructor() {
    }

    public get(researchType: ResearchEnum, researchMode: ModelTypeEnum): Parameter[] {
        switch (researchType) {
            case ResearchEnum.BASIC:
                return this.getBasicResearchOptions(researchMode);
            case ResearchEnum.EVOLUTION:
                return this.getEvolutionResearchOptions(researchMode);
            case ResearchEnum.THRESHOLD:
                return this.getThresholdResearchOptions(researchMode);
            case ResearchEnum.COLLECTION:
                return this.getCollectionResearchOptions(researchMode);
            case ResearchEnum.STRUCTURAL:
                return this.getStructuralResearchOptions(researchMode);
            case ResearchEnum.ACTIVATION:
                return this.getActivationResearchOptions(researchMode)
        }
    }

    private getBasicResearchOptions(researchMode: ModelTypeEnum): Parameter[] {
        switch (researchMode) {
            case ModelTypeEnum.BA:
                return [this.vertices, this.probability, this.edges, this.stepCount];
            case ModelTypeEnum.BB:
                return [this.vertices, this.probability, this.edges, this.fitnessDensityFunction];
            case ModelTypeEnum.ConnectedNonRegularHierarchic:
                return [this.vertices, this.branchingIndex, this.mu];
            case ModelTypeEnum.ConnectedRegularHierarchic:
                return [this.branchingIndex, this.level, this.mu];
            case ModelTypeEnum.ER:
                return [this.vertices, this.probability];
            case ModelTypeEnum.HMN:
                return [this.vertices, this.probability, this.zeroLevelNodesCount, this.alpha, this.blocksCount, this.makeConnected]
            case ModelTypeEnum.NonRegularHierarchic:
                return [this.vertices, this.branchingIndex, this.mu];
            case ModelTypeEnum.RegularHierarchic:
                return [this.branchingIndex, this.level, this.mu];
            case ModelTypeEnum.WS:
                return [this.vertices, this.probability, this.edges, this.stepCount]
        }
    }

    private getEvolutionResearchOptions(researchMode: ModelTypeEnum): Parameter[] {
        switch (researchMode) {
            case ModelTypeEnum.ER:
                return [this.evolutionStepCount, this.omega, this.tracingStepIncrement, this.permanentDistribution, this.vertices, this.probability]
        }
    }

    private getThresholdResearchOptions(researchMode: ModelTypeEnum): Parameter[] {
        switch (researchMode) {
            case ModelTypeEnum.ER:
                return [this.probabilityMax, this.probabilityDelta, this.vertices, this.probability];
            case ModelTypeEnum.RegularHierarchic:
                return [this.probabilityMax, this.probabilityDelta, this.branchingIndex, this.level, this.mu]
        }
    }

    private getCollectionResearchOptions(researchMode: ModelTypeEnum): Parameter[] {
        switch (researchMode) {
            case ModelTypeEnum.BA:
            case ModelTypeEnum.ER:
            case ModelTypeEnum.HMN:
            case ModelTypeEnum.NonRegularHierarchic:
            case ModelTypeEnum.RegularHierarchic:
            case ModelTypeEnum.WS:
                return [this.tracingStepIncrement, this.deactivationSpeed, this.activationSpeed, this.activationStepCount, this.initialActivationProbability];
        }
    }

    private getStructuralResearchOptions(researchMode: ModelTypeEnum): Parameter[] {
        switch (researchMode) {
            case ModelTypeEnum.ER:
                return [];
        }
    }

    private getActivationResearchOptions(researchMode: ModelTypeEnum): Parameter[] {
        switch (researchMode) {
            case ModelTypeEnum.BA:
                return [this.tracingStepIncrement, this.deactivationSpeed, this.activationSpeed, this.activationStepCount, this.initialActivationProbability, this.vertices, this.probability, this.edges, this.stepCount];
            case ModelTypeEnum.ER:
                return [this.tracingStepIncrement, this.deactivationSpeed, this.activationSpeed, this.activationStepCount, this.initialActivationProbability, this.vertices, this.probability];
            case ModelTypeEnum.HMN:
                return [this.tracingStepIncrement, this.deactivationSpeed, this.activationSpeed, this.activationStepCount, this.initialActivationProbability, this.vertices, this.probability, this.zeroLevelNodesCount, this.alpha, this.blocksCount, this.makeConnected];
            case ModelTypeEnum.WS:
                return [this.tracingStepIncrement, this.deactivationSpeed, this.activationSpeed, this.activationStepCount, this.initialActivationProbability, this.vertices, this.probability, this.edges, this.stepCount]
        }
    }
}
