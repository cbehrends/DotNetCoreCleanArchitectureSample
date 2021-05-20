import { Component, OnInit } from '@angular/core';
import {OrdersService} from './orders.service';
import {IOrder} from './IOrder';
import {throwError} from 'rxjs';
import {MatDialog} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {catchError} from 'rxjs/operators';
import {Title} from '@angular/platform-browser';
import {ServicesTypesService} from '../service-types/services-types.service';
import {IServiceType} from '../service-types/service-type';
import {IOrderReadDto} from './IOrderReadDto';
import {OrderEditDialogModel, OrderEditorComponent} from './order-editor/order-editor.component';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: IOrderReadDto[];
  servicesList: IServiceType[];
  errorReceived: boolean;
  addingNew: boolean;
  constructor(private ordersService: OrdersService,
              private servicesTypesService: ServicesTypesService,
              private dialog: MatDialog,
              private titleService: Title,
              private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.addingNew = false;
    this.errorReceived = false;
    this.titleService.setTitle('Orders');
    this.getOrders();
    this.getServices();

  }

  getOrders() {
    this.errorReceived = false;
    this.ordersService.getOrders()
      .pipe(catchError(this.handleError))
      .subscribe(orders => {
        this.orders = orders;
      });
  }

  getServices() {
    this.errorReceived = false;
    this.servicesTypesService.getServices()
      .pipe(catchError(this.handleError))
      .subscribe(services => {
        this.servicesList = services;
      });
  }

  newOrder(){
    this.addingNew = true;
    this.newDialog({firstName: '', totalAmount: 0, amountDue: 0} as IOrder);
  }

  saveOrder(order: IOrder){
    if (this.addingNew === true){
      return;
    }

    this.ordersService.saveOrder(order)
      .pipe(
        catchError(this.handleError),
        )
      .subscribe((updatedOrder: IOrder) => {
        this.orders.push( {
          id: updatedOrder.id,
          firstName: updatedOrder.firstName,
          servicesRenderedCount: updatedOrder.servicesRendered.length
        } as IOrderReadDto);
        this.addingNew = false;
        this.errorReceived = false;
      });
  }

  newDialog(order: IOrder){

    const dialogData = new OrderEditDialogModel('New Order', order, this.servicesList);
    const dialogRef = this.dialog.open(OrderEditorComponent, {
      maxWidth: '400px',
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {

      if (dialogResult){
        this.ordersService.createOrder(dialogResult)
          .pipe(catchError((err) => this.handleError(err)))
          .subscribe((updatedOrder: IOrder) => {
            this.orders.push( {
              id: updatedOrder.id,
              firstName: updatedOrder.firstName,
              amountDue: updatedOrder.amountDue,
              servicesRenderedCount: updatedOrder.servicesRendered.length
            } as IOrderReadDto);
          });
      }
    });
  }

  editDialog(orderId: number): void {
    let editOrder: IOrder;
    this.ordersService.getOrder(orderId)
      .subscribe(
        order => {
          editOrder = order;
          const dialogData = new OrderEditDialogModel('Edit Order', editOrder, this.servicesList);
          const dialogRef = this.dialog.open(OrderEditorComponent, {
            maxWidth: '500px',
            data: dialogData
          });

          dialogRef.afterClosed().subscribe(dialogResult => {

            if (dialogResult){
              this.ordersService.saveOrder(dialogResult)
                .pipe(catchError((err) => this.handleError(err)))
                .subscribe((updatedOrder: IOrder) => {
                  this.orders[(this.orders.indexOf(this.orders.find(s => s.id === updatedOrder.id)))] =  {
                    id: updatedOrder.id,
                    firstName: updatedOrder.firstName,
                    amountDue: updatedOrder.amountDue,
                    servicesRenderedCount: updatedOrder.servicesRendered.length
                  } as IOrderReadDto;
                });
            }
          });
        }
      );
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 5000, // Auto confirm if open more than 5 seconds
      verticalPosition: 'top'
    });
  }

  public handleError(error: any) {
      this.errorReceived = true;
      let errors = '';
      for (const fieldName in error.error) {
        if (error.error.hasOwnProperty(fieldName)) {
          errors += error.error[fieldName] + '\n';
        }
      }
      this.openSnackBar(errors, 'Confirm');
      return throwError(error);
  }
}
