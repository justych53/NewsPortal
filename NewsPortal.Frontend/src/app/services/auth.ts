import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface LoginRequest {
    userName: string;
    password: string;
}

export interface LoginResponse {
    token: string;
}

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = '/api/auth';
    private tokenKey = 'auth_token';
    private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasValidToken());

    constructor(
        private http: HttpClient,
        private router: Router
    ) { }


    private hasValidToken(): boolean {
        const token = this.getToken();
        if (!token) return false;


        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const expiration = payload.exp * 1000; 
            return Date.now() < expiration;
        } catch (error) {
            console.error('Error parsing token:', error);
            return false;
        }
    }

    login(credentials: LoginRequest): Observable<LoginResponse> {
        const payload = {
            UserName: credentials.userName,
            Password: credentials.password
        };

        return this.http.post<LoginResponse>(`${this.apiUrl}/login`, payload).pipe(
            tap(response => {
                if (response && response.token) {
                    this.setToken(response.token);
                    this.isAuthenticatedSubject.next(true);
                    this.router.navigate(['/admin']);
                }
            })
        );
    }

    logout(): void {
        localStorage.removeItem(this.tokenKey);
        this.isAuthenticatedSubject.next(false);
        this.router.navigate(['/']);
    }

    getToken(): string | null {
        return localStorage.getItem(this.tokenKey);
    }

    isAuthenticated(): Observable<boolean> {
        return this.isAuthenticatedSubject.asObservable();
    }

    // Проверка валидности токена
    isTokenValid(): boolean {
        return this.hasValidToken();
    }


    checkTokenValidity(): void {
        const isValid = this.hasValidToken();
        if (!isValid) {
            this.logout();
        }
        this.isAuthenticatedSubject.next(isValid);
    }

    private setToken(token: string): void {
        localStorage.setItem(this.tokenKey, token);
    }
}
