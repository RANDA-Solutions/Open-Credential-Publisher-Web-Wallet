import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';


@UntilDestroy()
@Component({
  selector: 'app-register-confirmation',
  templateUrl: './register-confirmation.component.html',
  styleUrls: ['./register-confirmation.component.scss']
})
export class RegisterConfirmationComponent implements OnInit {
  userId: string;
  code: string;
  message = '';
  confirmationMessage = '';
  showSpinner = false;
  allowEmailConfirmation = environment.allowSelfEmailConfirmation;
  private sub: Subscription;
  

  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService) {
    this.sub = this.route.queryParams.pipe(untilDestroyed(this)).subscribe(
			(param: any) => {
				this.userId = param['userId'];
				this.code = param['code'];
			});
  }

  ngOnInit(): void {
  }
  get qParams(){
    return {
      userId: this.userId,
      code: this.code
    };
  }
}
