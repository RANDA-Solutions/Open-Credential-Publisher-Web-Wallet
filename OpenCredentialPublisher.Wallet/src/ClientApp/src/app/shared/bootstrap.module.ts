import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


const bootstrapModules = [
  NgbModule
];

@NgModule({
    imports: [ bootstrapModules ],
    declarations: [
    ],
    providers: [],
    exports: [ bootstrapModules ]

    })
      export class BootstrapModule { }
