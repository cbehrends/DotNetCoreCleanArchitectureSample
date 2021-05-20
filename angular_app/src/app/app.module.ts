import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RoutingModule } from './routing/routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { HttpClientModule } from '@angular/common/http';
import { LayoutComponent } from './layout/layout.component';
import { HomeComponent } from './features/home/home.component';
import { HeaderComponent } from './layout/navigation/header/header.component';
import { ServiceTypesComponent } from './features/service-types/service-types.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import {ReactiveFormsModule} from '@angular/forms';
import {SidenavListComponent} from './layout/navigation/sidenav-list/sidenav-list.component';
import {OrdersComponent} from './features/orders/orders.component';
import {OrderEditorComponent} from './features/orders/order-editor/order-editor.component';

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
