import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { AuthService } from '../service/auth.service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm!: FormGroup;
  registerForm!: FormGroup;
  isRegistering: boolean = false;

  constructor(private fb: FormBuilder, private router: Router,private authService: AuthService,  private messageService: MessageService) {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.registerForm = this.fb.group({
      nombre: ['', ],
      email: ['',],
      password: ['', ],
      whatsapp:['',],
      direccion:['',],
      roleId: ['2',],
      roleName: ['Cliente',],
    });
  }

  toggleRegister(): void {
    this.router.navigate(['/register']);
  }
  contra(): void {
    this.isRegistering = !this.isRegistering;
    this.router.navigate(['/reset']);
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      this.authService.login(email, password).subscribe({
        next: () => {
          // Redirigir al usuario después de iniciar sesión
          this.router.navigate(['/products']);
        },
        error: (err) => {
          // Maneja el error de inicio de sesión aquí
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Error al iniciar sesion:'+ err.message
          });
          console.error('Error en el inicio de sesión', err);
        },
      });
    }
  }

 post(){
  this.authService.postUser(this.registerForm.value).subscribe({
    next: (response) => {
      this.router.navigate(['/login']);
      console.log('Usuario creado', response);
    },
    error: (error) => {
      console.error('Error creando usuario', error);
    }
  });
 }
 
 
}
