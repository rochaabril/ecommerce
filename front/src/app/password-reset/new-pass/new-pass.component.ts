import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-new-pass',
  templateUrl: './new-pass.component.html',
  styleUrls: ['./new-pass.component.css']
})
export class NewPassComponent implements OnInit {
  resetForm: FormGroup;
  token: string = '';
  loading: boolean = false;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.resetForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    // Capturar el token de la URL
    this.token = this.route.snapshot.queryParamMap.get('token') || '';
  }

  onSubmit(): void {
    if (this.resetForm.invalid) {
      return;
    }

    const newPassword = this.resetForm.value.newPassword;
    const confirmPassword = this.resetForm.value.confirmPassword;

    if (newPassword !== confirmPassword) {
      this.errorMessage = 'Las contraseñas no coinciden';
      return;
    }

    this.loading = true;

    // Hacer la petición al endpoint del backend
    this.http
  .post('http://localhost:5000/api/usuario/reset-password', {
    token: this.token,
    newPassword: newPassword,
  })
  .subscribe({
    next: (response: any) => {
      if (response.success) {
        alert(response.message);
        this.router.navigate(['/login']);
      } else {
        this.errorMessage = response.message || 'Ocurrió un error inesperado.';
      }
    },
    error: (err) => {
      this.errorMessage = 'Ocurrió un error al restablecer la contraseña.';
      console.error(err);
    },
    complete: () => {
      this.loading = false;
    },
  });
  }
}