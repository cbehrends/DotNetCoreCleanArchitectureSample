import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { OrdersComponent } from './orders.component';
import {MatDialog, MatDialogModule} from '@angular/material/dialog';
import {OverlayModule} from '@angular/cdk/overlay';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {OrdersService} from './orders.service';
import {of} from 'rxjs';
import {Order} from './Order';
import {Title} from '@angular/platform-browser';
import {ServicesTypesService} from '../service-types/services-types.service';

let orderServiceSpy: any;
let titleServiceSpy: any;
let serviceTypesServiceSpy: any;
const testData = [{id: 1, firstName: 'Foo'} as Order, {id: 2, firstName: 'Bar'} as Order];

const testVal = of(testData[0]);
const test = of(testData);

describe('OrdersComponent', () => {
  let ordersComponent: OrdersComponent;
  let fixture: ComponentFixture<OrdersComponent>;

  beforeEach(async(() => {

    orderServiceSpy = jasmine.createSpyObj('OrdersService', ['getOrders', 'saveOrder', 'handleError']);
    titleServiceSpy = jasmine.createSpyObj('Title', ['setTitle']);
    serviceTypesServiceSpy = jasmine.createSpyObj('ServiceTypes', []);
    orderServiceSpy.getOrders.and.returnValue(test);
    orderServiceSpy.saveOrder.and.returnValue(testVal);

    TestBed.configureTestingModule({
      declarations: [ OrdersComponent ],
      providers: [
        { provide: OrdersService, useValue: orderServiceSpy },
        { provide: ServicesTypesService, useValue: serviceTypesServiceSpy },
        { provide: Title, useValue: titleServiceSpy },
        MatDialog
      ],
      imports: [OverlayModule, MatDialogModule, MatSnackBarModule]
    })
    .compileComponents();

    ordersComponent = TestBed.inject(OrdersComponent);
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdersComponent);
    ordersComponent = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create new Orders component instance and call getOrders on ngInit', () => {
    expect(ordersComponent).toBeTruthy();
    expect(orderServiceSpy.getOrders).toHaveBeenCalled();
  });

  it('should getOrders', () => {
    ordersComponent.getOrders();
    expect(orderServiceSpy.getOrders).toHaveBeenCalled();
  });

  it('should handle error in getOrders', () => {
    orderServiceSpy.getOrders.and.throwError('BOOM');
    expect( () => {
      ordersComponent.getOrders();
    }).toThrowError('BOOM');

  });

  it('should call saveOrder on order service', () => {
    ordersComponent.saveOrder({id: 1, firstName: 'Foo'} as Order);
    expect(orderServiceSpy.saveOrder).toHaveBeenCalled();
  });

  it('should call createOrder on order service', () => {
    ordersComponent.saveOrder({id: 1, firstName: 'Foo'} as Order);
    expect(orderServiceSpy.saveOrder).toHaveBeenCalled();
  });

});
