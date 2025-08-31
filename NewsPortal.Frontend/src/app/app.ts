import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth';

@Component({
    selector: 'app-root',
    standalone: false,
    template: `
    <app-header></app-header>
    <router-outlet></router-outlet>
  `
})
export class App implements OnInit {
    constructor(private authService: AuthService) { }

    ngOnInit(): void {
        // Проверяем токен каждые 5 минут
        setInterval(() => {
            this.authService.checkTokenValidity();
        }, 5 * 60 * 1000);
    }
}
