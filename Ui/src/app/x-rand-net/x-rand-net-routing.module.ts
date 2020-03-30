import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AuthGuard} from "../main/guards/auth-guard/auth.guard";

const routes: Routes = [
    {
        path: 'application',
        loadChildren: '../application/application.module#ApplicationModule',
        canActivate: [AuthGuard],
    },
    {
        path: '',
        loadChildren: '../main/main.module#MainModule'
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class XRandNetRoutingModule {
}
