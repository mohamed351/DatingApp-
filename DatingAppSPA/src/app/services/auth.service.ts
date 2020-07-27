import { LoginFormData } from './../models/login';
import { RegistrationFormData } from './../models/registration';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment  } from '../../environments/environment'
import { tap, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }

  IsAuthicated():boolean {
    return localStorage.getItem("token") == null ? false : true;
  }
  GetToken() {
    return localStorage.getItem("token");
  }
  SetToken(token:string) {
    if (localStorage.getItem("token") == null) {
      localStorage.setItem("token", token);
    }
    else
    {
      localStorage.removeItem("token");
      localStorage.setItem("token", token);
    }
  }
  RemoveToken() {
    localStorage.removeItem("token");
  }
  Registration(formData:RegistrationFormData) {
    
    return  this.http.post(environment.Base_URl + "/api/Auth/register", formData);
  
  }
  Login(LoginFormData:LoginFormData) {
    return this.http.post(environment.Base_URl + "/api/Auth/login", LoginFormData).pipe(
      map((response:any) => {
        localStorage.setItem("token", response.token);
        console.log("HelloWorld");
      })
     
    );
  }

}
