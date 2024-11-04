import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string;

  constructor(private httpClient: HttpClient) { 
    this.baseUrl = environment.apiUrl;
  }

  auth(userName: string, password: string){

    let request = new HttpParams();

    request = request.set("userName", userName);
    request = request.set("password", password);
  
    return this.httpClient.post(this.baseUrl+"User/sign-in", null,
    {
      params: request
    }).pipe(catchError(errorResponse => this.handleError(errorResponse)));
  }

  create(email: string, userName:string, password: string, image: string, role: string){

    let request = new HttpParams();

    request = request.set("login", email);
    request = request.set("userName", userName);
    request = request.set("password", password);

    return this.httpClient.post(this.baseUrl+"User/create-one", null,
    {
      params: request
    }).pipe(catchError(errorResponse => this.handleError(errorResponse)));
  }

  private handleError(errorResponse: HttpErrorResponse){
    
    let message = "Unknown error";
    
    switch(errorResponse.status){
      case 400:
        message = "User not exists"
      break;
      case 409:
        message = "User are exists"
      break;
    }
    return throwError(() => message);
  }
}
