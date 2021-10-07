import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccessService } from '@modules/access/services/access.service';
import { ForgotPasswordModel } from '@shared/interfaces/forgot-password.interface';
import { PostResponseModel } from '@shared/interfaces/post-response.interface';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
	submitted = false;
  forgotPasswordForm:ForgotPasswordModel = { email: '' };
  modelErrors = [];

  constructor(private accessServices: AccessService, private router: Router) { }

  ngOnInit(): void {
  }

  forgot({value, valid} : { value: ForgotPasswordModel; valid: boolean}) {
    this.submitted = true;
		this.modelErrors = [];
		if (valid) {
			this.accessServices.forgotPassword(value.email).subscribe((response: PostResponseModel) => {
              if (response.hasError) {
                this.modelErrors = response.errorMessages;
              }
              else {
                this.router.navigate(['/access/forgot-password-confirmation']);
              }
            });
		}
  }


}
