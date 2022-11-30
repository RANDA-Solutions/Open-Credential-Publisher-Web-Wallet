import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnChanges, OnInit, ViewChild } from '@angular/core';
import { DownloadService } from '@core/services/download.service';
import { VerificationService } from '@core/services/verification.service';
import { LinksService } from '@modules/links/links.service';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { PdfRequestTypeEnum } from '@shared/models/enums/pdfRequestTypeEnum';
import { LinkDisplayVM } from '@shared/models/linkDisplayVM';
import { LinkDisplayVMNew } from '@shared/models/linkDisplayVMNew';
import { PdfRequest } from '@shared/models/pdfRequest';
import { PdfShare } from '@shared/models/pdfShare';
import { EvidenceService } from '@shared/services/evidence.service';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-link-summary]',
  templateUrl: './link-summary.component.html',
  styleUrls: ['./link-summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LinkSummaryComponent implements OnChanges, OnInit {
  @Input() link = new LinkDisplayVM();
  @ViewChild('tt') tooltip: NgbTooltip;
  message = 'loading summary';
  showSpinner = true;
  miniSpinner = false;
  public vm = new LinkDisplayVMNew();
  private debug = false;

  constructor(private linkService: LinksService, private evidenceService: EvidenceService, private downloads: DownloadService, private verificationService: VerificationService, private ref: ChangeDetectorRef) { }

  ngOnChanges() {
    if (this.debug) console.log('LinkSummaryComponent ngOnChanges');
    if (this.link.showData) {
      this.showSpinner = true;
      this.getData();
    }
  }

  ngOnInit() {
    if (this.debug) console.log('LinkSummaryComponent ngOnInit');
    // this.showSpinner = true;
    // this.getData();
  }

  ngOnDestroy(): void {
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('LinkSummaryComponent getData');
    this.linkService.getLinkDisplayDetail(this.link)
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as LinkDisplayVMNew;
        } else {
          this.vm = new LinkDisplayVMNew;
        }
        this.showSpinner = false;
        this.ref.markForCheck();
      });
  }
  pdfView(pdf: PdfShare){
    const dr : PdfRequest = {
      requestType: PdfRequestTypeEnum.LinkViewPdf,
      linkId: this.link.id,
      clrId: null,
      assertionId:  pdf.assertionId,
      evidenceName:  pdf.evidenceName,
      artifactId:  pdf.artifactId,
      artifactName:  pdf.artifactName,
      createLink: false,
      accessKey: this.evidenceService.accessKey
    }
    this.message = 'downloading pdf';
    this.showSpinner = true;
      if (this.debug) console.log('LinkSummaryComponent showPdf');
      this.downloads.pdf(dr)
        .pipe(take(1)).subscribe(resp => {
          this.downloads.saveAs(resp.body, pdf.artifactName, 'Pdf');
          this.showSpinner = false;
          this.message = 'loading summary';
          this.ref.markForCheck();
        });

  }
  downloadVCJson(){
    this.message = 'downloading credential';
    this.showSpinner = true;
    if (this.debug) console.log('DisplayCredentialComponent downloadVCJson');
    this.downloads.vcLinkJson(this.link.id, this.link.accessKey)
      .pipe(take(1))
      .subscribe(resp => {
        var contentDispositionHeader = resp.headers.get('Content-Disposition');
        var filename = contentDispositionHeader.split(';')[1].trim().split('=')[1].replace(/"/g, '');
        this.downloads.saveAs(JSON.stringify(resp.body), filename, 'Pdf');
        this.showSpinner = false;
        this.message = 'loading summary';
        this.ref.markForCheck();
      });
  }
}
