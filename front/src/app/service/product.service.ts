import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { Product } from '../model/product';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from 'src/environments/environment';
    
@Injectable()
export class ProductService {
    
    // private apiUrl = 'http://localhost:5000/api/Producto'; // Ajusta la URL según tu backend
    // private apiUrlExcel = 'http://localhost:5000/api/Export/ExportarProductos';
    private apiUrl = environment.apiUrl+'/Producto';
    private apiUrlExcel = environment.apiUrl+'/Export/ExportarProductos';


    constructor(private http: HttpClient, private authService: AuthService) {}

    exportExcel(): Observable<Blob> {
      return this.http.get(this.apiUrlExcel, { responseType: 'blob' });
    }
    getLatest10(): Observable<Product[]> {
      return this.http.get<Product[]>(`${this.apiUrl}/novedades`);
    }
    getProducts(): Observable<Product[]> {
        return this.http.get<Product[]>(this.apiUrl);
        
    }
    getByIdProducts(id:number): Observable<Product> {
      return this.http.get<Product>(`${this.apiUrl}/${id}`);
      
  }

    createProduct(product: Product, file?: File): Observable<Product> {
        const formData = new FormData();
        
        // Añadir los datos del producto al formulario
        
        formData.append('Nombre', product.nombre || '');
        formData.append('Precio', product.precio?.toString() || '0');
        formData.append('Observaciones', product.observaciones || '');
        formData.append('Stock', String(product.stock));
        formData.append('CantidadMinima', product.cantidadMinima?.toString() || '0');
        formData.append('CategoriaId', product.categoriaId?.toString() || '0');
      
        // Añadir el archivo al formulario
        if (file) {
          formData.append('archivo', file);
        }
        const headers = new HttpHeaders({
            Authorization: `Bearer ${this.authService.getToken()}`, // Obtén el token desde tu servicio de autenticación
          });
        // Enviar la petición POST
        return this.http.post<Product>(this.apiUrl, formData, { headers });
      }
      updateProduct(product: Product, file?: File): Observable<Product> {
        const headers = new HttpHeaders({
          Authorization: `Bearer ${this.authService.getToken()}`, // Incluye solo el token si es necesario
        });
      
        const formData = new FormData();
        if (product.id) formData.append('id', product.id.toString());
        if (product.nombre) formData.append('nombre', product.nombre);
        if (product.precio !== undefined) formData.append('precio', product.precio.toString());
        if (product.stock !== undefined) formData.append('stock', product.stock.toString());
        if (product.cantidadMinima !== undefined) formData.append('cantidadMinima', product.cantidadMinima.toString());
        if (product.categoriaId !== undefined) formData.append('categoriaId', product.categoriaId.toString());
        if (file) formData.append('archivo', file, file.name); // Asegúrate de usar el nombre correcto del archivo
      
        return this.http.put<Product>(`${this.apiUrl}/${product.id}`, formData, { headers });
      }
      
    deleteProduct(productId: number): Observable<void> {
        const headers = new HttpHeaders({
            Authorization: `Bearer ${this.authService.getToken()}`, // Si es necesario, incluye el token
          });
        return this.http.delete<void>(`${this.apiUrl}/${productId}`,{ headers });
    }
    actualizarPrecios(porcentaje: number): Observable<any> {
      const request = { Porcentaje: porcentaje };
      return this.http.post<any>(`${this.apiUrl}/ActualizarPrecios`, request);
    }
};