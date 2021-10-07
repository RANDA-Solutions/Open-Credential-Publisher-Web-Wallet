import { Component, Input, OnInit } from '@angular/core';
import { CredentialService } from '@core/services/credentials.service';
import { environment } from '@environment/environment';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AssertionHeaderVM } from '@shared/models/assertionHeaderVM';
import { ClrAssertionVM } from '@shared/models/clrAsserrtionVM';
import { take } from 'rxjs/operators';
import { AssertionPopupComponent } from '../assertion-popup/assertion-popup';

@Component({
  selector: 'app-clr-section-assertion',
  templateUrl: './clr-section-assertion.component.html',
  styleUrls: ['./clr-section-assertion.component.scss']
})
export class ClrSectionAssertionComponent implements OnInit {
  @Input() clrId: number;
  @Input() assertionHeader: AssertionHeaderVM;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  lineage: string;
  lineageKeys: string;
  showSpinner = false;
  isCollapsed = true;
  isLoaded = false;
  clrAssertion = new ClrAssertionVM();
  private debug = false;
  constructor(private modalService: NgbModal, private credentialService: CredentialService) { }

  ngOnInit() {
    if (this.debug) console.log('ClrSectionAssertionComponent ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }

  collapseToggle(){
    this.isCollapsed = !this.isCollapsed;
    if (!this.isCollapsed && !this.isLoaded) {
      this.getData();
    }
  }
  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('ClrSectionAssertionComponent getData');
    this.credentialService.getClrAssertion(this.clrId, encodeURIComponent(this.assertionHeader.id))
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(' ClrSectionAssertionComponent data follows');
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.clrAssertion = (<ApiOkResponse>data).result as ClrAssertionVM;
        } else {
          this.clrAssertion = new ClrAssertionVM();
        }
        this.showSpinner = false;
        this.isLoaded = true;
      });
  }
  popup(assertionId: number){
    const modalRef = this.modalService.open(AssertionPopupComponent);
    modalRef.componentInstance.assertionId = assertionId;
  }
}
