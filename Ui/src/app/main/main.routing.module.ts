import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AdminPageComponent} from "./admin/admin-page-component/admin-page.component";
import {AdminGuard} from "./guards/admin-guard/admin.guard";
import {LoginComponent} from "./login-component/login.component";
import {RegisterComponent} from "./register-component/register.component";
import {DownloadComponent} from "./download-component/download.component";
import {HomePageComponent} from "./home-page-component/home-page.component";
import {NewsComponent} from "./news-component/news.component";
import {InfoComponent} from "./info-component/info.component";

const routes: Routes = [
    {
        path: 'admin',
        component: AdminPageComponent,
        canActivate: [AdminGuard],
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: 'info',
        component: InfoComponent
    },
    {
        path: 'news',
        component: NewsComponent
    },
    {
        path: 'downloads',
        component: DownloadComponent
    },
    {
        path: 'home',
        component: HomePageComponent
    },
    {path: '**', redirectTo: 'home'}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MainRoutingModule {
}
