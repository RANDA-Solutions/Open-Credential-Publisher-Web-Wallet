import { Component, OnChanges, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CredentialService } from '@core/services/credentials.service';
import { DownloadService } from '@core/services/download.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { PackageTypeEnum } from '@shared/models/enums/packageTypeEnum';
import { PdfRequestTypeEnum } from '@shared/models/enums/pdfRequestTypeEnum';
import { PackageVM } from '@shared/models/packageVM';
import { PdfRequest } from '@shared/models/pdfRequest';
import { take } from 'rxjs/operators';


@UntilDestroy()
@Component({
  selector: 'app-delete-credential-component',
  templateUrl: './delete-credential.component.html',
  styleUrls: ['./delete-credential.component.scss']
})
export class DeleteCredentialComponent  implements OnChanges, OnInit{
  packageId: number;
  package = new PackageVM();
  showSpinner = false;
  miniSpinner = false;
  modelErrors = new Array<string>();
  private debug = false;
  message = 'loading credential';
  private sub: any;

  constructor(private route: ActivatedRoute, private router: Router, private credentialService: CredentialService
    , private downloads: DownloadService) {
  }
  ngOnChanges() {
    this.showSpinner = true;
    if (this.debug) console.log('DeleteCredentialComponent ngOnChanges');
    this.getData(this.packageId );
  }
  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('DeleteCredentialComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.packageId = params['id'];
      console.log(params['id']);
      this.getData(this.packageId );
   });
  }
  downloadVCJson(){
    this.miniSpinner = true;
    if (this.debug) console.log('DeleteCredentialComponent downloadVCJson');
    this.downloads.vcJson(this.packageId)
      .pipe(take(1))
      .subscribe(resp => {
        var contentDispositionHeader = resp.headers.get('Content-Disposition');
        var filename = contentDispositionHeader.split(';')[1].trim().split('=')[1].replace(/"/g, '');
        this.downloads.saveAs(resp.body, filename, 'Pdf');
        this.miniSpinner = false;
      });
  }
  downloadPdf(){
    const dr : PdfRequest = {
      requestType: PdfRequestTypeEnum.OwnerViewPdf,
      linkId: null,
      clrId: this.package.newestPdfTranscript.clrId,
      assertionId: this.package.newestPdfTranscript.assertionId,
      evidenceName: this.package.newestPdfTranscript.evidenceName,
      artifactId: this.package.newestPdfTranscript.artifactId,
      artifactName: this.package.newestPdfTranscript.artifactName,
      createLink: true,
      accessKey: null
    }
    this.miniSpinner = true;
      if (this.debug) console.log('DeleteCredentialComponent showPdf');
      this.downloads.pdf(dr)
        .pipe(take(1))
        .subscribe(resp => {
          let blob =  new Blob([resp.body], { type: 'application/pdf' })
          var fileURL = URL.createObjectURL(blob);
          window.open(fileURL);
          this.miniSpinner = false;
        });
  }
  getData(id: number):any {
    this.showSpinner = true;
    console.log(`DeleteCredentialComponent getData ${id}`);
    console.log(id);
    this.credentialService.getPackage(id)
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.package = (<ApiOkResponse>data).result as PackageVM;
          if (this.debug) console.log(JSON.stringify(this.package));
          if (this.debug) console.log(JSON.stringify(this.package.newestPdfTranscript));
        } else {
          this.package = new PackageVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }

  deletePackage():any {
    this.message = 'deleting credential';
    this.showSpinner = true;
    if (this.debug) console.log('DeleteCredentialComponent deletePackage');
    this.credentialService.deletePackage(this.packageId)
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        this.message = 'loading credential';
        if (data.statusCode == 200) {
          this.router.navigate(['/credentials'])
        } else {
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }

  public get PackageTypeEnum() {
    return PackageTypeEnum;
  }
  topFunction():void {
    const element = document.getElementById ('theTop');
    element.scrollIntoView();
  }
}
