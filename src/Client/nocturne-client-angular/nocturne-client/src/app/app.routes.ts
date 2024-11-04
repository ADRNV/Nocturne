import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthComponent } from './features/auth/auth/auth.component';
import { NgModule } from '@angular/core';
import { AboutComponent } from './features/about/about.component';

export const routes: Routes = [
    {path:'', redirectTo:'about', pathMatch:'full'},
    {path:'about', component: AboutComponent},
    {path: 'auth', component: AuthComponent},
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
export class AppRoutingModule {
  
}
  