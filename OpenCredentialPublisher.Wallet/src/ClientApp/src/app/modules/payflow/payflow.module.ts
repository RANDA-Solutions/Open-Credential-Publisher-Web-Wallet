import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DirectivesModule } from '../directives/directives.module';
import { AddCreditModalComponent, CompleteModalComponent, ProcessingModalComponent } from './components';
import { PayflowService } from './services/payflow.service';

@NgModule({
    declarations: [
        AddCreditModalComponent, CompleteModalComponent, ProcessingModalComponent
    ],
    imports: [
      CommonModule, DirectivesModule, FormsModule, HttpClientModule, ReactiveFormsModule
    ],
    exports: [
        AddCreditModalComponent, CompleteModalComponent, ProcessingModalComponent
    ],
    providers: [
        PayflowService
    ]
  })
  export class PayflowModule { }
  