import { NgModule } from '@angular/core';
import { SharedModule } from './modules/shared/shared.module';
import { AppComponent } from './app.component';
import { AuthComponent } from './features/auth/auth/auth.component';
import { AdministrationModule } from './modules/administration/administration/administration.module';

@NgModule({
  declarations: [
    AppComponent 
  ],
  imports: [
    SharedModule,
    AdministrationModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
