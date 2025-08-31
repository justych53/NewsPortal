import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth';
import { map, tap } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(private authService: AuthService, private router: Router) { }

    canActivate() {
        // Проверяем валидность токена перед навигацией
        this.authService.checkTokenValidity();

        return this.authService.isAuthenticated().pipe(
            tap(authenticated => {
                console.log('AuthGuard: Authentication status:', authenticated);
                if (!authenticated) {
                    this.router.navigate(['/']);
                }
            }),
            map(authenticated => {
                return authenticated;
            })
        );
    }
}
