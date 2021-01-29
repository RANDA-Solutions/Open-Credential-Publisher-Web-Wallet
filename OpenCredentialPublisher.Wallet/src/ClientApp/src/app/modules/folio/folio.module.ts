import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FolioComponent, FolioDraftsComponent, FolioSendAllComponent, FolioSendComponent, FolioStartComponent } from './components';
import { FolioRoutingModule } from './folio-routing.module';

@NgModule({
  declarations: [
    FolioStartComponent,
    FolioSendComponent,
    FolioSendAllComponent,
    FolioDraftsComponent,
    FolioComponent
  ],
  imports: [
    CommonModule,
    FolioRoutingModule
  ]
})
export class FolioModule { }
