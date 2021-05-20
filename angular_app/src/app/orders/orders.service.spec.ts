import {inject, TestBed} from '@angular/core/testing';
import { OrdersService } from './orders.service';
import {HttpClientTestingModule, HttpTestingController} from '@angular/common/http/testing';
import {IOrder} from './IOrder';
import {IOrderReadDto} from './IOrderReadDto';

describe('OrdersService', () => {
  let service: OrdersService;
  let httpMock: HttpTestingController;
  const fakeOrders = [
    { firstName: 'Corey' } as IOrder,
    { firstName: 'Hank' } as IOrder
  ];

  const fakeReadOnlyOrders = [
    { firstName: 'Corey' } as IOrderReadDto,
    { firstName: 'Hank' } as IOrderReadDto
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        OrdersService
      ],
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(OrdersService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch orders from DataService', () => {

    service.getOrders().subscribe(orders => {
      expect(orders.length).toBe(2);
      expect(orders).toEqual(fakeReadOnlyOrders);
    });

    service.getOrders();
    const req = httpMock.expectOne('http://localhost:5000/orders');
    expect(req.request.method).toEqual('GET');

    req.flush(fakeOrders);
  });

  it('should fetch single order ', () => {

    service.getOrder(1).subscribe(order => {
      console.log(order);
    });

    const req = httpMock.expectOne('http://localhost:5000/orders/1');
    expect(req.request.method).toEqual('GET');
  });

  it('should update order ', () => {

    service.saveOrder({id: 1, firstName: 'FOO'} as IOrder).subscribe(order => {
      console.log(order);
    });

    const req = httpMock.expectOne('http://localhost:5000/orders');
    expect(req.request.method).toEqual('PUT');
  });

  it('should create new order ', () => {

    service.createOrder({id: 1, firstName: 'FOO'} as IOrder).subscribe(order => {
      console.log(order);
    });

    const req = httpMock.expectOne('http://localhost:5000/orders');
    expect(req.request.method).toEqual('POST');
  });
});
