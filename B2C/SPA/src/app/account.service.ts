import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyProfile, ResponseMessage } from './profile'
import { protectedResources } from './auth-config';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  url = protectedResources.MyProfileApi.endpoint;
  myProfile: MyProfile = null;

  constructor(private http: HttpClient) { }

  deleteMyProfile(id: string): Observable<unknown>{
    const url = `${this.url}/${id}`;
    return this.http.delete<ResponseMessage>(url);
  }

  getMyProfile(id: string) {
    const url = `${this.url}/${id}`;    
    return this.http.get<MyProfile>(url);    
  }
}
