import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { VerificationService } from '@core/services/verification.service';
import { environment } from '@environment/environment';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { VerificationModel } from '@shared/models/verificationModel';
import { VerificationResult } from '@shared/models/verificationResult';
import { VerifyVM } from '@shared/models/verifyVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-verification',
  templateUrl: './verification.component.html',
  styleUrls: ['./verification.component.scss']
})
export class VerificationComponent implements OnInit {
  @Input() clrId: number;
  @Input() clrIdentifier: string;
  @Input() assertionId: string;
  @Input() endorsementId: string;
  @Input() verification: VerificationModel;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  @ViewChild('tt') tooltip: NgbTooltip;
  @ViewChild('tt2') tooltip2: NgbTooltip;
  @ViewChild('vresult') vresult: ElementRef;
  @ViewChild('vrevoc') vrevoc: NgbTooltip;

  resultId: string;
  labels = "<div>Hosted</div><div>Signed</div>";
  message = '';
  infoImageUrl = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=';
  revocationsMessage = '';
  verificationResult = new VerificationResult();
  showSpinner = false;
  miniSpinner = false;
  private debug = false;

  constructor(private utilsService: UtilsService, private verificationService: VerificationService) { }

  ngOnChanges() {
    this.resultId = this.assertionId || this.endorsementId || this.clrIdentifier;
    if (this.debug) console.log('VerificationComponent ngOnInit');
  }

  ngOnInit(): void {
    if (this.debug) console.log('VerificationComponent ngOnInit');
  }

  get safeResultId():string {
    return this.utilsService.safeId(this.resultId);
  }
  bubbleOpen() {
    if (this.verificationResult.infoBubble){
      this.tooltip2.open();
    }
  }
  verify(){
    this.miniSpinner = true;
    const input = {
      clrId: this.clrId,
      assertionId: this.assertionId,
      clrIdentifier: this.clrIdentifier,
      endorsementId: this.endorsementId,
      verificationDType: this.verification,
      ancestors: this.ancestors,
      ancestorKeys: this.ancestorKeys
    } as VerifyVM;
    this.verificationService.verify(input)
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.verificationResult = (<ApiOkResponse>data).result;
          if (this.verificationResult.infoBubble) {
            this.infoImageUrl = 'assets/images/noun_Info_742307.svg';
            this.tooltip2.open();
          }
        } else {
          this.vresult.nativeElement.setAttribute('class','alert-danger');
          this.verificationResult.message = 'Verification not completed.';
        }
        this.miniSpinner = false;
      });
  }
}
