import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-header',
  templateUrl: './header.html',
  styleUrls: ['./header.scss'],
  standalone: false
})
export class Header implements OnInit {
  isAuthenticated = false;
  showLoginModal = false;
  loginForm: FormGroup;
  loginError = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    console.log('Header: Component initialized');
    this.authService.isAuthenticated().subscribe(auth => {
      console.log('Header: Auth state changed:', auth);
      console.log('Header: Current token:', this.authService.getToken());
      this.isAuthenticated = auth;
    });
  }

  openLogin(): void {
    console.log('Header: Opening login modal');
    this.showLoginModal = true;
    this.loginError = '';
    this.loginForm.reset();
  }

  closeLogin(): void {
    console.log('Header: Closing login modal');
    this.showLoginModal = false;
    this.loginError = '';
  }

  onSubmit(): void {
    console.log('Header: Login form submitted');

    if (this.loginForm.invalid) {
      console.log('Header: Form invalid');
      this.loginError = 'Заполните все поля';
      return;
    }

    console.log('Header: Form valid, calling authService.login');
    console.log('Header: Login data:', this.loginForm.value);

    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        console.log('Header: Login successful in subscribe');
        console.log('Header: Response:', response);
        this.closeLogin();
      },
      error: (error) => {
        console.error('Header: Login error:', error);
        console.error('Header: Error details:', error.error);
        this.loginError = error.error?.error || 'Ошибка авторизации';
      },
      complete: () => {
        console.log('Header: Login observable completed');
      }
    });
  }

  logout(): void {
    console.log('Header: Logout called');
    this.authService.logout();
  }
}
