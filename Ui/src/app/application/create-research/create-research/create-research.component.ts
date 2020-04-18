import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";
import {AnalyzeOptionsService} from "../../common/analyze-options/services/analyze-options.service";
import {BaseResearch} from "../../common/research/models/base-research";
import {ResearchService} from "../../common/research/services/research.service";
import {Location} from '@angular/common';
import {ResearchEnum} from "../../common/research/enums/research-enum";
import {Parameter} from "../../common/parameter/interfaces/parameter";
import {ParameterService} from "../../common/parameter/services/parameter.service";
import {AnalyzeOptionEnum} from "../../common/analyze-options/enums/analyze-option-enum";
import {Subscription} from "rxjs";
import {ModelTypeService} from "../../common/research/services/model-type.service";
import {GenerationTypeService} from "../../common/research/services/generation-type.service";
import {ModelTypeEnum} from "../../common/research/enums/model-type-enum";
import {GenerationTypeEnum} from "../../common/research/enums/generation-type-enum";
import {StorageTypeEnum} from "../../common/research/enums/storage-type-enum";

@Component({
    selector: 'app-create-research',
    templateUrl: './create-research.component.html',
    styleUrls: ['./create-research.component.css']
})
export class CreateResearchComponent implements OnInit, OnDestroy {

    public formGroup: FormGroup;
    private generalForm: FormGroup;
    public modelTypes: ModelTypeEnum[];
    public generationTypes: GenerationTypeEnum[];
    public storageTypes = [ /*StorageTypeEnum.EXCEL,*/ StorageTypeEnum.TEXT, StorageTypeEnum.XML];
    public analyzeOptions: AnalyzeOptionEnum[];
    public parameters: Parameter[];
    private researchTypeSubscription: Subscription;
    private modelTypeSubscription: Subscription;

    constructor(
        private location: Location,
        private researchService: ResearchService,
        private parameterService: ParameterService,
        private modelTypeService: ModelTypeService,
        private generationTypeService: GenerationTypeService,
        private analyzeOptionsService: AnalyzeOptionsService
    ) {
    }

    ngOnInit() {
        this.setGeneralForm();
        this.setTypeSubscription();
        this.setModelTypeSubscription()
    }

    ngOnDestroy() {
        this.researchTypeSubscription.unsubscribe();
        this.modelTypeSubscription.unsubscribe()
    }

    private setGeneralForm() {
        this.generalForm = new FormGroup({
            research: new FormControl(),
            name: new FormControl(),
            model: new FormControl(),
            storage: new FormControl(),
            generation: new FormControl(),
            count: new FormControl(),
            tracing: new FormControl(),
            connected: new FormControl(),
        });
    }

    private generateForm() {
        const analyzeOptions = {};
        this.analyzeOptions.forEach(option => {
            analyzeOptions[option] = new FormControl(false)
        });
        const analyzeOptionsForm = new FormGroup(analyzeOptions);

        const parameters = {};
        this.parameters.forEach(parameter => {
            parameters[parameter.name] = new FormControl(parameter.default)
        });
        const parametersForm = new FormGroup(parameters);

        this.formGroup = new FormGroup({
            general: this.generalForm,
            analyzeOptions: analyzeOptionsForm,
            parameters: parametersForm
        })
    }

    private setTypeSubscription() {
        const initialResearch = ResearchEnum.BASIC;
        this.modelTypes = this.modelTypeService.get(initialResearch);
        this.setResearchType(initialResearch, this.modelTypes[0]);
        this.researchTypeSubscription = this.generalForm.get('research').valueChanges.subscribe(value => {
            this.modelTypes = this.modelTypeService.get(value);
            this.setResearchType(value, this.modelTypes[0]);
        })
    }

    private setModelTypeSubscription() {
        this.modelTypeSubscription = this.generalForm.get('model').valueChanges.subscribe(model => {
            const value = this.generalForm.value;
            this.setResearchType(value.research, model);
        })
    }

    private setResearchType(research: ResearchEnum, model: ModelTypeEnum) {
        this.generationTypes = this.generationTypeService.get(research);
        this.analyzeOptions = this.analyzeOptionsService.get(research);
        this.parameters = this.parameterService.get(research, model);

        this.setInitialFormValue(research, model);
        this.generateForm();
    }

    private setInitialFormValue(research: ResearchEnum, model: ModelTypeEnum) {
        this.generalForm.reset({
            research: research,
            name: `${research} Research`,
            model: model,
            storage: this.storageTypes[0],
            generation: this.generationTypes[0],
            count: 1,
            tracing: false,
            connected: false,
        }, {emitEvent: false})
    }

    public submit(value: any) {
        const research = new BaseResearch();
        research.addFormData(value);
        this.researchService.addResearch(research);

        this.location.back()
    }
}
