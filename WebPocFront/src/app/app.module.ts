import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConsultaComponent } from './consulta/consulta.component';

import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { AuthService } from './auth.service';

@NgModule({
  declarations: [AppComponent, ConsultaComponent, HomeComponent, LoginComponent, AdminComponent],
  imports: [BrowserModule, AppRoutingModule,
    RouterModule.forRoot([
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path: '',
        component: HomeComponent
      }
    ])],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule {}
