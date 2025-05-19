import { Injectable } from '@angular/core';
import { Product } from '../model/product';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, of } from 'rxjs';
import { Carrito } from '../model/carrito';
import { ItemCarrito } from '../model/itemCarrito';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItemsSubject = new BehaviorSubject<boolean>(false); // Estado del carrito
  cartItems$ = this.cartItemsSubject.asObservable();
  private carritoApiUrl = environment.apiUrl+'/Carrito'
  private itemCarritoApiUrl = environment.apiUrl+'/ItemCarrito'
  // private carritoApiUrl = 'http://localhost:5000/api/Carrito';
  // private itemCarritoApiUrl = 'http://localhost:5000/api/ItemCarrito';
  private currentCart: Carrito | null = null;
  constructor(private http: HttpClient) {}
  updateCartStatus(isNotEmpty: boolean) {
    this.cartItemsSubject.next(isNotEmpty);  // Actualiza el estado
  }
  getByIdUser(iduser: number): Observable<any> {
    return this.http.get<any>(`${this.carritoApiUrl}/usuario/${iduser}`);
  }
  getOrCreateCart(usuarioId: number): Observable<any> {
    if (this.currentCart) {
      return of(this.currentCart); // Si ya existe, devolver el carrito actual.
    }

    // Si no existe, crear uno nuevo.
    const newCart = {
      usuarioId: usuarioId,
    };
    return this.http.post<any>(this.carritoApiUrl, newCart).pipe(
      map((cart) => {
        this.currentCart = cart;
        return cart;
      })
    );
  }
  // Agregar un producto al carrito
  addItemToCart(item: ItemCarrito): Observable<ItemCarrito> {
    this.updateCartStatus(true);
    return this.http.post<ItemCarrito>(this.itemCarritoApiUrl, item);
  }
  // Carrito CRUD

  getCarritos(): Observable<Carrito[]> {
    return this.http.get<Carrito[]>(this.carritoApiUrl);
  }

  getCarritoById(id: number): Observable<Carrito> {
    return this.http.get<Carrito>(`${this.carritoApiUrl}/${id}`);
  }

  createCarrito(carrito: Carrito): Observable<Carrito> {
    return this.http.post<Carrito>(this.carritoApiUrl, carrito);
  }

  updateCarrito(id: number, carrito: Carrito): Observable<void> {
    return this.http.put<void>(`${this.carritoApiUrl}/${id}`, carrito);
  }

  deleteCarrito(id: number): Observable<void> {
    return this.http.delete<void>(`${this.carritoApiUrl}/${id}`);
  }

  finalizeCarrito(carritoId: number): Observable<string> {
    return this.http.post<string>(
      `${this.carritoApiUrl}/finalizar/${carritoId}`, {carritoId},
      {}
    );
  }

  // ItemCarrito CRUD

  getItems(): Observable<ItemCarrito[]> {
    return this.http.get<ItemCarrito[]>(this.itemCarritoApiUrl);
  }

  getItemByIdCarrito(id: number): Observable<ItemCarrito[]> {
    return this.http.get<ItemCarrito[]>(`${this.itemCarritoApiUrl}/idCarrito/${id}`);
  }

  createItem(item: ItemCarrito): Observable<ItemCarrito> {
    this.updateCartStatus(true);
    return this.http.post<ItemCarrito>(this.itemCarritoApiUrl, item);
  }

  updateItem(id: number, item: ItemCarrito): Observable<void> {
    return this.http.put<void>(`${this.itemCarritoApiUrl}/${id}`, item);
  }
  updateCantidad(idItem: number, cantidad: number, observaciones: string): Observable<ItemCarrito> {
    // const url = `http://localhost:5000/api/ItemCarrito/updateCantidad/${idItem}?cantidad=${cantidad}&obser=${observaciones}`;
    const url = environment.apiUrl+`/ItemCarrito/updateCantidad/${idItem}?cantidad=${cantidad}&obser=${observaciones}`;
    return this.http.put<ItemCarrito>(url, { cantidad, observaciones});
  }

  deleteItem(id: number): Observable<void> {
    this.updateCartStatus(false);
    return this.http.delete<void>(`${this.itemCarritoApiUrl}/${id}`);
  }
}