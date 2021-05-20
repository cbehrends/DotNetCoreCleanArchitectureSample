import {Inject, Injectable} from '@angular/core';
import {Observable, throwError} from 'rxjs';
import {IServiceType} from './service-type';
import {catchError, map, tap} from 'rxjs/operators';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class ServicesTypesService {
  private serviceTypeUrl = 'http://localhost:5000';
  constructor(private http: HttpClient) { }

  getServices(): Observable<IServiceType[]> {
    const url = this.serviceTypeUrl + '/services';

    return this.http
      .get<IServiceType[]>(url)
      .pipe(
        catchError(this.handleError)
      );
  }

  addService(description: string, cost: number): any{
    const url = this.serviceTypeUrl + '/services';
    return this.http.post(url, {description, cost})
      .pipe(
        map(data => data),
        catchError(this.handleError)
      );
  }

  deleteService(id: number): any{
    const url = this.serviceTypeUrl + '/services/' + id;
    return this.http.delete(url)
      .pipe(
        map(data => data),
        catchError(this.handleError)
      );
  }

  private handleError(res: HttpErrorResponse) {
    console.error(res.error);
    return throwError(new Error(res.error));
  }
}
