import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RoutingModule } from './routing/routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { HttpClientModule } from '@angular/common/http';
import { LayoutComponent } from './layout/layout.component';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './navigation/header/header.component';
import { ServiceTypesComponent } from './service-types/service-types.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import {ReactiveFormsModule} from '@angular/forms';
import {SidenavListComponent} from './navigation/sidenav-list/sidenav-list.component';
import {OrdersComponent} from './orders/orders.component';
import {OrderEditorComponent} from './orders/order-editor/order-editor.component';

@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    HomeComponent,
    HeaderComponent,
    SidenavListComponent,
    OrdersComponent,
    OrderEditorComponent,
    ServiceTypesComponent,
    ConfirmDialogComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    FlexLayoutModule,
    RoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
