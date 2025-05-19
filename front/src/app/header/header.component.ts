import { Component, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { AuthService } from '../service/auth.service';
import { ProductService } from '../service/product.service';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import { EventService } from '../service/event.service';
import { Router } from '@angular/router';
import { CategoriaService } from '../service/categoria.service';
import { CartService } from '../service/cart.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
 

  menuActive = false;
  isSubMenuOpen = false;
  
  isAdmin: boolean = false;  
  hasItemsInCart = false;
  
  constructor(private authService: AuthService, private productService: ProductService, private eventSertvice: EventService,private router: Router, private categoriaService: CategoriaService, private cartService: CartService) {}
  ngOnInit() {
    // Verifica si el usuario está logueado
     // Verifica si el usuario tiene rol admin
    const usuarioId = this.authService.getUserId();
    if(!usuarioId){
      return
    }
    this.cartService.getByIdUser(usuarioId).subscribe((data: any) =>{
        const carritoId = data.id;
        if (carritoId) {
          this.cartService.getItemByIdCarrito(carritoId).subscribe((items: any[]) => {
            this.hasItemsInCart = items.length > 0;  // Si el carrito tiene productos, el punto rojo aparece
          });
        }
    });
    this.cartService.cartItems$.subscribe(isNotEmpty => {
      this.hasItemsInCart = isNotEmpty;  // Actualiza el estado dinámicamente
    });
      
  }
  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    const clickedInsideMenu = target.closest('nav');

    if (!clickedInsideMenu) {
      this.menuActive = false;
    }
  }

  get isLoggedIn(): boolean {
    this.isAdmin = this.authService.getUserRole() === 'Admin';
    
    return this.authService.isLoggedIn();
  }
  toggleMenu() {
    this.menuActive = !this.menuActive;
  }
  toggleSubMenu() {
    this.isSubMenuOpen = !this.isSubMenuOpen; // Alterna la visibilidad del submenú
  }
  excel() {
    this.productService.exportExcel().subscribe((response: Blob) => {
      // Usamos FileSaver.js para descargar el archivo
      saveAs(response, 'Productos.xlsx');
    });

  }
  
  getLatestProducts() {
    this.eventSertvice.triggerNovedades();
  }
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  getCategorias(){
    this.categoriaService.getAll().subscribe({})
  }
}
