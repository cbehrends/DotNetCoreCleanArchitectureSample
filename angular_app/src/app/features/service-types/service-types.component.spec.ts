import {async, ComponentFixture, fakeAsync, TestBed} from '@angular/core/testing';
import { ServiceTypesComponent } from './service-types.component';
import {ServicesTypesService} from './services-types.service';
import {MAT_DIALOG_DATA, MatDialog, MatDialogModule} from '@angular/material/dialog';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {Observable, of, throwError} from 'rxjs';

import {ServiceType} from './service-type';
import {HttpResponse} from '@angular/common/http';
import {BrowserAnimationsModule, NoopAnimationsModule} from "@angular/platform-browser/animations";

const errorResp = new Error('BOOM');

describe('ServiceTypesComponent', () => {
  let component: ServiceTypesComponent;
  let fixture: ComponentFixture<ServiceTypesComponent>;

  const testVal = new Observable<ServiceType>();
  const testSuccessResponse  = new Observable<HttpResponse<any>>();
  let getServicesSpy: any;

  beforeEach(async(() => {
    const spy = jasmine.createSpyObj('ServicesTypesService', ['getServices', 'addService', 'deleteService']);

    spy.getServices.and.returnValue(testVal);
    spy.addService.and.returnValue(testVal);
    spy.deleteService.and.returnValue(testSuccessResponse);

    TestBed.configureTestingModule({
      providers: [
        { provide: ServicesTypesService, useValue: spy },
        MatDialog,
        ServiceTypesComponent,
        {provide: MAT_DIALOG_DATA, useValue: {}},
        ],
      imports: [MatDialogModule, MatSnackBarModule, BrowserAnimationsModule],
      declarations: [ ServiceTypesComponent ]
    })
    .compileComponents();

    getServicesSpy = TestBed.inject(ServicesTypesService);
    component = TestBed.inject(ServiceTypesComponent);

  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call ServicesTypesService.getServices on init', async () => {
    await component.ngOnInit();

    expect(getServicesSpy.getServices).toHaveBeenCalled();
  });

  it('should call ServicesTypesService.addService on add new service', async () => {
    const newService = {id: 1, description: 'FOO'} as ServiceType;
    component.services = new Array(newService);
    getServicesSpy.addService.and.returnValue(of(newService));
    component.addService();

    expect(getServicesSpy.addService).toHaveBeenCalled();
  });

  it('should call ServicesTypesService.addService on add new service and deal with errors',  () => {
    // const newService = {id: 1, description: 'FOO'} as IServiceType;
    // component.services = new Array(newService);

    getServicesSpy.addService.and.throwError(errorResp);

    expect(() => {
        component.addService();
      }
    ).toThrowError();

    expect(getServicesSpy.addService).toHaveBeenCalled();

  });

  it('should call ServicesTypesService.deleteService', async () => {
    component.services = new Array({id: 1, description: 'FOOO'} as ServiceType);
    component.deleteService(1);
    // expect(component.deleteService(1)).toBeTruthy();
    expect(getServicesSpy.deleteService).toHaveBeenCalled();
  });

  it('should call ServicesTypesService.deleteService on service delete and deal with errors', () => {

    getServicesSpy.deleteService.and.throwError(errorResp);

    expect(() => {
        component.deleteService(1);
      }
    ).toThrowError(errorResp.message);
  });

});
