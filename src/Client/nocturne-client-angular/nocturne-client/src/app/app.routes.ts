import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthComponent } from './features/auth/auth/auth.component';
import { NgModule } from '@angular/core';
import { AboutComponent } from './features/about/about.component';
import { RegisterUserComponent } from './features/register-user/register-user.component';

export const routes: Routes = [
    {path:'', redirectTo:'about', pathMatch:'full'},
    {path:'about', component: AboutComponent},
    {path: 'auth', component: AuthComponent},
    {path: 'register', component: RegisterUserComponent }
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
export class AppRoutingModule {
  
}
  