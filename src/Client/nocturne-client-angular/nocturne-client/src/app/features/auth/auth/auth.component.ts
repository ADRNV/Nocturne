import { Component, inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '../../../shared/services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, Observable, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { error } from 'console';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent {
  
  private _snackBar = inject(MatSnackBar);
  constructor(private authService: AuthService) {

  }

  onLogin(form:NgForm){

    let email = form.control.value.username;
    let password = form.control.value.password;

    this.authService.auth(email, password).subscribe(response => {
      this._snackBar.open("Login succesful", "Close", {duration: 5000})
    }, 
    errorMsg => {
      this._snackBar.open(errorMsg, "Close", {duration: 5000})
    }, 
    () => console.log("FireBase request is OK"));
  }
}
