import {Component, Inject, OnInit, Output} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Order} from '../Order';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ServiceType} from '../../service-types/service-type';
import {RenderedService} from '../RenderedService';
import {OrdersService} from '../orders.service';

@Component({
  selector: 'app-claim-editor',
  templateUrl: './order-editor.component.html',
  styleUrls: ['./order-editor.component.css']
})
export class OrderEditorComponent implements OnInit {
  public editForm: FormGroup;
  title: string;
  order: Order;
  serviceList: ServiceType[];

  constructor(public dialogRef: MatDialogRef<OrderEditorComponent>,
              private formBuilder: FormBuilder,
              private ordersService: OrdersService,
              @Inject(MAT_DIALOG_DATA) public data: OrderEditDialogModel) {

    this.title = data.title;
    this.order = data.order;
    this.serviceList = data.serviceList;
    this.editForm = this.formBuilder.group({
      firstName: '',
      amountDue: 0.00,
      totalAmount: 0.00
    });
  }

  addRenderedService(service: ServiceType): void {
    if (this.order.servicesRendered === undefined){
      this.order.servicesRendered = new Array<RenderedService>(0);
    }
    this.order.servicesRendered.push({
      id: -1,
      serviceId: service.id,
      cost: service.cost,
      description: service.description} as RenderedService);
    this.reCalculateCost();
  }

  private reCalculateCost() {
    let newSum = 0.00;
    const originalValue = this.order.totalAmount;
    this.order.servicesRendered.forEach(a => newSum = newSum + a.cost);

    this.order.totalAmount = newSum;
    if (originalValue < this.order.totalAmount){
      this.order.amountDue += this.order.totalAmount - originalValue;
    }
  }

  removeRenderedService(id: number): void {
    this.order.servicesRendered.splice(
      this.order.servicesRendered.indexOf(this.order.servicesRendered.find(s => s.id === id)), 1);

    this.reCalculateCost();
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  ngOnInit(): void {
    this.editForm.controls.firstName.setValue(this.order.firstName);
    this.editForm.controls.amountDue.setValue(this.order.amountDue);
    this.editForm.controls.totalAmount.setValue(this.order.totalAmount);

  }

  saveForm() {
    this.order = {...this.order, firstName: this.editForm.value.firstName};
    this.dialogRef.close(this.order);
  }

  approvePayment() {
    this.ordersService.approvePayment(this.order.id);
    this.dialogRef.close(false);
  }
}

export class OrderEditDialogModel {

  constructor(
    public title: string,
    public order: Order,
    public serviceList: ServiceType[]) {
  }
}
