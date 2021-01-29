import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormComponent } from './form/form.component';

@NgModule({
    declarations: [
        FormComponent
    ],
    imports: [
      CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule
    ],
    exports: [
        FormComponent
    ]
  })
  export class DirectivesModule { }
  