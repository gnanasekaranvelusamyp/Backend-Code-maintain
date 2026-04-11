import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  message: string = '';

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const url = 'https://your-backend-url.com/api/login'; // Replace with your backend API
      this.http.post(url, this.loginForm.value).subscribe({
        next: (res: any) => {
          this.message = 'Login successful!';
          console.log(res);
        },
        error: (err) => {
          this.message = 'Invalid credentials';
          console.error(err);
        }
      });
    } else {
      this.message = 'Please fill all fields';
    }
  }
}
