import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { CarritoComponent } from './carrito/carrito.component';
import { LoginComponent } from './login/login.component';
import { ProdutsTableComponent } from './produts-table/produts-table.component';
import { UserComponent } from './user/user.component';
import { PasswordResetComponent } from './password-reset/password-reset.component';
import { RegisterComponent } from './login/register/register.component';
import { NovedadesComponent } from './novedades/novedades.component';
import { NewPassComponent } from './password-reset/new-pass/new-pass.component';


const routes: Routes = [
  { path: 'products', component: ProductsComponent },
  { path: 'novedades', component: NovedadesComponent },
  { path: 'carrito', component: CarritoComponent },
  { path:'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'table', component: ProdutsTableComponent},
  { path: 'user', component: UserComponent},
  { path: 'reset', component: PasswordResetComponent},
  { path: 'reset/reset-password', component: NewPassComponent},
  { path: '', redirectTo: '/products', pathMatch: 'full' }, 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
