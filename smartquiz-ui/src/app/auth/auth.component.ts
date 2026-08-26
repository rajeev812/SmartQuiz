import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent {
  mode: 'register' | 'login' = 'register';
  firstName = '';
  lastName = '';
  email = '';
  password = '';
  errorMessage = '';
  isBusy = false;

  constructor(private http: HttpClient, private router: Router) {}

  submit(): void {
    this.errorMessage = '';
    this.isBusy = true;
    const endpoint = this.mode === 'register' ? 'register' : 'login';
    const body = this.mode === 'register'
      ? { firstName: this.firstName, lastName: this.lastName, email: this.email, password: this.password }
      : { email: this.email, password: this.password };

    this.http.post<{ firstName: string; lastName: string; token: string }>(`http://localhost:5214/api/Auth/${endpoint}`, body).subscribe({
      next: response => {
        localStorage.setItem('smartquiz.studentName', `${response.firstName} ${response.lastName}`.trim());
        localStorage.setItem('smartquiz.userEmail', body.email);
        localStorage.setItem('smartquiz.token', response.token);
        this.router.navigate(['/home']);
      },
      error: error => {
        this.errorMessage = error.error?.message || 'Unable to complete this request.';
        this.isBusy = false;
      }
    });
  }

  switchMode(): void {
    this.mode = this.mode === 'register' ? 'login' : 'register';
    this.errorMessage = '';
  }
}
