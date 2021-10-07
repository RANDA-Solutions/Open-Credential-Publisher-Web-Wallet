import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AppService } from '@core/services/app.service';
import { ClrService } from '@core/services/clr.service';
import { DownloadService } from '@core/services/download.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ArtifactVM } from '@shared/models/clrSimplified/artifactVM';
import { EvidenceVM } from '@shared/models/clrSimplified/evidenceVM';
import { PdfRequestTypeEnum } from '@shared/models/enums/pdfRequestTypeEnum';
import { PdfRequest } from '@shared/models/pdfRequest';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-evidence]',
  templateUrl: './evidence.component.html',
  styleUrls: ['./evidence.component.scss']
})
export class EvidenceComponent implements OnInit {
  @Input() clrId: number;
  @Input() clrIdentifier: string;
  @Input() id: string;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  @Output() doesEvidenceExist = new EventEmitter<boolean>();
  evidences: Array<EvidenceVM>;
  showSpinner = false;
  message = 'loading evidence';
  showIt = false;
  private debug = false;
  constructor(private appService: AppService, private clrService: ClrService, public utils: UtilsService, private downloads: DownloadService
    , private utilsService: UtilsService) { }

  ngOnChanges() {
    if (this.debug) console.log('EvidenceComponent ngOnChanges');
    if (this.id != null && this.clrId != null) {
      if (this.debug) console.log('EvidenceComponent ngOnChanges getting results & descriptions');
      this.getData();
    }

  }
  ngOnInit(): void {
    if (this.debug) console.log('EvidenceComponent ngOnInit');
  }
  getData():any {
    if (this.debug) console.log('EvidenceComponent getData');
    if (this.ancestors.split('.').pop() == 'assertion') {
      this.showSpinner = true;
      this.clrService.getAssertionEvidenceVMList(this.clrId, encodeURIComponent(this.id))
        .pipe(take(1)).subscribe(data => {
          if (this.debug) console.log(data);
          if (data.statusCode == 200) {
            this.evidences = (<ApiOkResponse>data).result as Array<EvidenceVM>;
            this.doesEvidenceExist.emit(this.evidences.length > 0 );
            if (this.debug) console.log(`EvidenceComponent evidences: ${this.evidences.length}`);
          } else {
            this.evidences = new Array<EvidenceVM>();
          }
          this.showSpinner = false;
        });
    }
  }

  showPdf(evidence: EvidenceVM, artifact: ArtifactVM){
    const dr : PdfRequest = {
      requestType: this.appService.currentUrl.toLowerCase().includes('public/links')
        ?  PdfRequestTypeEnum.LinkViewPdf : PdfRequestTypeEnum.OwnerViewPdf,
      linkId: null,
      clrId: this.clrId,
      assertionId: this.id,
      evidenceName: artifact.evidenceName,
      artifactId: artifact.artifactId,
      artifactName: artifact.name,
      createLink: false
    }
    this.message = 'loading pdf';
    this.showSpinner = true;
      if (this.debug) console.log('EvidenceComponent showPdf');
      this.downloads.pdf(dr)
        .pipe(take(1))
        .subscribe(resp => {
          let blob =  new Blob([resp.body], { type: 'application/pdf' })
          var fileURL = URL.createObjectURL(blob);
          window.open(fileURL);
          this.showSpinner = false;
          this.message = 'loading evidence';
        });
  }
  get safeAssertionId():string {
    return this.utilsService.safeId(this.id);
  }
}
