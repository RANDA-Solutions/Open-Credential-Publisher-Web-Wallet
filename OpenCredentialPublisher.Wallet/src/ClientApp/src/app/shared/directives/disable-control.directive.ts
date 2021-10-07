import { Directive, Input } from '@angular/core';
import { NgControl } from '@angular/forms';
@Directive({
    // eslint-disable-next-line @angular-eslint/directive-selector
    selector: '[appDisableControl]'
  })
  export class DisableControlDirective {
    @Input() set appDisableControl( condition: boolean ) {
      const action = condition ? 'disable' : 'enable';
      this.ngControl.control[action]();
    }
    constructor( private ngControl: NgControl ) {
    }
  }
