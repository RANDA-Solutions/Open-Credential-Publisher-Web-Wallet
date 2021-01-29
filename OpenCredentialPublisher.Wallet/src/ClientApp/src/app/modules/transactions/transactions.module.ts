import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TransactionsDetailsComponent, TransactionsListComponent } from './components';
import { TransactionsRoutingModule } from './transactions-routing.module';



@NgModule({
  declarations: [TransactionsDetailsComponent, TransactionsListComponent],
  imports: [
    CommonModule,
    TransactionsRoutingModule
  ]
})
export class TransactionsModule { }
