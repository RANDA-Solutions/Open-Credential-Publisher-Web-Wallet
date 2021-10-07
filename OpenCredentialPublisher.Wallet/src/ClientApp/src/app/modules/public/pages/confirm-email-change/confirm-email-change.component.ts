import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '@modules/account/account.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { VerifyEmailVM } from '@shared/models/verifyEmailVM';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-confirm-email-change',
  templateUrl: './confirm-email-change.component.html',
  styleUrls: ['./confirm-email-change.component.scss']
})
export class ConfirmEmailChangeComponent implements OnInit {
  vm = new VerifyEmailVM();
  userId: string;
  code: string;
  isError = false;
  message = 'confirming email change';
  statusMessage = '';
  showSpinner = false;
  private sub: Subscription;

  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService) {
    this.sub = this.route.queryParams.pipe(untilDestroyed(this)).subscribe(
			(param: any) => {
				this.vm.userId = param['userId'];
				this.vm.email = param['email'];
				this.vm.code = param['code'];
        this.getData();
			});
  }

  ngOnInit(): void {
  }

  getData() {
    if (this.userId == null || this.code == null){
      this.statusMessage = 'Error - Confirmation information is not valid.';
    }
    this.showSpinner = true;
    const vm = new VerifyEmailVM();
    this.accountService.confirmEmailChange(this.vm)
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.statusMessage = (<ApiOkResponse>data).result;
          this.isError = this.statusMessage.startsWith('Error');
        } else {
          this.statusMessage = 'Error - Confirmation was not successful.';
          this.isError = true;
        }
        this.showSpinner = false;
      });
  }
}

