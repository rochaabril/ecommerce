import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  // private apiUrl = 'http://localhost:5000/api/Imagen/images/'; // Cambia esto según la URL de tu backend
  private apiUrl = environment.apiUrl+'Imagen/images';
  constructor(private http: HttpClient) {}

  getImageUrl(file: string | undefined): string {
    return file ? `${this.apiUrl}${file}` : 'path/to/default-image.png';
  }
}
