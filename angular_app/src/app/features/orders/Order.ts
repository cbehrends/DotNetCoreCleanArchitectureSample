import {RenderedService} from './RenderedService';

export interface Order {
  id: number;
  firstName: string;
  totalAmount: number;
  amountDue: number;
  servicesRendered: RenderedService[];
}
