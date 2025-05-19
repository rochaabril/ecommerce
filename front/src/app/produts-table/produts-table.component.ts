import { Component, OnInit, ViewChild } from '@angular/core';
import { Product } from '../model/product';
import { ProductService } from '../service/product.service';
import { ImageService } from '../service/image.service';
import { Categoria } from '../model/categoria';
import { CategoriaService } from '../service/categoria.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-produts-table',
  templateUrl: './produts-table.component.html',
  styleUrls: ['./produts-table.component.css']
})
export class ProdutsTableComponent implements OnInit {
  @ViewChild('dt') dt!: Table;
  products: Product[] = [];
  selectedProduct: Product | null = null;
  categorias: Categoria[] = []; 
  showCreate: boolean = false;
  showEdit: boolean = false;
  currentStockFilter: boolean | null = null;
  currentCategoriaFilter: any | null = null;
  newPrice: number = 0;; 
  stockOptions = [
    { label: 'Todos', value: null },
    { label: 'Con stock', value: true },
    { label: 'Sin stock', value: false },
  ];
  categoriaOptions: { label: string; value: number }[] = [];

  constructor(private productService: ProductService, private imageService: ImageService, private categoriaService:CategoriaService, private confirmationService: ConfirmationService,  // Inyecta ConfirmationService
    private messageService: MessageService, private router: Router, private authService: AuthService ) {}

    ngOnInit(): void {
      const token = this.authService.getToken(); // Recupera el token desde el almacenamiento local
    
      if (token) {
        // Si hay un token, verifica si ha expirado
        if (this.authService.isTokenExpired()) {
          this.authService.logout(); // Si el token expiró, cierra la sesión
          this.router.navigate(['/login']); // Redirige al login
        } else {
          // Si el token es válido, carga los datos
          this.getProducts();
          this.getCategories();
        }
      } else{
        this.getProducts();
        this.getCategories();
      }
     
    }
    updatePrices() {
      if (this.newPrice === 0) {
        
        return;
      }
  
      this.productService.actualizarPrecios(this.newPrice).subscribe(
        response => {
            // Mensaje de éxito
          this.getProducts();
          this.getCategories();
        },
        error => {
          // Mensaje de error
        }
      );
    }
  getCategoriaNombre(categoriaId: number): string {
    const categoria = this.categorias.find(c => c.id === categoriaId);
    return categoria ? categoria.nombre ?? 'Categoría desconocida' : 'Categoría desconocida';
  }
  getCategories() {
    this.categoriaService.getAll().subscribe((data: Categoria[]) => {
      this.categorias = data;
      // Mapea las categorías para la dropdown
      this.categoriaOptions = [
        { label: 'Todos', value: -1 }, // Usar -1 en lugar de null
        ...this.categorias.map(c => ({
          label: c.nombre ?? 'Sin nombre',
          value: c.id ?? 0
        }))
      ];
    });
  }
  // Obtiene los productos desde el servicio
  getProducts() {
    this.productService.getProducts().subscribe((data: Product[]) => {  // Asegúrate de que los datos sean del tipo Product
      this.products = data.map(product => ({
        ...product,
        imagenUrl: this.imageService.getImageUrl(this.getFileNameFromUrl(product.imagenUrl || 'default-image.png'))
      }));
    });
  }
  getFileNameFromUrl(url: string): string {
    const urlParts = url.split('/');
    return urlParts[urlParts.length - 1];  // Obtiene la última parte, que es el nombre del archivo
  }

  confirmDelete(product: Product): void {
    this.selectedProduct = product;  // Guarda el producto seleccionado
    this.confirmationService.confirm({
      message: '¿Estás seguro de que deseas eliminar este producto?',
      header: 'Confirmar eliminación',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.deleteProduct();  // Si el usuario acepta, se elimina el producto
      },
      reject: () => {
        // Acción en caso de que el usuario rechace (opcional)
      }
    });
  }
  // Filtra globalmente los productos
  onFilterGlobal(event: Event, dt: any): void {
    const inputElement = event.target as HTMLInputElement;
    const filterValue = inputElement?.value || '';
    dt.filterGlobal(filterValue, 'contains');
  }
  filterByStock(value: boolean | null): void {
    this.currentStockFilter = value;
  
    if (value === null) {
      this.dt.clear(); // Limpia todos los filtros
    } else {
      this.dt.filter(value, 'stock', 'equals'); // Aplica el filtro según el valor
    }
  }
  filterByCategoria(value: number): void {
    this.currentCategoriaFilter = value;
  
    if (value === -1) { // Si es "Todos"
      this.dt.clear(); // Limpia todos los filtros
    } else {
      this.dt.filter(value, 'categoriaId', 'equals'); // Aplica el filtro por categoría
    }
  }
  // Abre el diálogo de creación
  showCreateDialog(): void {
    this.showCreate = true;
  }

  // Abre el diálogo de edición
  showEditDialog(product: Product): void {
    this.selectedProduct = product;
    this.showEdit = true;
  }

  // Oculta el diálogo de creación
  hideCreateDialog(): void {
    this.showCreate = false;
  }

  // Oculta el diálogo de edición
  hideEditDialog(): void {
    this.showEdit = false;
  }

  // Elimina un producto de la lista
  deleteProduct(): void {
    if (this.selectedProduct && this.selectedProduct.id) {
      this.productService.deleteProduct(this.selectedProduct.id).subscribe({
        next: () => {
          this.messageService.add({severity: 'success', summary: 'Producto eliminado', detail: 'El producto se ha eliminado correctamente.'});
          this.getProducts();  // Refresca la lista
        },
        error: (error) => {
          this.messageService.add({severity: 'error', summary: 'Error', detail: 'Hubo un error al eliminar el producto.'});
          console.error('Error al eliminar producto:', error);
        },
      });
    } else {
      this.messageService.add({severity: 'warn', summary: 'Producto no válido', detail: 'El producto seleccionado no es válido.'});
    }
  }
  rejectDelete(){}
  // Refresca la lista de productos
  refreshProducts(): void {
    this.getProducts();
  }
}