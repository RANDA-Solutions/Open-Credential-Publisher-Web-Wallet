import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AccountService } from '@modules/account/account.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { MessageService } from 'primeng/api';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-account-profile',
  templateUrl: './accountprofile.component.html',
  styleUrls: ['./accountprofile.component.scss']
})
export class AccountProfileComponent implements OnInit {
  profile: any
  modelErrors = new Array<string>();
  showSpinner = false;
  public form: FormGroup;
  private debug = false;
  message = 'loading profile'
  statusMessage = '';
  isError = false;

  constructor(private _accountService: AccountService, private _formBuilder: FormBuilder
    , public messageService: MessageService) { }

  ngOnInit(): void {
    this.showSpinner = true
    this._accountService.getProfile()
    .pipe(take(1)).subscribe(data => {
      if (data.statusCode == 200) {

        this.profile = (<ApiOkResponse>data).result;
        this.form = this._formBuilder.group({
          displayName: [this.profile.displayName],
          phoneNumber: [this.profile.phoneNumber],//, [Validators.required]],
        });
      } else {
        //this.relationships = new Array<WalletVM>();
        this.modelErrors = (<ApiBadRequestResponse>data).errors;
      }
      this.showSpinner = false;
    });

  }

  onSubmit() {
    return this._accountService.saveProfile(this.form.value).subscribe((data: ApiOkResponse) => {
      this.profile = (<ApiOkResponse>data).result;
      this.messageService.add({
        key: 'main', severity: 'success', summary: 'Success Message'
        , detail: 'Profile updated.'
      });
      this.form = this._formBuilder.group({
        displayName: [this.profile.displayName],
        phoneNumber: [this.profile.phoneNumber],//, [Validators.required]],
      });
    });
  }

}
