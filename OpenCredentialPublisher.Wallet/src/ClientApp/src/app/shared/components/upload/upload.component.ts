import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CredentialService } from '@core/services/credentials.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiResponse } from '@shared/models/apiResponse';


@Component({
  selector: '[app-upload]',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
  @Output() fileUploaded = new EventEmitter<any>();
  fileName = 'Choose CLR';
  file: File;
  isCollapsed = true;
  showSpinner = false;
  modelErrors = new Array<string>();
  message = "uploading";
  private debug = false;
  constructor(private route: ActivatedRoute, private router: Router, private credentialService: CredentialService
    , private http: HttpClient, private utilsService: UtilsService) { }

  ngOnInit(): void {
  }
  onFileSelected(event) {
    this.file = (event.target as HTMLInputElement).files[0];
    this.fileName = this.file.name;
  }
  fileUpload() {
    if (this.file) {
      this.showSpinner = true;
      const urlApi = `${environment.apiEndPoint}credentials/upload`;
      const formData = new FormData();
      formData.append('file', this.file, this.file.name);
      this.http.post(urlApi, formData, {reportProgress: true, observe: 'events'})
        .subscribe(event => {
          if (event.type === HttpEventType.UploadProgress) {
            if (this.debug) console.log(`UploadComponent upload progress: ${Math.round(100 * event.loaded / event.total)}`) ;
          } else if (event.type === HttpEventType.Response) {
            if ((event.body as ApiResponse).statusCode == 200) {
              this.fileUploaded.emit(true);
            } else {
              this.modelErrors = (<ApiBadRequestResponse>event.body).errors;
            }
            this.showSpinner = false;
          }
        }, (error) => {
          this.utilsService.handleError(error)
        });
    } else {
      this.fileName = 'Choose CLR';
    }
  }
  collapseToggle(event: any){
    if (event.target.tagName == 'A') return;
    this.isCollapsed = !this.isCollapsed;
  }
}
