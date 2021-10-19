import { Component, Input, OnInit } from '@angular/core';
import { CredentialService } from '@core/services/credentials.service';
import { environment } from '@environment/environment';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AssertionHeaderVM } from '@shared/models/assertionHeaderVM';
import { take } from 'rxjs/operators';
import { AssertionPopupComponent } from '../assertion-popup/assertion-popup';

@Component({
  selector: 'app-clr-section-assertions',
  templateUrl: './clr-section-assertions.component.html',
  styleUrls: ['./clr-section-assertions.component.scss']
})
export class ClrSectionAssertionsComponent implements OnInit {
  @Input() clrId: number;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  lineage: string;
  lineageKeys: string;
  showSpinner = false;
  clrAssertions = new Array<AssertionHeaderVM>();
  private debug = false;
  constructor(private modalService: NgbModal, private credentialService: CredentialService) { }

  ngOnInit() {
    if (this.debug) console.log('ClrSectionAssertionsComponent ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('ClrSectionAssertionsComponent getData');
    this.credentialService.getClrAssertions(this.clrId)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(' ClrSectionAssertionsComponent data follows');
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.clrAssertions = (<ApiOkResponse>data).result as Array<AssertionHeaderVM>;
          if (this.debug) console.log(this.clrAssertions.length);
        } else {
          this.clrAssertions = new Array<AssertionHeaderVM>();
        }
        this.showSpinner = false;
      });
  }
  popup(assertionId: number){
    const modalRef = this.modalService.open(AssertionPopupComponent);
    modalRef.componentInstance.assertionId = assertionId;
  }
}
