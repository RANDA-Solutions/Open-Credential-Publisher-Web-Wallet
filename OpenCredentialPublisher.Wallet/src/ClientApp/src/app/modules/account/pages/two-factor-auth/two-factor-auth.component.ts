import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { TwoFactorAuthenticationVM } from '@shared/models/twoFactorAuthenticationVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-two-factor-auth',
  templateUrl: './two-factor-auth.component.html',
  styleUrls: ['./two-factor-auth.component.scss']
})
export class TwoFactorAuthComponent implements OnInit {
  vm = new TwoFactorAuthenticationVM();
  userId: string;
  code: string;
  isError = false;
  message = 'loading two factor authentication';
  showSpinner = false;
  modelErrors = new Array<string>();
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
    this.accountService.getTwoFAVM()
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as TwoFactorAuthenticationVM;
          if (this.debug) console.log(`TwoFactorAuthComponent is2faEnabled: ${this.vm.is2faEnabled}`);
        } else {
          this.vm = new TwoFactorAuthenticationVM();
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
}
