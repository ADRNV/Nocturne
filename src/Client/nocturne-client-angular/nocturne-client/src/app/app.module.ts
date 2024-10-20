import { NgModule } from '@angular/core';
import { SharedModule } from './modules/shared/shared.module';
import { AppComponent } from './app.component';
import { AuthComponent } from './features/auth/auth/auth.component';

@NgModule({
  declarations: [
    AppComponent 
  ],
  imports: [
    SharedModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
