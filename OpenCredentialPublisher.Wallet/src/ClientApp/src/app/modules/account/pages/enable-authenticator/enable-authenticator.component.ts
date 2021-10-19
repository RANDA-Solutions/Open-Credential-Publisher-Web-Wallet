import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { TwoFactorAuthenticationModelInput } from '@shared/models/TwoFactorAuthModelInput';
import { TwoFactorEnableAuthenticationVM } from '@shared/models/twoFactorEnableAuthenticationVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-enable-authenticator',
  templateUrl: './enable-authenticator.component.html',
  styleUrls: ['./enable-authenticator.component.scss']
})
export class EnableAuthenticatorComponent implements OnInit {
  vm = new TwoFactorEnableAuthenticationVM();
  input = new TwoFactorAuthenticationModelInput();
  userId: string;
  isError = false;
  message = 'loading two factor authentication';
  showSpinner = false;
  modelErrors = new Array<string>();
  submitted = false;
  private debug = false;

  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService) {
  }

  ngOnInit(): void {
    this.getData();
  }

  getData() {
    // if (this.userId == null || this.code == null){
    //   this.vm.statusMessage = 'Error - Confirmation information is not valid.';
    // }
    this.showSpinner = true;
    this.accountService.getTwoFAKeyNCode()
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as TwoFactorEnableAuthenticationVM;
        } else {
          this.vm = new TwoFactorEnableAuthenticationVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
          this.isError = this.vm.statusMessage.startsWith('Error');
        }
        if (this.vm.statusMessage == null) {
          this.isError = false;
        } else {
          this.isError = this.vm.statusMessage.startsWith('Error');
        }
        this.showSpinner = false;
      });
  }
  sendCode({value, valid} : { value: TwoFactorAuthenticationModelInput; valid: boolean}) {
    if (this.debug) console.log(`EnableAuthenticatorComponent sendCode: ${value.twoFactorCode}`);
    this.submitted = true;
		this.modelErrors = [];
		if (valid) {
			this.accountService.sendCode(value.twoFactorCode)
        .pipe(take(1)).subscribe(data => {
          if (data.statusCode == 200) {
            this.vm = (<ApiOkResponse>data).result as TwoFactorEnableAuthenticationVM;
            if ((<ApiOkResponse>data).redirectUrl != null) {
              if ((<ApiOkResponse>data).redirectUrl == 'ShowRecoveryCodes')  {
                this.router.navigate(['/account/manage/show-recovery-codes', (<ApiOkResponse>data).result as TwoFactorEnableAuthenticationVM]);
              } else if ((<ApiOkResponse>data).redirectUrl == 'TwoFactorAuthentication') {
                this.router.navigate(['/account/manage/two-factor-auth', (<ApiOkResponse>data).result as TwoFactorEnableAuthenticationVM]);
              } else {
                this.isError = true;
                this.vm.statusMessage = `Error unexpected redirect: ${(<ApiOkResponse>data).redirectUrl} ` + this.vm.statusMessage;
              }
            }
          } else {
            this.vm = new TwoFactorEnableAuthenticationVM();
            this.modelErrors = (<ApiBadRequestResponse>data).errors;
            this.isError = this.vm.statusMessage.startsWith('Error');
          }
        });
		}
  }
}
