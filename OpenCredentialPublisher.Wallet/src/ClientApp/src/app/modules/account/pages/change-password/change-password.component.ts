import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AccountService } from '@modules/account/account.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ChangePasswordVM } from '@shared/models/changePasswordVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  vm = new ChangePasswordVM();
  modelErrors = new Array<string>();
  message = 'loading';
  isError = false;
  showSpinner = false;
  statusMessage = '';
  submitted = false;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  changePassword({value, valid} : { value: ChangePasswordVM; valid: boolean}) {
    this.submitted = true;
		this.modelErrors = [];
		if (valid) {
      this.accountService.changePassword(value)
        .pipe(take(1)).subscribe(data => {
          if (data.statusCode == 200) {
            this.statusMessage = (<ApiOkResponse>data).result;
          } else {
            this.modelErrors = (<ApiBadRequestResponse>data).errors;
          }
          this.showSpinner = false;
        });
    }
  }
}
