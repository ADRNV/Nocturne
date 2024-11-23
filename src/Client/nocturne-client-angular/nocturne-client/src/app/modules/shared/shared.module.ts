import { NgModule } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { MatButton, MatButtonModule } from '@angular/material/button'
import { MatFormField, MatInput, MatInputModule } from '@angular/material/input'
import { AuthComponent } from '../../features/auth/auth/auth.component';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldControl, MatFormFieldModule } from '@angular/material/form-field';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../features/header/header.component';
import { AppRoutingModule } from '../../app.routes';
import { AboutComponent } from '../../features/about/about.component';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule, SimpleSnackBar } from '@angular/material/snack-bar'
import { RegisterUserComponent } from '../../features/register-user/register-user.component';
import {MatIconModule} from '@angular/material/icon';

@NgModule({
  declarations: [
    AuthComponent,
    HeaderComponent,
    AboutComponent,
    RegisterUserComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    RouterOutlet,
    AppRoutingModule,
    HttpClientModule,
    MatSnackBarModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule
  ],
  exports:[
    AuthComponent,
    HeaderComponent,
    AboutComponent,
    RouterOutlet,
    RouterModule
  ]
})
export class SharedModule { }
