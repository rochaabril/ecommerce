import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userRole: string = '';
  private apiUrl = environment.apiUrl; // Ajusta la URL según tu backend

  constructor(private http: HttpClient) {}
  getUserRole(): string {
    // Primero busca en el localStorage, si no está, usa el rol de la variable interna
    const storedRole = localStorage.getItem('role');
    if (storedRole) {
      
      return storedRole;  // Si el rol está en localStorage, lo retorna
    }
    return this.userRole || '';  // Si no está en localStorage, retorna el rol almacenado en la clase
  }

  // Método para establecer el rol del usuario (esto puede ser parte de la autenticación)
  setUserRole(role: string) {
    this.userRole = role;
    localStorage.setItem('role', role);  // Guarda el rol en localStorage para acceso posterior
  }
  resetPass(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Usuario/request-reset-password`, JSON.stringify(email), {
      headers: { 'Content-Type': 'application/json' }
    });
  }
  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/login`, { email, password }).pipe(
      tap((response: any) => {
        const token = response.token;
        localStorage.setItem('token', token);  // Guarda el token en localStorage
      
        const userRole = this.getUserRoleFromToken(token);
      if (userRole) {
        this.setUserRole(userRole); // Guarda el rol en el servicio
      }
      })
    );
  }
  getUserRoleFromToken(token: string): string | null {
    const decodedToken = this.decodeToken(token);
    if (decodedToken && decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]) {
      return decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }
    return null; // Si no hay rol, retorna null
  }
  decodeToken(token: string) {
    try {
      const payloadBase64 = token.split('.')[1];
      const payloadJson = JSON.parse(atob(payloadBase64));
      return payloadJson; // Devuelve el payload completo decodificado
    } catch (error) {
      console.error('Error al decodificar el token:', error);
      return null;
    }
  }
  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true; // Si no hay token, está expirado
  
    try {
      const payloadBase64 = token.split('.')[1]; // El payload está en la segunda parte del token
      const payloadJson = JSON.parse(atob(payloadBase64)); // Decodifica el payload en formato JSON
      const expirationDate = payloadJson.exp * 1000; // Convierte de segundos a milisegundos
      return expirationDate < Date.now(); // Comprueba si el token ha expirado
    } catch (error) {
      console.error('Error al procesar el token:', error);
      return true; // Si hay algún error, consideramos que el token ha expirado
    }
  }
  getToken(): string | null {
    return localStorage.getItem('token'); // Recupera el token desde localStorage
  }
  postUser(userData: { nombre: string; email: string; password: string; roleId: number; roleName: string; direccion: string; whatsapp: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/Usuario`, userData).pipe(
      tap((response: any) => {
        console.log('Usuario creado con éxito', response);
      }),
      catchError(error => {
        console.error('Error al crear usuario', error);
        return throwError(error); // Maneja el error según lo necesario
      })
    );
  }
  getByIdUser(id: number){
    return this.http.get(`${this.apiUrl}/Usuario/${id}`);
  }

  updateUser(id: number, userData: {id:number; nombre?: string; email?: string; password?: string; roleId?: number; direccion?: string; whatsapp?: string; emailVerificado?: boolean }): Observable<any> {
    return this.http.put(`${this.apiUrl}/Usuario/${id}`, userData);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
  getUserData(){}
  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !!token; // Devuelve true si hay un token
  }
  logout(): void {
    localStorage.removeItem('token');
    localStorage.clear();
  }
  getUserId(): number | null {
    const token = this.getToken();
    if (!token) return null;
  
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
  
      // Busca el ID del usuario en el claim con el nombre completo
      return payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
        ? Number(payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"])
        : null;
    } catch (error) {
      console.error('Error al decodificar el token:', error);
      return null;
    }
  }
}
