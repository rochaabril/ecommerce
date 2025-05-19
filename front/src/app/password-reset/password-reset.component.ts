import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Toast } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { AuthService } from '../service/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.css']
})
export class PasswordResetComponent{
  resetForm: FormGroup;

  constructor(private fb: FormBuilder, private messageService: MessageService,private router: Router, private authService: AuthService) {
    this.resetForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }
  reset(): void {
    const email = this.resetForm.value.email; // Obtener el valor del correo
    this.authService.resetPass(email).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Se ha enviado un mensaje de recuperación a su correo'
        });
        this.resetForm.reset(); // Limpiar el formulario después de enviar
       
      },
      
    });
  }

  goToLogin() {
    this.router.navigate(['/login']); // Navega a la página de login
  }
  onSubmit(): void {
    if (this.resetForm.valid) {
      this.messageService.add({
        severity: 'success',
        summary: 'Éxito',
        detail: 'Se ha enviado un mensaje de recuperación a su correo'
      });
      this.resetForm.reset(); // Limpia el formulario después de enviar
    }
  }
}
