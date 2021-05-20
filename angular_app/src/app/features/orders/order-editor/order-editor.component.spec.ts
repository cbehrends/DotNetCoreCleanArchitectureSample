import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { OrderEditorComponent } from './order-editor.component';
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from '@angular/material/dialog';
import {FormBuilder} from '@angular/forms';
import {Order} from '../Order';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';

describe('OrderEditorComponent', () => {
  let component: OrderEditorComponent;
  let fixture: ComponentFixture<OrderEditorComponent>;
  const formBuilder: FormBuilder = new FormBuilder();

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderEditorComponent ],
      imports: [HttpClientTestingModule],
      providers: [MatDialogModule,
        {provide: MatDialogRef, useValue: {}},
        FormBuilder,
        // { provide: FormBuilder, useValue: formBuilder },
        {provide: MAT_DIALOG_DATA, useValue: {}},
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderEditorComponent);
    component = fixture.componentInstance;
    component.editForm = formBuilder.group({
      firstName: ''
    });
    component.order = {firstName: 'foo'} as Order;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
