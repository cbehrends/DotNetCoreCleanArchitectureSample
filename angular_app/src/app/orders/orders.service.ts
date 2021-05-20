import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {IOrder} from './IOrder';
import {tap} from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';
import {IOrderReadDto} from './IOrderReadDto';

@Injectable({
  providedIn: 'root'
})

export class OrdersService {
  private ordersUrl = 'http://localhost:5000';
  constructor(private http: HttpClient) { }

  getOrder(id: number): any{
    const url = this.ordersUrl + '/orders/' + id;
    return this.http.get(url);
  }

  getOrders(): Observable<IOrderReadDto[]> {
    const url = this.ordersUrl + '/orders';

    return this.http.get<IOrderReadDto[]>(url);
  }

  createOrder(order: IOrder){
    const url = this.ordersUrl + '/orders';
    return this.http.post(url, order);
  }

  saveOrder(order: IOrder): Observable<any>{
    const url = this.ordersUrl + '/orders';
    return this.http.put(url, order);
  }

  approvePayment(orderId: number){
    const url = this.ordersUrl + '/orders/approve_payment/' + orderId;
    return this.http.post(url, null)
      .subscribe((r) => r);
  }
}
