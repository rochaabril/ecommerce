import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Product } from 'src/app/model/product';
import { CategoriaService } from 'src/app/service/categoria.service';
import { ProductService } from 'src/app/service/product.service';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent {
  @Input() product: Product | null = null; // Recibe el producto a editar desde el componente padre
  @Output() productUpdated = new EventEmitter<Product>(); // Evento para notificar al componente padre cuando se actualice el producto
  productForm: FormGroup;
  selectedFile?: File;
  categorias: any[] = [];

  constructor(private fb: FormBuilder, private productService: ProductService, private categoriaService: CategoriaService) {
    this.productForm = this.fb.group({
      nombre: ['', [Validators.required]],
      precio: [0, [Validators.required, Validators.min(0)]],
      stock: ['false', Validators.required],
      cantidadMinima: [0, [Validators.required, Validators.min(1)]],
      categoriaId: [0, Validators.required],
    });
  }
  ngOnInit(): void {
    // Obtener las categorías desde el servicio
    this.categoriaService.getAll().subscribe(categorias => {
      this.categorias = categorias;
    });
  }
  ngOnChanges(): void {
    if (this.product && this.product.id) {
      // Para el stock, convertimos 'true'/'false' a 'Sí'/'No'
      this.productForm.patchValue({
        ...this.product,
        stock: this.product.stock ? 'true' : 'false'
      });
    }
  }

  onImageSelected(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      // Verifica que el archivo se está seleccionando
    }
  }

  saveProduct(): void {
    if (!this.productForm.valid) {
      console.error('Formulario inválido');
      return;
    }
  
    const updatedProduct: Product = this.productForm.value;
  
    if (this.product?.id) {
      updatedProduct.id = this.product.id; // Mantén el ID del producto para la actualización
  
      this.productService.updateProduct(updatedProduct, this.selectedFile).subscribe({
        next: (updatedProduct) => {
          console.log('Producto actualizado');
          this.productUpdated.emit(updatedProduct); // Emitimos el producto actualizado al componente padre
        },
        error: (error) => {
          console.error('Error al actualizar producto:', error);
        }
      });
    } else {
      console.error('Producto no encontrado para actualizar');
    }
  }
}
