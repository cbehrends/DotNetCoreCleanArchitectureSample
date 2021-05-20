import { BrowserModule } from '@angular/platform-browser';
import {APP_INITIALIZER, NgModule} from '@angular/core';
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
import {EventTypes, OidcConfigService, PublicEventsService} from 'angular-auth-oidc-client';
import {environment} from '../environments/environment';
import {LogLevel} from 'codelyzer';
import {filter} from "rxjs/operators";

export function configureAuth(oidcConfigService: OidcConfigService) {
  return () =>
    oidcConfigService.withConfig({
      stsServer: 'http://localhost:8080/auth/realms/dev',
      redirectUrl: window.location.origin,
      postLogoutRedirectUri: window.location.origin,
      clientId: 'angularClient',
      scope: 'openid profile email',
      responseType: 'code',
      silentRenew: true,
      silentRenewUrl: `${window.location.origin}/silent-renew.html`,
      renewTimeBeforeTokenExpiresInSeconds: 10,
      logLevel: environment.production ? LogLevel.None : LogLevel.Debug,
  });
}

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
  providers: [
    OidcConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: configureAuth,
      deps: [OidcConfigService],
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(private readonly eventService: PublicEventsService) {
    this.eventService
      .registerForEvents()
      .pipe(filter((notification) => notification.type === EventTypes.ConfigLoaded))
      .subscribe((config) => console.log('ConfigLoaded', config));
  }
}
