import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { TwoFactorAuthenticationVM } from '@shared/models/twoFactorAuthenticationVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-disable-twofa',
  templateUrl: './disable-twofa.component.html',
  styleUrls: ['./disable-twofa.component.scss']
})
export class DisableTwoFAComponent implements OnInit {
  vm = new TwoFactorAuthenticationVM();
  message = "";
  showSpinner = false;
  private debug = false;
  isError = false;
  modelErrors = new Array<string>();
  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService) { }

  ngOnInit(): void {
  }

  disable2FA() {
    if (this.debug) console.log(`DisableTwoFAComponent disable2FA`);
		this.modelErrors = [];
    this.accountService.disable2FA()
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as TwoFactorAuthenticationVM;
          this.router.navigate(['/account/manage/two-factor-auth']);
        } else {
          this.vm = new TwoFactorAuthenticationVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
          this.isError = this.vm.statusMessage.startsWith('Error');
        }
      });

  }
}

