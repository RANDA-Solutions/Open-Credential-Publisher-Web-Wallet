import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnChanges, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CredentialService } from '@core/services/credentials.service';
import { DownloadService } from '@core/services/download.service';
import { environment } from '@environment/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { PackageTypeEnum } from '@shared/models/enums/packageTypeEnum';
import { PdfRequestTypeEnum } from '@shared/models/enums/pdfRequestTypeEnum';
import { PackageVM } from '@shared/models/packageVM';
import { PdfRequest } from '@shared/models/pdfRequest';
import { EvidenceService } from '@shared/services/evidence.service';
import { take } from 'rxjs/operators';


@UntilDestroy()
@Component({
  selector: 'app-display-credential-component',
  templateUrl: './display-credential.component.html',
  styleUrls: ['./display-credential.component.scss']
})
export class DisplayCredentialComponent  implements OnChanges, OnInit{
  packageId: number;
  package = new PackageVM();
  showSpinner = false;
  miniSpinner = false;
  modelErrors = new Array<string>();
  private debug = false;
  message = 'loading credential';
  private sub: any;

  constructor(private route: ActivatedRoute, private router: Router, private credentialService: CredentialService
    , private evidenceService: EvidenceService
    , private downloads: DownloadService
    , @Inject(DOCUMENT) private document: any) {
  }
  ngOnChanges() {
    this.showSpinner = true;
    if (this.debug) console.log('DisplayCredentialComponent ngOnChanges');
    this.getData(this.packageId );
  }
  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('DisplayCredentialComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.packageId = params['id'];
      console.log(params['id']);
      this.getData(this.packageId );
   });

   this.evidenceService.setAccessKey(null);
   this.evidenceService.setRequestType(PdfRequestTypeEnum.OwnerViewPdf);

  }
  downloadVCJson(){
    this.miniSpinner = true;
    if (this.debug) console.log('DisplayCredentialComponent downloadVCJson');
    this.downloads.vcJson(this.packageId)
      .pipe(take(1))
      .subscribe(resp => {
        var contentDispositionHeader = resp.headers.get('Content-Disposition');
        var filename = contentDispositionHeader.split(';')[1].trim().split('=')[1].replace(/"/g, '');
        if (this.debug) console.log(`DisplayCredentialComponent VCJson:${JSON.stringify(resp.body)}`);
        this.downloads.saveAs(JSON.stringify(resp.body), filename, 'Pdf');
        this.miniSpinner = false;
      });
  }
  showPdf(){
    const dr : PdfRequest = {
      requestType: PdfRequestTypeEnum.LinkViewPdf,
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
      if (this.debug) console.log('DisplayCredentialComponent showPdf');
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
    console.log(`DisplayCredentialComponent getData ${id}`);
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
  public get PackageTypeEnum() {
    return PackageTypeEnum;
  }
  topFunction():void {
    const element = document.getElementById ('theTop');
    element.scrollIntoView();
  }
}
