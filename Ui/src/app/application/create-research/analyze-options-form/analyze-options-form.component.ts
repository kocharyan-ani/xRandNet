import {Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {AnalyzeOptionEnum} from "../../common/analyze-options/enums/analyze-option-enum";

@Component({
    selector: 'app-analyze-options-form',
    templateUrl: './analyze-options-form.component.html',
    styleUrls: ['./analyze-options-form.component.css']
})
export class AnalyzeOptionsFormComponent {

    @Input() formGroup: FormGroup;
    @Input() analyzeOptions: AnalyzeOptionEnum[];

    public selectAll() {
        this.analyzeOptions.forEach(name => this.formGroup.get(name).patchValue(true))
    }

    public deselectAll() {
        this.analyzeOptions.forEach(name => this.formGroup.get(name).patchValue(false))
    }
}
