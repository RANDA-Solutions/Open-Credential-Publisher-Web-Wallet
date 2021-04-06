import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ListComponent } from './components/list.component';
import { InvitationComponent } from './components/invitation.component';
import { SendComponent } from './components/send.component';
import { EditComponent } from './components/edit.component';
import { DeleteComponent } from './components/delete.component';



@NgModule({
  declarations: [ListComponent, InvitationComponent, SendComponent, EditComponent, DeleteComponent],
  imports: [
    CommonModule
  ]
})
export class WalletsModule { }
