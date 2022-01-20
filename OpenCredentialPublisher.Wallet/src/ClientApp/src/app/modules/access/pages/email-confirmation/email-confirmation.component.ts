import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { LoginService } from '@root/app/auth/login.service';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss']
})
export class EmailConfirmationComponent implements OnInit {
  userId: string;
  code: string;
  returnUrl: string;
  message = '';
  confirmationMessage = '';
  showSpinner = false;
  private sub: Subscription;

  constructor(private route: ActivatedRoute
    , private router: Router
    , private accountService: AccountService
    , private loginService: LoginService) {
    this.sub = this.route.queryParams.pipe(untilDestroyed(this)).subscribe(
			(param: any) => {
				this.userId = param['userId'];
				this.code = param['code'];
        this.returnUrl = param['returnUrl'];
        if (environment.debug)
          console.log(param);
        this.getData();
			});
  }

  ngOnInit(): void {
  }

  getData() {
    if (this.userId == null || this.code == null){
      this.message = 'Confirmation information is not valid.';
    }
    this.showSpinner = true;
    this.accountService.confirmEmailAccount(this.userId, this.code)
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          if (this.returnUrl) {
            this.loginService.storeReturnUrl(this.returnUrl);
          }
          this.confirmationMessage = 'Success';
        } else {
          this.confirmationMessage = 'Error - Confirmation was not successful.';
        }
        this.showSpinner = false;
      });
  }
}
