import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { ProductsComponent } from './products/products.component';
import { DataViewModule } from 'primeng/dataview';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { ProductService } from './service/product.service';
import { TagModule } from 'primeng/tag';
import { RatingModule } from 'primeng/rating';
import { DropdownModule } from 'primeng/dropdown';
import { CarritoComponent } from './carrito/carrito.component';

import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './service/auth.service';
import { CartService } from './service/cart.service';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { PanelModule } from 'primeng/panel';
import { CheckboxModule } from 'primeng/checkbox';
import { ProdutsTableComponent } from './produts-table/produts-table.component'
import { TableModule } from 'primeng/table'
import { DialogModule } from 'primeng/dialog'
import { ToastModule } from 'primeng/toast'
import { ToolbarModule } from 'primeng/toolbar'
import { FileUploadModule } from 'primeng/fileupload'
import { ConfirmDialogModule } from 'primeng/confirmdialog'
import { MessageService,ConfirmationService } from 'primeng/api';
import { UserComponent } from './user/user.component';
import { PasswordResetComponent } from './password-reset/password-reset.component';
import { CreateProductComponent } from './produts-table/create-product/create-product.component';
import { EditProductComponent } from './produts-table/edit-product/edit-product.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { RegisterComponent } from './login/register/register.component';
import { NovedadesComponent } from './novedades/novedades.component';
import { MenuModule } from 'primeng/menu';
import { NewPassComponent } from './password-reset/new-pass/new-pass.component';
import { BrowserModule } from '@angular/platform-browser';
@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    ProductsComponent,
    CarritoComponent,
    LoginComponent,
    ProdutsTableComponent,
    UserComponent,
    PasswordResetComponent,
    CreateProductComponent,
    EditProductComponent,
    RegisterComponent,
    NovedadesComponent,
    NewPassComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DataViewModule,  
    CardModule,       
    ButtonModule,
    TagModule,
    RatingModule,
    FormsModule,
    DropdownModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    PanelModule,
    CheckboxModule,
    TableModule,
    DialogModule,
    ToastModule,
    ToolbarModule,
    FileUploadModule,
    ConfirmDialogModule,
    MenuModule,
  ],
  providers: [ProductService,AuthService,CartService,MessageService,ConfirmationService,   {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,
  },],
  bootstrap: [AppComponent]
})
export class AppModule { }
