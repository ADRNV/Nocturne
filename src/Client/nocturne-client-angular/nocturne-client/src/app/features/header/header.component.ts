import { Component, inject } from '@angular/core';
import { AuthService } from '../../shared/services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

  private _snackBar = inject(MatSnackBar);
  
  logined = false;

  constructor(private authService: AuthService){
    authService.token.subscribe((token) => {
      this.logined = token != null;
    })
  }

  onLogout(){
    this.authService.logout().subscribe(() => {
      this._snackBar.open("Loggedout","OK",{duration: 5000})
    });
  }
}
