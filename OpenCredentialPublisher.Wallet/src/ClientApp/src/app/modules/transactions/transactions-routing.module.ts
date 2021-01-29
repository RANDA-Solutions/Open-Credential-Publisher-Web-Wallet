import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionsDetailsComponent, TransactionsListComponent } from './components';

const routes: Routes = [
  {
      path: '',
      component: TransactionsListComponent
  },
  {
      path: ':id',
      component: TransactionsDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransactionsRoutingModule { }
