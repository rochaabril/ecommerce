import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthService } from 'src/app/service/auth.service';
import { CartService } from 'src/app/service/cart.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm!: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService, private cartService: CartService,  private messageService: MessageService) {
    this.registerForm = this.fb.group({
      nombre: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      whatsapp: ['', Validators.required],
      direccion: ['', Validators.required],
      roleId: ['2', Validators.required],
      roleName: ['Cliente', Validators.required],
    });
  }

  post(): void {
    if (this.registerForm.valid) {
      this.authService.postUser(this.registerForm.value).subscribe({
        next: (response) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Usuario creado con exito'
          });
          console.log('Usuario creado', response);
          this.cartService.getOrCreateCart(response.id).subscribe({})
          this.router.navigate(['/login']); // Redirige al login después del registro
        },
        error: (error) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Error al crear usuario:' + error.message
          });
          console.error('Error creando usuario', error);
        }
      });
    }
  }
}