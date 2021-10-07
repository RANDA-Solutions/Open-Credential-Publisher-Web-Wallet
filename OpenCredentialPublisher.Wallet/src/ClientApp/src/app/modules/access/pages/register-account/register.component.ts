import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
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
  private returnUrl: string | null;
  private debug = false;
  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }
  register({ value, valid }: { value: RegisterAccountVM; valid: boolean }){
    if (this.debug) console.log(`RegisterAccountComponent register`);
		if (valid) {
			this.showSpinner = true;
			this.accountService.registerAccount(value)
        .pipe(take(1)).subscribe(data => {
          if (data.statusCode == 200) {
            const url = (<ApiOkResponse>data).redirectUrl;
            if (this.debug) console.log(`RegisterAccountComponent redirect:${url}`);
            this.router.navigateByUrl(url);
          } else {
            this.modelErrors = (<ApiBadRequestResponse>data).errors;
          }
          this.showSpinner = false;
        });
		}
	}
}
