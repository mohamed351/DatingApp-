import { AuthService } from './../services/auth.service';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import {tap } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor{

    constructor(private authService:AuthService, private router:Router) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.authService.IsAuthicated()) {
          let clonedRequest =  req.clone({
                headers: req.headers.set("Authorization", "Bearer" + this.authService.GetToken())
            });
           return next.handle(clonedRequest).pipe(
                tap(
                    success => { },
                    error => {
                        if (error.status == 401) {
                            this.authService.RemoveToken();
                            this.router.navigateByUrl("/user");
                        }
                    }
                )
            );
        }
        else {
            return next.handle(req.clone());
        }
    }

}