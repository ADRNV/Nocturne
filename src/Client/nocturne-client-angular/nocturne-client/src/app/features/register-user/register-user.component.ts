import { AfterViewInit, Component, ElementRef, OnInit, Renderer2, signal, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PasswordValidator } from '../core/validators/password';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.css'
})
export class RegisterUserComponent implements OnInit {


  public registrationForm!: FormGroup;

  
  private emailControl = new FormControl(null, [Validators.email, Validators.required]);
  private usernameControl = new FormControl(null, [Validators.required, Validators.minLength(6)]);
  private passwordControl = new FormControl(null, [Validators.required,  Validators.minLength(8)]);
  private passwordConfirm = new FormControl(null, [Validators.required, PasswordValidator.passwordEqualityValidator(this.passwordControl)]);
  constructor(private authService: AuthService, private snackbar: MatSnackBar) {
   
  }

  ngOnInit(): void {
    this.registrationForm = new FormGroup( 
      {
        "email": this.emailControl,
        "username": this.usernameControl,
        "password": this.passwordControl,
        "passwordConfirm":  this.passwordConfirm
      }
    )
  }

  register(){
    this.authService.create(this.registrationForm.value.email,  
      this.registrationForm.value.username, 
      this.registrationForm.value.password, 
      this.registrationForm.value.image, 
      "User")
    .subscribe(r => {
      this.snackbar.open("Signed up", "Ok", {duration: 4000})
    });
  }

  //TODO: Move to diretive
  switchPasswordShow(element: HTMLInputElement){
    
    if(element.type != "text"){
      element.type = "text";
    } else{
      element.type = "password";
    }
  }
}
