import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccessService } from '@modules/access/services/access.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ResetPasswordModel } from '@shared/interfaces/reset-password.interface';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
	submitted = false;
  resetPasswordForm:ResetPasswordModel = { email: '', password: '', confirmPassword: '', code: '' };
  modelErrors = [];
  buttonSpinner = false;
  private sub: Subscription;

  constructor(private accessServices: AccessService, private route: ActivatedRoute, private router: Router) {
    this.sub = this.route.queryParams.pipe(untilDestroyed(this)).subscribe(
			(param: any) => {
				this.resetPasswordForm.code = param['code'];
        this.resetPasswordForm.email = param['email'];
			});
  }

  ngOnInit(): void {
  }

  reset({value, valid} : { value: ResetPasswordModel; valid: boolean}) {
    this.submitted = true;
		this.modelErrors = [];
		if (valid) {
      this.buttonSpinner = true;
      value.code = this.resetPasswordForm.code;
      value.email = this.resetPasswordForm.email;
			this.accessServices.resetPassword(value)
      .pipe(take(1)).subscribe(data => {
        //console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/access/reset-password-confirmation']);
        } else {
          this.buttonSpinner = false;
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
      }, (error) => this.buttonSpinner = false);
		}
  }
}
