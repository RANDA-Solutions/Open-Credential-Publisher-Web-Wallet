import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '@modules/account/account.service';
import { LoginService } from '@root/app/auth/login.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { RegisterAccountVM } from '@shared/models/registerAccountVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-register-account',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterAccountComponent implements OnInit {
  modelErrors = new Array<string>();
  input: RegisterAccountVM = new RegisterAccountVM();
  showSpinner = false;
  buttonSpinner = false;
  private returnUrl: string | null;
  private debug = false;
  constructor(private accountService: AccountService
    , private loginService: LoginService
    , private router: Router)
    { }

  ngOnInit(): void {
  }
  register({ value, valid }: { value: RegisterAccountVM; valid: boolean }){
    if (this.debug) console.log(`RegisterAccountComponent register`);
		if (valid) {
      value.returnUrl = this.loginService.returnUrl;
			this.buttonSpinner = true;
			this.accountService.registerAccount(value)
        .pipe(take(1)).subscribe(data => {
          if (data.statusCode == 200) {
            const url = (<ApiOkResponse>data).redirectUrl;
            if (this.debug) console.log(`RegisterAccountComponent redirect:${url}`);
            this.router.navigateByUrl(url);
          } else {
            this.modelErrors = (<ApiBadRequestResponse>data).errors;
            this.buttonSpinner = false;
          }
        }, (error) => {
          this.buttonSpinner = false;
        });
		}
	}
}
