import { PreventAuthGuard } from './guards/prevent-auth.guard';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  { path: "", component: HomeComponent },
  { path: "login", component: LoginComponent , canActivate:[PreventAuthGuard] },
  {path:"register",component:RegistrationComponent, canActivate:[PreventAuthGuard]}
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
