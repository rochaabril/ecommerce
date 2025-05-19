import { Component, OnInit } from '@angular/core';
import { AuthService } from '../service/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  user = { name: 'Nombre de Ejemplo', email: 'correo@ejemplo.com', direccion: 'calle 123', telefono: '2344556743' }; // Datos ficticios o carga desde el servicio
  isEditing = false;
  usuarioId: number | null = null;
  person: any;
  constructor(private authService: AuthService, private router: Router) {}
  ngOnInit(): void {
    this.getUser(); // Llama al método al inicializar el componente
  }
  obtenerID(){
    this.usuarioId = this.authService.getUserId();
  
  }
  getUser(){
    this.obtenerID();
    if(!this.usuarioId){
      return
    }
    this.authService.getByIdUser(this.usuarioId).subscribe(data =>{
       this.person = data
       console.log('PERSON DATA',this.person)
    });
  }
  toggleEdit(): void {
    this.isEditing = !this.isEditing;
  }
  saveChanges(): void {
    // Llama al servicio para actualizar los datos del usuario
    if(!this.usuarioId){
      return
    }
  
    const updatedUser = {
      id: this.usuarioId,
      nombre: this.person.nombre,
      email: this.person.email,
      direccion: this.person.direccion,
      whatsapp: this.person.whatsapp,
      password: this.person.password,  // Si también deseas actualizar la contraseña, asegúrate de agregarla
    };
    if(!this.usuarioId){
      return
    }
  
    this.authService.updateUser(this.usuarioId, updatedUser).subscribe(
      (response) => {
        // Al finalizar la actualización, cambia el estado de edición
        this.isEditing = false;
        console.log('Usuario actualizado:', response);
      },
      (error) => {
        console.error('Error al actualizar el usuario:', error);
      }
    );
  }
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
