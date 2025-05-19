import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Categoria } from '../model/categoria';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {
  // private apiUrl = 'http://localhost:5000/api/Categoria';
  private apiUrl = environment.apiUrl+'/Categoria';

  
  constructor(private http: HttpClient) {}

  // Obtener todas las categorías
  getAll(): Observable<Categoria[]> {
    return this.http.get<Categoria[]>(`${this.apiUrl}`);
  }

  // Obtener categoría por ID
  getById(id: number): Observable<Categoria> {
    return this.http.get<Categoria>(`${this.apiUrl}/usuario/${id}`);
  }
 

  // Crear una nueva categoría
  create(categoria: Categoria): Observable<Categoria> {
    return this.http.post<Categoria>(`${this.apiUrl}`, categoria);
  }

  // Actualizar una categoría existente
  update(id: number, categoria: Categoria): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, categoria);
  }

  // Eliminar una categoría
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
