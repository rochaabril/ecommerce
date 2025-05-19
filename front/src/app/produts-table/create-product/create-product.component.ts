import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Categoria } from 'src/app/model/categoria';
import { Product } from 'src/app/model/product';
import { CategoriaService } from 'src/app/service/categoria.service';
import { ProductService } from 'src/app/service/product.service';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css']
})
export class CreateProductComponent {
  @Output() productCreated = new EventEmitter<Product>();
  productForm: FormGroup;
  categoryForm: FormGroup;
  selectedFile?: File;
  categorias: any[] = [];
  isCategoryModalOpen = false;
  
  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private categoriaService: CategoriaService,
    private messageService: MessageService
  ) {
    this.productForm = this.fb.group({
      nombre: ['', Validators.required],
      precio: [null, [Validators.required, Validators.min(0)]],
      stock: [true, Validators.required],
      cantidadMinima: [null, [Validators.required, Validators.min(1)]],
      categoriaId: [null, Validators.required],
    });

    this.categoryForm = this.fb.group({
      nombre: ['', Validators.required],
    });

    this.getCategorias();
  }

  onImageSelected(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  getCategorias(): void {
    this.categoriaService.getAll().subscribe({
      next: (data) => (this.categorias = data),
      error: (err) => console.error('Error al obtener categorías:', err),
    });
  }

  createProduct(): void {
    if (this.productForm.invalid) return;

    console.log('PRODUCto', this.productForm.value)
    

    const newProduct: Product = this.productForm.value;
    const fileToSend: File = this.selectedFile || new File([], '');

    this.productService.createProduct(newProduct, fileToSend).subscribe({
      next: (createdProduct) => {
        this.productCreated.emit(createdProduct);
        this.productForm.reset();
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Se ha creado el producto exitosamente'
        });
      },
      error: (err) =>  this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Error al crear el producto: '+ err.message,
      }),
    });
  }

  openCategoryModal(): void {
    this.isCategoryModalOpen = true;
  }

  closeCategoryModal(): void {
    this.isCategoryModalOpen = false;
  }

  createCategory(): void {
    if (this.categoryForm.invalid) return;

    this.categoriaService.create(this.categoryForm.value).subscribe({
      next: (createdCategory) => {
        this.categorias.push(createdCategory);
        this.closeCategoryModal();
        this.categoryForm.reset();
        this.getCategorias();
      },
      error: (err) => 
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al crear la categoria: '+ err.message,
        }),
     
    });
  }
}