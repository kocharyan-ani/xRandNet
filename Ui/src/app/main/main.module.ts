import {NgModule} from "@angular/core";
import {AdminPageComponent} from "./admin/admin-page-component/admin-page.component";
import {LoginComponent} from "./login-component/login.component";
import {RegisterComponent} from "./register-component/register.component";
import {HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
import {MainRoutingModule} from "./main.routing.module";
import {HomePageComponent} from "./home-page-component/home-page.component";
import {DownloadComponent} from "./download-component/download.component";
import {NewsComponent} from "./news-component/news.component";
import {InfoComponent} from "./info-component/info.component";
import {HttpModule} from "../common/http/http.module";
import {UploadService} from "./services/upload-service/upload.service";
import {FlexLayoutModule} from "@angular/flex-layout";
import {SoftwareUploadDialog} from "./admin/software-upload-dialog/software-upload-dialog";
import {UserManualUploadDialog} from "./admin/user-manual-upload-dialog/user-manual-upload-dialog";
import {BugDetailsDialog} from "./admin/bug-details-dialog/bug-details-dialog";
import {MatCardModule} from "@angular/material/card";
import {MatButtonModule} from "@angular/material/button";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatListModule} from "@angular/material/list";
import {MatIconModule} from "@angular/material/icon";
import {MatCommonModule} from "@angular/material/core";
import {MatDialogModule} from "@angular/material/dialog";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {MatTooltipModule} from "@angular/material/tooltip";
import {MatSelectModule} from "@angular/material/select";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";


@NgModule({
    declarations: [
        AdminPageComponent,
        LoginComponent,
        RegisterComponent,
        HomePageComponent,
        DownloadComponent,
        NewsComponent,
        InfoComponent,
        SoftwareUploadDialog,
        UserManualUploadDialog,
        BugDetailsDialog
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        MainRoutingModule,
        MatButtonModule,
        MatFormFieldModule,
        ReactiveFormsModule,
        MatInputModule,
        MatListModule,
        MatIconModule,
        MatCommonModule,
        MatDialogModule,
        FlexLayoutModule,
        MatProgressBarModule,
        MatTooltipModule,
        HttpModule.forRoot(),
        MatSelectModule,
        MatCardModule,
        MatProgressSpinnerModule,
    ],
    entryComponents: [
        SoftwareUploadDialog,
        UserManualUploadDialog,
        BugDetailsDialog
    ],
    providers: [UploadService],
})
export class MainModule {
}
