import {NgModule} from '@angular/core';
import {ApplicationRoutingModule} from './application-routing.module';
import {ApplicationComponent} from './application/application.component';
import {CommonModule} from '@angular/common';
import {CreateResearchComponent} from './create-research/create-research/create-research.component';
import {GeneralFormComponent} from "./create-research/general-form/general-form.component";
import {ReactiveFormsModule} from "@angular/forms";
import { ResearchListingComponent } from './research-landing/research-listing/research-listing.component';
import { ParametersFormComponent } from './create-research/parameters-form/parameters-form.component';
import { AnalyzeOptionsFormComponent } from './create-research/analyze-options-form/analyze-options-form.component';
import { ResearchLandingComponent } from './research-landing/research-landing/research-landing.component';
import {NotificationModule} from "../common/notification/notification.module";
import {MatMenuModule} from "@angular/material/menu";
import {MatCardModule} from "@angular/material/card";
import {MatTableModule} from "@angular/material/table";
import {MatInputModule} from "@angular/material/input";
import {MatSelectModule} from "@angular/material/select";
import {MatButtonModule} from "@angular/material/button";
import {MatGridListModule} from "@angular/material/grid-list";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatFormFieldModule} from "@angular/material/form-field";


@NgModule({
    imports: [
        CommonModule,
        MatMenuModule,
        MatCardModule,
        MatTableModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        MatGridListModule,
        MatCheckboxModule,
        MatFormFieldModule,
        ReactiveFormsModule,
        ApplicationRoutingModule,
    ],
    declarations: [
        ApplicationComponent,
        CreateResearchComponent,
        GeneralFormComponent,
        ResearchListingComponent,
        ParametersFormComponent,
        AnalyzeOptionsFormComponent,
        ResearchLandingComponent
    ]
})
export class ApplicationModule {
}
