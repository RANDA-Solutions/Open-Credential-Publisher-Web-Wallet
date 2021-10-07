import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { TwoFactorAuthenticationVM } from '@shared/models/twoFactorAuthenticationVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-reset-authenticator',
  templateUrl: './reset-authenticator.component.html',
  styleUrls: ['./reset-authenticator.component.scss']
})
export class ResetAuthenticatorComponent implements OnInit {
  vm = new TwoFactorAuthenticationVM();
  message = "";
  showSpinner = false;
  private debug = false;
  isError = false;
  modelErrors = new Array<string>();
  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService) { }

  ngOnInit(): void {
  }

  resetAuthenticator() {
    if (this.debug) console.log(`ResetAuthenticatorComponent resetAuthenticator`);
		this.modelErrors = [];
    this.accountService.resetAuthenticator()
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as TwoFactorAuthenticationVM;
          this.router.navigate(['/account/manage/enable-authenticator']);
        } else {
          this.vm = new TwoFactorAuthenticationVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
          this.isError = this.vm.statusMessage.startsWith('Error');
        }
      });

  }
}
