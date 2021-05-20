import { TestBed } from '@angular/core/testing';
import { ServicesTypesService } from './services-types.service';
import {MatDialog} from '@angular/material/dialog';
import {HttpClientTestingModule, HttpTestingController} from '@angular/common/http/testing';
import {ServiceType} from './service-type';
import {Observable} from 'rxjs';
import {HttpErrorResponse, HttpResponse} from '@angular/common/http';
const testUrl = 'http://localhost:5000/services';
const errorResp = new HttpErrorResponse({status: 500, statusText: 'BOOM', url: testUrl});

describe('ServicesTypesService', () => {
  let service: ServicesTypesService;
  let httpMock: HttpTestingController;
  const fakeServices = [
    {id: 1, description: 'Test'} as ServiceType,
    {id: 2, description: 'Test 2'} as ServiceType
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MatDialog],
      imports: [HttpClientTestingModule]
    });
    httpMock = TestBed.inject(HttpTestingController);
    service = TestBed.inject(ServicesTypesService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create instance of ServiceTypesService', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch services', () => {

    service.getServices().subscribe(orders => {
      expect(orders.length).toBe(2);
      expect(orders).toEqual(fakeServices);
    });

    service.getServices();
    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('GET');

    req.flush(fakeServices);
  });

  it('should handle error when fetching services', () => {

    service.getServices()
      .subscribe(
      data => fail(errorResp),
      (error: Error) => {
        expect(error.message).toBeTruthy();
      }
    );

    const req = httpMock.expectOne(testUrl);

    // Respond with mock error
    req.flush(errorResp, { status: errorResp.status, statusText: errorResp.statusText});
  });

  it('should add service', () => {
    const dummyService = {
      id: 1,
      description: 'foo',
      cost: 100
    } as ServiceType;

    service.addService('FOO', 100).subscribe(retVal => {
      expect(retVal).toBe(dummyService);
    });

    const req = httpMock.expectOne('http://localhost:5000/services');
    expect(req.request.method).toEqual('POST');

    req.flush(dummyService);
  });

  it('should handle errors when adding new service', () => {

    service.addService('FOO', 100).subscribe(
      data => fail(errorResp),
      (error: Error) => {
        expect(error.message).toBeTruthy();
      }
    );

    const req = httpMock.expectOne('http://localhost:5000/services');
    expect(req.request.method).toEqual('POST');

    req.flush(errorResp, { status: errorResp.status, statusText: errorResp.statusText});
  });

  it('should delete service', () => {

    service.deleteService(1).subscribe(retVal => {
      expect(retVal).toBeTruthy();
    });

    const req = httpMock.expectOne('http://localhost:5000/services/1');
    expect(req.request.method).toEqual('DELETE');

    req.flush(new HttpResponse({status: 200}));
  });

  it('should handle error from delete of service', () => {

    service.deleteService(1).subscribe(
      data => fail(errorResp),
      (error: Error) => {
        expect(error.message).toBeTruthy();
      }
    );

    const req = httpMock.expectOne('http://localhost:5000/services/1');
    expect(req.request.method).toEqual('DELETE');

    req.flush(errorResp, { status: errorResp.status, statusText: errorResp.statusText});

  });
});
