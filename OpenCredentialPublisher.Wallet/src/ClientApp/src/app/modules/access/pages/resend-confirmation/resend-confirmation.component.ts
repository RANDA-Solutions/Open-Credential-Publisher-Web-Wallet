import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccessService } from '@modules/access/services/access.service';
import { ForgotPasswordModel } from '@shared/interfaces/forgot-password.interface';
import { PostResponseModel } from '@shared/interfaces/post-response.interface';

@Component({
  selector: 'app-resend-confirmation',
  templateUrl: './resend-confirmation.component.html',
  styleUrls: ['./resend-confirmation.component.scss']
})
export class ResendConfirmationComponent implements OnInit {
	submitted = false;
  forgotPasswordForm:ForgotPasswordModel = { email: '' };
  modelErrors = [];
  buttonSpinner = false;
  sent = false;

  constructor(private accessServices: AccessService, private router: Router) { }

  ngOnInit(): void {
  }

  forgot({value, valid} : { value: ForgotPasswordModel; valid: boolean}) {
    if (value.email == null || !/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i.test(value.email)) {
      this.modelErrors = [ 'Please enter a valid email.'];
      return;
    }
    this.submitted = true;
		this.modelErrors = [];
		if (valid) {
      this.buttonSpinner = true;
			this.accessServices.resendConfirmation(value.email).subscribe((response: PostResponseModel) => {
              if (response.hasError) {
                this.modelErrors = response.errorMessages;
              }
              else {
                this.sent = true;
              }
              this.buttonSpinner = false;
            }, (error) => {
              this.buttonSpinner = false;
            });
		}
  }


}
