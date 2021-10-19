import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { TwoFactorEnableAuthenticationVM } from '@shared/models/twoFactorEnableAuthenticationVM';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-show-recovery-codes',
  templateUrl: './show-recovery-codes.component.html',
  styleUrls: ['./show-recovery-codes.component.scss']
})
export class ShowRecoveryCodesComponent implements OnInit {
  vm = new TwoFactorEnableAuthenticationVM();
  message = "";
  showSpinner = true;
  private debug = false;
  private sub: Subscription;
  isError = false;
  modelErrors = new Array<string>();

  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService) {
  }

  ngOnInit() {
    if (this.debug) console.log('ShowRecoveryCodesComponent ngOnInit');
    this.getData();
  }
  getData() {
  this.accountService.generateRecoveryCodes()
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as TwoFactorEnableAuthenticationVM;
          this.router.navigate(['/account/manage/show-recovery-codes', this.vm]);
        } else {
          this.vm = new TwoFactorEnableAuthenticationVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
          this.isError = this.vm.statusMessage.startsWith('Error');
        }
        this.showSpinner = false;
      });
    }

}
