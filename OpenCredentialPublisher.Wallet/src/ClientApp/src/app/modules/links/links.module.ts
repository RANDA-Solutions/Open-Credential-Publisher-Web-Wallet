import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CreateComponent } from './components/create.component';
import { DeleteComponent } from './components/delete.component';
import { DisplayComponent } from './components/display.component';
import { ListComponent } from './components/list.component';
import { NotAvailableComponent } from './components/not-available.component';
import { ShareComponent } from './components/share.component';



@NgModule({
  declarations: [CreateComponent, DeleteComponent, DisplayComponent, ListComponent, NotAvailableComponent, ShareComponent],
  imports: [
    CommonModule
  ]
})
export class LinksModule { }
