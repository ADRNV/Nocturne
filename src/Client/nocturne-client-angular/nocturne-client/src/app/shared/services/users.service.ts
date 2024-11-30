import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { RecordsResponse } from '../../models/records-response';
import { User } from '../../models/user';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private baseUrl: string;

  constructor(private httpClient: HttpClient) { 
    this.baseUrl = environment.apiUrl;
  }

  getUsers(page: number, size: number): Observable<RecordsResponse<User>>{
    
    var params = new HttpParams();

    params = params.append("page", page);
    params = params.append("pageSize", size);

    return this.httpClient.get<RecordsResponse<User>>(this.baseUrl+"Users/page",
      {
        params: params
      }
    );
  }
}
