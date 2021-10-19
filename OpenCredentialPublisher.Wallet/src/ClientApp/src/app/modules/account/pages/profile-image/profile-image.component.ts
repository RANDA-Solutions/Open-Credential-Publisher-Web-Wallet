import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ApiResponse } from '@shared/models/apiResponse';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls: ['./profile-image.component.scss']
})

export class ProfileImageComponent implements OnInit {
  fileName = 'Choose Picture';
  file: File;
  currentImage: any
  modelErrors = new Array<string>();
  showSpinner = false;
  public form: FormGroup;
  theFile: any = null;
  message = 'loading profile image';

  constructor(private _accountService: AccountService, private _formBuilder: FormBuilder
    , private http: HttpClient, public messageService: MessageService, ) { }

  ngOnInit(): void {
    this.showSpinner = true
    this._accountService.getProfileImage()
    .pipe(take(1)).subscribe(data => {
      if (data.statusCode == 200) {

        this.currentImage = (<ApiOkResponse>data).result;
        this.form = this._formBuilder.group({
          profileImage: this.currentImage
        });

      } else {
        //this.relationships = new Array<WalletVM>();
        this.modelErrors = (<ApiBadRequestResponse>data).errors;
      }
      this.showSpinner = false;
    });

  }

  onFileSelected(event: any) {
    this.file = (event.target as HTMLInputElement).files[0];
    this.fileName = this.file.name;
  }

  fileUpload() {
    if (this.file) {
      this.message = 'uploading profile image';
      this.showSpinner = true;
      const urlApi = `${environment.apiEndPoint}account/saveProfileImage`;
      const formData = new FormData();
      formData.append('file', this.file, this.file.name);
      this.http.post(urlApi, formData, {reportProgress: true, observe: 'events'})
        .subscribe(event => {
          if (event.type === HttpEventType.UploadProgress)
            console.log(`upload progress: ${Math.round(100 * event.loaded / event.total)}`) ;
          else if (event.type === HttpEventType.Response) {
            if ((event.body as ApiResponse).statusCode == 200) {
              this.currentImage = (event.body as ApiOkResponse).result
              this.messageService.add({
                key: 'main', severity: 'success', summary: 'Success Message'
                , detail: 'Profile image updated.'
              });
            } else {
              this.modelErrors = (<ApiBadRequestResponse>event.body).errors;
            }
            this.showSpinner = false;
            this.message = 'loading profile image';
          }
        });
    } else {
      this.fileName = 'Choose Picture';
    }
  }
}
