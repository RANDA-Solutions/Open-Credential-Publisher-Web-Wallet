import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AccountService } from '@modules/account/account.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ChangeEmailVM } from '@shared/models/changeEmailVM';
import { MessageService } from 'primeng/api';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-email',
  templateUrl: './email.component.html',
  styleUrls: ['./email.component.scss']
})
export class EmailComponent implements OnInit {
  submitted = false;
  changeEmailVM = new ChangeEmailVM();
  modelErrors = new Array<string>();
  statusMessage = '';
  message = 'loading email';
  showSpinner = false;

  public form: FormGroup;

  constructor(private accountService: AccountService, private _formBuilder: FormBuilder, public messageService: MessageService) { }

  ngOnInit(): void {
    this.message = 'loading email';
    this.showSpinner = true;
    this.accountService.getEmail()
    .pipe(take(1)).subscribe(data => {
      if (data.statusCode == 200) {
        this.changeEmailVM = (<ApiOkResponse>data).result;
      } else {
        this.modelErrors = (<ApiBadRequestResponse>data).errors;
      }
      this.showSpinner = false;
    });

  }

  SendVerificationEmail() {
    this.message = 'requesting verification email';
    this.showSpinner = true;
    this.accountService.sendVerificationEmail()
    .pipe(take(1)).subscribe(data => {
      this.statusMessage = (<ApiOkResponse>data).result;
      this.showSpinner = false;
    });

  }

  change({value, valid} : { value: ChangeEmailVM; valid: boolean}) {
    this.submitted = true;
    this.message = 'saving new email';
    this.showSpinner = true;
    this.accountService.saveEmail(value)
        .pipe(take(1)).subscribe(data => {
          this.statusMessage = (<ApiOkResponse>data).result;
          this.showSpinner = false;
          this.message = 'loading email';
        });
  }

}
