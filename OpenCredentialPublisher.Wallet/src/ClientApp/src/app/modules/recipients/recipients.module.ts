import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ListComponent } from './components/list.component';
import { DeleteComponent } from './components/delete.component';
import { CreateComponent } from './components/create.component';
import { EditComponent } from './components/edit.component';



@NgModule({
  declarations: [ListComponent, DeleteComponent, CreateComponent, EditComponent],
  imports: [
    CommonModule
  ]
})
export class RecipientsModule { }
