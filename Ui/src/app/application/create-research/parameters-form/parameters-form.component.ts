import {Component, Input, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {Parameter} from "../../common/parameter/interfaces/parameter";
import {ParameterTypeEnum} from "../../common/research/enums/parameter-type-enum";

@Component({
    selector: 'app-parameters-form',
    templateUrl: './parameters-form.component.html',
    styleUrls: ['./parameters-form.component.css']
})
export class ParametersFormComponent implements OnInit {

    @Input() formGroup: FormGroup;
    @Input() parameters: Parameter[];

    public parameterTypeEnum = ParameterTypeEnum;

    constructor() {
    }

    ngOnInit() {
    }

}
