import {NgModule} from '@angular/core';
import {XRandNetComponent} from './x-rand-net-component/x-rand-net.component';
import {BrowserModule} from '@angular/platform-browser';
import {XRandNetRoutingModule} from './x-rand-net-routing.module';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {HttpClientModule} from "@angular/common/http";
import {HttpModule} from "../common/http/http.module";
import {BugReportDialog, MainComponent} from "../main/main-component/main.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
import {JwtModule} from "@auth0/angular-jwt";
import {MatCommonModule, MatOptionModule} from "@angular/material/core";
import {MatDialogModule} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatSelectModule} from "@angular/material/select";
import {MatSidenavModule} from "@angular/material/sidenav";
import {MatListModule} from "@angular/material/list";
import {MatIconModule} from "@angular/material/icon";

export function tokenGetter() {
    return localStorage.getItem("jwt");
}

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        BrowserModule,
        BrowserAnimationsModule,
        XRandNetRoutingModule,
        HttpClientModule,
        HttpModule.forRoot(),
        MatDialogModule,
        MatButtonModule,
        MatFormFieldModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCommonModule,
        MatSelectModule,
        MatOptionModule,
        MatToolbarModule,
        MatSidenavModule,
        MatListModule,
        MatIconModule,
        JwtModule.forRoot({
            config: {
                tokenGetter: tokenGetter,
                whitelistedDomains: ['xrand.net', "localhost"],
                blacklistedRoutes: []
            }
        })
    ],
    declarations: [XRandNetComponent, MainComponent, BugReportDialog],
    bootstrap: [XRandNetComponent],
    exports: [BugReportDialog],
    entryComponents: [
        BugReportDialog
    ]
})
export class XRandNetModule {
}
