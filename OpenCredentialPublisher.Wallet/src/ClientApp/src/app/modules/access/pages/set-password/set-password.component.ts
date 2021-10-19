import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccessService } from '@modules/access/services/access.service';
import { untilDestroyed } from '@ngneat/until-destroy';
import { PostResponseModel } from '@shared/interfaces/post-response.interface';
import { SetPasswordModel } from '@shared/interfaces/set-password.interface';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-set-password',
  templateUrl: './set-password.component.html',
  styleUrls: ['./set-password.component.scss']
})
export class SetPasswordComponent implements OnInit {
	submitted = false;
  setPasswordForm:SetPasswordModel = { password: '', confirmPassword: '' };
  modelErrors = [];

  private returnUrl: string;
  private sub: Subscription;

  constructor(private accessServices: AccessService, private activatedRoute: ActivatedRoute, private router: Router) { 
    
  }

  ngOnInit(): void {
    this.sub = this.activatedRoute.queryParams.pipe(untilDestroyed(this)).subscribe(
			(param: any) => {
				this.returnUrl = param['returnUrl'] ?? param['ReturnUrl'];
			});
  }
  
  set({value, valid} : { value: SetPasswordModel; valid: boolean}) {
    this.submitted = true;
		this.modelErrors = [];
		if (valid) {
			this.accessServices.setPassword(value.password).subscribe((response: PostResponseModel) => {
              if (response.hasError) {
                this.modelErrors = response.errorMessages;
              }
              else {
                this.router.navigate(['./set-password-confirmation', '']);
              }
            });
		}
  }


}
