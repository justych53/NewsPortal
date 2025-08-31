import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from './auth';

@Injectable({
    providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {

    constructor(
        private authService: AuthService,
        private router: Router
    ) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.authService.getToken();
        let authReq = req;

        if (token) {
            authReq = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
        }

        return next.handle(authReq).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 401) {
                    if (this.isAuthRequest(req)) {

                        console.log('Authentication failed - wrong credentials');
                    } else {
                        this.authService.logout();
                        this.router.navigate(['/']);
                        alert('Сессия истекла. Пожалуйста, войдите снова.');
                    }
                }
                return throwError(error);
            })
        );
    }

    private isAuthRequest(req: HttpRequest<any>): boolean {
        return req.url.includes('/api/auth/');
    }
}
