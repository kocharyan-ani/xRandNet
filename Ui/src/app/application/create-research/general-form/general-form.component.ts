import {Component, Input, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {ResearchService} from "../../common/research/services/research.service";
import {ModelTypeEnum} from "../../common/research/enums/model-type-enum";
import {ResearchEnum} from "../../common/research/enums/research-enum";
import {GenerationTypeEnum} from "../../common/research/enums/generation-type-enum";
import {StorageTypeEnum} from "../../common/research/enums/storage-type-enum";

@Component({
    selector: 'app-general-form',
    templateUrl: './general-form.component.html',
    styleUrls: ['./general-form.component.css']
})
export class GeneralFormComponent implements OnInit {

    @Input() formGroup: FormGroup;
    @Input() modelTypes: ModelTypeEnum[];
    @Input() generationTypes: GenerationTypeEnum[];
    @Input() storageTypes: StorageTypeEnum[];

    public researchNames: ResearchEnum[];

    constructor(
        private researchService: ResearchService
    ) {
    }

    ngOnInit() {
        this.researchNames = this.researchService.getResearchNames();
    }
}
