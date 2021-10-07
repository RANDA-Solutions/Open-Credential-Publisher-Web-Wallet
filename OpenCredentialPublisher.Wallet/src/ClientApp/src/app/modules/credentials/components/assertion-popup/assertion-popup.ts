import { Component, Input, OnInit } from '@angular/core';
import { CredentialService } from '@core/services/credentials.service';
import { environment } from '@environment/environment';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AssertionDetailVM } from '@shared/models/asserrtionDetailVM';
import { ClrAssertionVM } from '@shared/models/clrAsserrtionVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-assertion-popup',
  templateUrl: './assertion-popup.component.html',
  styleUrls: ['./assertion-popup.component.scss']
})
export class AssertionPopupComponent implements OnInit {
  @Input() assertionId: number;
  showSpinner = false;
  asrt = new AssertionDetailVM();
  private debug = false;
  constructor(public activeModal: NgbActiveModal, private credentialService: CredentialService) { }

  ngOnInit() {
    if (this.debug) console.log('AssertionPopupComponent ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }

  collapseToggle(asrt: ClrAssertionVM){
    asrt.isCollapsed = !asrt.isCollapsed;
  }
  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('AssertionPopupComponent getData');
    this.credentialService.getAssertionDetail(this.assertionId)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(' AssertionPopupComponent data follows');
        console.log(data);
        if (data.statusCode == 200) {
          this.asrt = (<ApiOkResponse>data).result as AssertionDetailVM;
        } else {
          this.asrt = new AssertionDetailVM();
        }
        this.showSpinner = false;
      });
  }
  // verify(){
  //   this.showSpinner = true;
  //   this.verificationService.verifyVC(this.pkgId)
  //     .pipe(take(1)).subscribe(data => {
  //       console.log(data);
  //       if (data.statusCode == 200) {
  //         this.proofResult = (<ApiOkResponse>data).result as VerificationResult;
  //       } else {
  //         this.proofResult = (<ApiOkResponse>data).result as VerificationResult;
  //         this.vcproofresult.nativeElement.setAttribute('class','alert-danger');
  //       }
  //       this.showSpinner = false;
  //     });
  // }
}
