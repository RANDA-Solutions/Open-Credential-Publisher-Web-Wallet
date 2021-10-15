import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { VerificationService } from '@core/services/verification.service';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { VerificationResult } from '@shared/models/verificationResult';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-vc-verification',
  templateUrl: './vc-verification.component.html',
  styleUrls: ['./vc-verification.component.scss']
})
export class VCVerificationComponent implements OnInit {
  @Input() pkgId: number;
  @ViewChild('vcproofresult') vcproofresult: ElementRef;

  resultId: string;
  labels = "<div>Hosted</div><div>Signed</div>";
  message = '';
  revocationsMessage = '';
  proofResult = new VerificationResult();
  showSpinner = false;
  private debug = false;

  constructor(private utilsService: UtilsService, private verificationService: VerificationService) { }

  ngOnChanges() {
  }

  ngOnInit(): void {
    if (this.debug) console.log('VCVerificationComponent ngOnInit');
  }

  get safeResultId():string {
    return this.utilsService.safeId(this.resultId);
  }
  verify(){
    this.showSpinner = true;
    this.verificationService.verifyVC(this.pkgId)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.proofResult = (<ApiOkResponse>data).result as VerificationResult;
        } else {
          this.proofResult = (<ApiOkResponse>data).result as VerificationResult;
          this.vcproofresult.nativeElement.setAttribute('class','alert-danger');
        }
        this.showSpinner = false;
      });
  }
}
