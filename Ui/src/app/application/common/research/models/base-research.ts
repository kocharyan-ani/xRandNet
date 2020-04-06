import {ResearchEnum} from "../enums/research-enum";
import {AnalyzeOption} from "../../analyze-options/interfaces/analyze-option";
import {ModelTypeEnum} from "../enums/model-type-enum";
import {GenerationTypeEnum} from "../enums/generation-type-enum";
import {StorageTypeEnum} from "../enums/storage-type-enum";
import {ParameterOption} from "../../analyze-options/interfaces/parameter-option";

export class BaseResearch {
    public research: string = ResearchEnum.BASIC;
    public name: string = 'Research';
    public model: ModelTypeEnum ;
    public storage: StorageTypeEnum;
    public generation: GenerationTypeEnum;
    public count: number = 1;
    public tracing: boolean = false;
    public connected: boolean = false;
    public status: string = '-';

    public analyzeOptions: AnalyzeOption[] = [];
    public parameters: ParameterOption[] = [];

    public addFormData(data: any) {
        let generalData = data.general;
        this.research = generalData.research;
        this.name = generalData.name;
        this.model = generalData.model;
        this.storage = generalData.storage;
        this.generation = generalData.generation;
        this.count = generalData.count;
        this.tracing = generalData.tracing;
        this.connected = generalData.connected;

        let analyzeOptions = data.analyzeOptions;
        this.analyzeOptions = Object.keys(analyzeOptions).map(key => <AnalyzeOption>{
            key: key,
            value: analyzeOptions[key]
        });

        let parameters = data.parameters;
        this.parameters = Object.keys(parameters).map(key => <ParameterOption>{
            key: key,
            value: String(parameters[key])
        })
    }
}
