import { Component, OnDestroy, OnInit } from '@angular/core';
import { Product } from '../model/product';
import { ProductService } from '../service/product.service';
import { CartService } from '../service/cart.service';
import { Subscription, switchMap } from 'rxjs';
import { ItemCarrito } from '../model/itemCarrito';
import { AuthService } from '../service/auth.service';
import { CategoriaService } from '../service/categoria.service';
import { Categoria } from '../model/categoria';
import { EventService } from '../service/event.service';
import { ImageService } from '../service/image.service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit, OnDestroy{
  products: Product[] = [];
  categoriaList: any;
  filteredProducts: Product[] = [];
  searchText: string = '';
  showOutOfStockOnly: boolean = false;
  selectedCategory: Categoria | null = null;
  categories: Categoria[] = [];// Lista de categorías únicas
  usuarioId: number | null = null;
  idcarrito?: number;
  novedadesSubscription: Subscription | undefined;
  imageViewerVisible: boolean = false;
 selectedImageUrl: string | null = null;
 

  constructor(private productService: ProductService, private cartService: CartService, private authService: AuthService, private categoriaService: CategoriaService, private eventService: EventService, private imageService: ImageService,private router: Router,  private messageService: MessageService) { }

  ngOnInit() {
    const token = this.authService.getToken();
    if(token){

      if (this.authService.isTokenExpired()) {
        this.authService.logout();
        this.router.navigate(['/login']); // Redirige al login
      }else{
        this.getProducts();
        this.getCategorias();
        this.obtenerID();
        this.novedadesSubscription = this.eventService.novedades$.subscribe(() => {
          this.fetchLatestProducts();
        });
      }
    }else{

      this.getProducts();
      this.getCategorias();
      this.obtenerID();
      this.novedadesSubscription = this.eventService.novedades$.subscribe(() => {
        this.fetchLatestProducts();
      });
    }
  }
  ngOnDestroy() {
    // Cancelar la suscripción para evitar fugas de memoria
    if (this.novedadesSubscription) {
      this.novedadesSubscription.unsubscribe();
    }
  }
  obtenerID(){
    this.usuarioId = this.authService.getUserId();
    
    if (!this.usuarioId) {
      
    }
  }
  addToCart(product: Product): void {
    const token = this.authService.getToken();
    if(!token){
      this.router.navigate(['/login']); 
    }
    this.obtenerID();
    if (!product.id || !this.usuarioId) {
      return;
    }
  
    // Obtener el carrito del usuario
    this.cartService.getByIdUser(this.usuarioId).subscribe((data: any) => {
      this.idcarrito = data.id;
      if (!this.idcarrito) {
        return;
      }
    
      // Verificar si el producto ya está en el carrito
      this.cartService.getItemByIdCarrito(this.idcarrito).subscribe((items: ItemCarrito[]) => {
        const existingItem = items.find(item => item.productoId === product.id);
  
        if (existingItem) {
         
          // Si el producto ya está en el carrito, actualizar la cantidad
         
           // Sumar la cantidad al ítem existente
          existingItem.observaciones = product.observaciones || 'ninguna';
         
          // Actualizar el ítem en el carrito
          if(!existingItem.id|| !product.quantity){
           return
          }
         
          this.cartService.updateCantidad(existingItem.id,product.quantity, existingItem.observaciones).subscribe(() => {
            this.router.navigate(['/products']);
            this.messageService.add({
              severity: 'success',
              summary: 'Éxito',
              detail: 'Se ha modificado el producto del carrito'
            });
            window.location.reload();
          });
        } else {
          // Si el producto no está en el carrito, agregarlo
          const itemCarrito: ItemCarrito = {
            carritoId: this.idcarrito,
            productoId: product.id,
            cantidad: product.quantity || 1,
            observaciones: product.observaciones
          };
  
          // Crear el nuevo ítem en el carrito
          this.cartService.createItem(itemCarrito).subscribe(() => {
            console.log('Producto agregado al carrito');
            this.messageService.add({
              severity: 'success',
              summary: 'Éxito',
              detail: 'Se ha agregado el producto al carrito'
            });
            window.location.reload();
            this.router.navigate(['/products']);
          });
        }
      });
    });
  }
  getCategoryName(categoriaId: number): string {
    const category = this.categories.find(cat => cat.id === categoriaId);
    return category?.nombre ?? 'Categoría no encontrada';
  }
  getProducts() {
    this.productService.getProducts().subscribe((data: Product[]) => {
      this.products = data.map(product => ({
        ...product,
        imagenUrl: this.imageService.getImageUrl(this.getFileNameFromUrl(product.imagenUrl || 'default-image.png')) // Si imagenUrl es undefined, asigna 'default-image.png'
      }));
      this.filteredProducts = [...this.products];
    });
  }
  getFileNameFromUrl(url: string): string {
    const urlParts = url.split('/');
    return urlParts[urlParts.length - 1];  // Obtiene la última parte, que es el nombre del archivo
  }

 showImageViewer(imageUrl: string): void {
  this.selectedImageUrl = imageUrl;
  this.imageViewerVisible = true;
}
  // Filtrar productos por categoría y texto de búsqueda
  filterProducts() {
    console.log(this.products.map(product => ({ nombre: product.nombre, stock: product.stock })));
    this.filteredProducts = this.products.filter(product => {
      const matchesSearchText = this.searchText === '' || product.nombre?.toLowerCase().includes(this.searchText.toLowerCase());
      const matchesCategory = !this.selectedCategory || product.categoriaId === this.selectedCategory.id;
      const matchesOutOfStock = !this.showOutOfStockOnly || product.stock == false;
      return matchesSearchText && matchesCategory && matchesOutOfStock;
    });
  }

  fetchLatestProducts() {
    this.productService.getLatest10().subscribe((productos) => {
      this.filteredProducts = productos;
      console.log('Últimos productos:', this.filteredProducts);
    });
  }
  

  // Método para manejar el cambio en el campo de búsqueda
  onSearchChange(event: any) {
    this.searchText = event.target.value;
    this.filterProducts();
  }

  getCategorias() {
    this.categoriaService.getAll().subscribe((data: Categoria[]) => {
      this.categories = data;
    });
  }
  toggleOutOfStockFilter() {
    this.showOutOfStockOnly = !this.showOutOfStockOnly;
    this.filterProducts();
  }
}
