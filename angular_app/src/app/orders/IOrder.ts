import {IRenderedService} from './IRenderedService';

export interface IOrder {
  id: number;
  firstName: string;
  totalAmount: number;
  amountDue: number;
  servicesRendered: IRenderedService[];
}
