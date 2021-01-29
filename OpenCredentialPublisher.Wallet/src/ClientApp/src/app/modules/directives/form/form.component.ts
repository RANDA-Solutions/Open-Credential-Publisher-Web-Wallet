import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { finalize, first, tap } from 'rxjs/operators';
import { CanDeactivateComponent } from '../can-deactivate/can-deactivate.component';
import { Result } from '../models/result';

// @Directive({
//   selector: 'form'
// })
@Component({
  template: 'NOT FOR STANDALONE USE'
})
export class FormComponent extends CanDeactivateComponent {
  public submitted: boolean = false;
  public loading: boolean = false;
  public skipDirtyCheck: boolean = false;
  public form: FormGroup;

  constructor() {
    super();
  }

  get f() {
    return this.form.controls;
  }

  submitForm(x: () => Observable<any>, scrollToError: boolean = true) {
    this.submitted = true;

    if (this.form && this.form.invalid) {
      this.getFormValidationErrors();

      if (scrollToError) this.scrollToError();

      let result = new Result<any>();
      result.success = false;
      return of(result);
    }

    this.loading = true;

    return x().pipe(
      first(),
      tap((result: Result<any>) => {
        if (result && result.success) this.resetForm();
      }),
      finalize(() => {
        this.resetFormStatus();
      })
    );
  }

  canDeactivate(): boolean {
    if (this.skipDirtyCheck || !this.form) return true;

    return !(this.form.dirty && !this.submitted);
  }

  private getFormValidationErrors() {
    Object.keys(this.form.controls).forEach(key => {
        const controlInvalid: boolean = this.form.get(key).invalid;
        if (controlInvalid) {
            console.log('key, error: ', key, this.form.get(key).errors);
        }
    });
  }

  private resetFormStatus() {
    this.submitted = false;
    this.loading = false;
  }

  public resetForm() {
    if (this.form) {
      this.form.markAsPristine();
      this.form.markAsUntouched();
    }
  }

  private scrollToError() {
    const firstElementWithError = document.querySelector('.ng-invalid');

    if (firstElementWithError) {
      firstElementWithError.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
      });
    }
  }
}
