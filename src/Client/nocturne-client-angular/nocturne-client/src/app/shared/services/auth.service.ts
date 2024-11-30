import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, of, tap, throwError } from 'rxjs';
import { User } from '../../models/user';
import { RefreshToken, Token } from '../../models/token';
import { response } from 'express';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string;

  public token = new BehaviorSubject<Token | null>(null);

  constructor(private httpClient: HttpClient) { 
    this.baseUrl = environment.apiUrl;
  }

  auth(userName: string, password: string) : Observable<Token> {

    let request = new HttpParams();

    request = request.set("userName", userName);
    request = request.set("password", password);
  
    return this.httpClient.post<Token>(this.baseUrl+"User/sign-in", null,
    {
      params: request
    }).pipe(
        catchError(errorResponse => this.handleError(errorResponse)), 
        tap(response => {
          this.handleAuthetication(response.accessToken, response.refreshToken)
        })
      );
  }

  logout() : Observable<Token | null> {
    this.token.next(null);
    
    localStorage.setItem('AuthToken', '');

    return this.token.asObservable();
  }

  create(email: string, userName:string, password: string, image: string, role: string){

    let request = new HttpParams();

    request = request.set("login", email);
    request = request.set("userName", userName);
    request = request.set("password", password);

    return this.httpClient.post<Token>(this.baseUrl+"User/create-one", null,
    {
      params: request
    }).pipe(catchError(errorResponse => this.handleError(errorResponse)),
    tap(response => {
      this.handleAuthetication(response.accessToken, response.refreshToken)
    }));
  }

  private handleAuthetication(accessToken: string, refreshToken: RefreshToken) {

    let newToken = new Token(accessToken, refreshToken);

    this.token.next(newToken);

    localStorage.setItem('AuthToken', JSON.stringify(newToken));
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
