import { Component, OnInit } from '@angular/core';
import { Product } from '../model/product';
import { CartService } from '../service/cart.service';
import { Router } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { map, Observable } from 'rxjs';
import { ImageService } from '../service/image.service';
import { ProductService } from '../service/product.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-carrito',
  templateUrl: './carrito.component.html',
  styleUrls: ['./carrito.component.css']
})
export class CarritoComponent implements OnInit  {
updateQuantity(_t5: any) {
throw new Error('Method not implemented.');
}
  cartProducts: Product[] = [];
  totalPrice: number = 0;
  usuarioId: number | null = null;
  idcarrito?: number;
  itemslist: any;
  constructor(private cartService: CartService, private router: Router, private authService: AuthService, private imageService: ImageService, private productService:ProductService,  private messageService: MessageService) { }

  ngOnInit(): void {
    
    this.obtenerIdUser();
    this.getIdCarrito();
   
  }
  obtenerIdUser(){
    this.usuarioId = this.authService.getUserId();
   
  
  }
  getIdCarrito(){
    if ( !this.usuarioId) {
     
      return;
    }
    this.cartService.getByIdUser(this.usuarioId).subscribe((data: any)=> {
      this.idcarrito = data.id;
      this.getCarrito();
    })
  }
  getCarrito(): void {
    if (!this.idcarrito) {
      return;
    }
    this.cartService.getItemByIdCarrito(this.idcarrito).subscribe((data: any) => {
      this.itemslist = [];
      console.log('DATOS:', data)
  
      data.forEach((item: any) => {
        // Llamar a getByIdproduct para obtener más detalles del producto
        this.getByIdproduct(item.producto.id).subscribe((imagenUrl: string) => {
          this.itemslist.push({
            id: item.id,
            carritoId: item.carritoId,
            quantity: item.cantidad,
            observaciones:item.observaciones,
            product: {
              ...item.producto,
              imagenUrl: this.imageService.getImageUrl(this.getFileNameFromUrl(imagenUrl || 'default-image.png'))  // Aquí agregamos la URL de la imagen
            },
          });
  
          // Recalcular el precio total después de agregar el nuevo item
          this.calculateTotalPrice();
        });
      });
  
      console.log('Ítems:', this.itemslist);
    });
  }
  getByIdproduct(id: number): Observable<string> {
    return this.productService.getByIdProducts(id).pipe(
      map((data: Product) => {
        return data.imagenUrl || '';  // Si imagenUrl es undefined, devuelve una cadena vacía
      })
    );
  }
  getFileNameFromUrl(url: string): string {
    const urlParts = url.split('/');
    return urlParts[urlParts.length - 1];  // Obtiene la última parte, que es el nombre del archivo
  }
  loadCartProducts(): void { // Copia para detectar cambios
    this.calculateTotalPrice();
  }
  increaseQuantity(item: any): void {
    item.quantity++;  // Aumentamos la cantidad localmente
  
    // Verificamos que la cantidad nunca sea menor a 1
    if (item.quantity < 1) {
      item.quantity = 1;
    }
  
    // Actualizamos la cantidad en el backend
    this.cartService.updateCantidad(item.id, item.quantity, item.observaciones).subscribe({
      next: () => {
        this.calculateTotalPrice();  // Recalcular el precio total después de la actualización
        console.log('Cantidad actualizada');
      },
      error: (err) => {
        console.error('Error al actualizar la cantidad:', err);
      }
    });
  }
  
  decreaseQuantity(item: any): void {
    if (item.quantity > 1) {
      item.quantity--;  // Disminuimos la cantidad localmente
  
      // Actualizamos la cantidad en el backend
      this.cartService.updateCantidad(item.id, item.quantity,item.observaciones).subscribe({
        next: () => {
          this.calculateTotalPrice();  // Recalcular el precio total después de la actualización
          console.log('Cantidad actualizada');
        },
        error: (err) => {
          console.error('Error al actualizar la cantidad:', err);
        }
      });
    }
  }
  removeProduct(item: any): void {
    // this.itemslist = this.itemslist.filter((i: any) => i !== item);
    this.cartService.deleteItem(item.id).subscribe({
      next: () => {
        // Si la eliminación fue exitosa, recarga la página
        this.getCarrito();
      },
      error: (err) => {
        // Manejo de errores en caso de que la eliminación falle
        console.error('Error al eliminar el producto:', err);
      }
    });
    this.calculateTotalPrice();
  }
  calculateTotalPrice(): void {
    this.totalPrice = this.itemslist.reduce(
      (acc: number, item: any) => acc + (item.product.precio * item.quantity),
      0
    );
  }
  
  

  continueShopping(): void {
    this.router.navigate(['/products']); // Ajusta la ruta según tu estructura
  }

  finalizePurchase(): void {

    this.cartProducts = [];
    this.totalPrice = 0;
    if(!this.idcarrito){return}
    this.cartService.finalizeCarrito(this.idcarrito).subscribe({
    });
    this.messageService.add({
      severity: 'success',
      summary: 'Exito',
      detail: 'Carrito finalizado con exito: ',
    }),
    this.router.navigate(['/products']);
  }

}
