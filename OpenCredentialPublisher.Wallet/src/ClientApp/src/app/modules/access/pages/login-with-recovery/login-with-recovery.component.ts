import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '@modules/account/account.service';
import { LoginService } from '@root/app/auth/login.service';
import { TwoFactorAuthenticationResultEnum } from '@shared/models/enums/twoFactorAuthenticationResultEnum';
import { TwoFactorAuthenticationModel } from '@shared/models/twoFactorAuthenticationModel';
import { TwoFactorAuthenticationModelInput } from '@shared/models/TwoFactorAuthModelInput';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-login-with-recovery',
  templateUrl: './login-with-recovery.component.html',
  styleUrls: ['./login-with-recovery.component.scss']
})
export class LoginWithRecoveryComponent implements OnInit {
  input = new TwoFactorAuthenticationModelInput();
  resultModel = new TwoFactorAuthenticationModel();
  userId: string;
  isError = false;
  message = 'loading two factor authentication';
  showSpinner = false;
  modelErrors = new Array<string>();
  submitted = false;
  private debug = false;

  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService
    , private loginService: LoginService,) {
  }

  ngOnInit(): void {
    if (this.debug) console.log(`LoginWithRecoveryComponent ngOnInit`);
  }

  login({value, valid} : { value: TwoFactorAuthenticationModelInput; valid: boolean}) {
    if (this.debug) console.log(`LoginWithRecoveryComponent sendCode: ${value.twoFactorCode}`);
    this.message = 'logging in';
    this.showSpinner = true;
    this.submitted = true;
		this.modelErrors = [];
		if (valid) {
			this.accountService.loginRecovery(value)
        .pipe(take(1)).subscribe(data => {
          this.resultModel = data as TwoFactorAuthenticationModel;
          if (this.debug) console.log(`LoginWithRecoveryComponent returned from api/Login result: ${this.resultModel.result}`);
          if (this.resultModel.result == TwoFactorAuthenticationResultEnum.Success) {
            this.loginService.completeLogin().then(resp => {
              if (this.debug) console.log(`LoginFormComponent returned from OAuthService.doLogin()`);
            });
            this.router.navigate([this.resultModel.returnUrl]);
          } else if (this.resultModel.result == TwoFactorAuthenticationResultEnum.Lockout)  {
              this.router.navigate(['/public/lockout']);
          } else {
            this.modelErrors.push(this.resultModel.errorMessage);
          }
        });
		}
  }

  selectionChange(){
    this.input.rememberMachine = !this.input.rememberMachine;
  }
}
