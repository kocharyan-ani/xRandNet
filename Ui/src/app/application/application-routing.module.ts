import { NgModule } from '@angular/core';
import {ApplicationComponent} from './application/application.component';
import {RouterModule, Routes} from '@angular/router';
import {CreateResearchComponent} from "./create-research/create-research/create-research.component";
import {ResearchListingComponent} from "./research-landing/research-listing/research-listing.component";
import {ResearchLandingComponent} from "./research-landing/research-landing/research-landing.component";

const routes: Routes = [
    {
        path: '',
        component: ApplicationComponent,
        children: [
            {
                path: '',
                component: ResearchLandingComponent
            },
            {
                path: 'create-research',
                component: CreateResearchComponent
            }
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApplicationRoutingModule { }
