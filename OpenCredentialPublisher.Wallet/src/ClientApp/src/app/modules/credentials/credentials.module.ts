import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './components/list.component';
import { DisplayComponent } from './components/display.component';



@NgModule({
  declarations: [ListComponent, DisplayComponent],
  imports: [
    CommonModule
  ]
})
export class CredentialsModule { }
