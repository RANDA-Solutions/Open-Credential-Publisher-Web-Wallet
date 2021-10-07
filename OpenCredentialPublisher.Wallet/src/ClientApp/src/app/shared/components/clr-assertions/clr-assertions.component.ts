import { Component, Input, OnInit } from '@angular/core';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AssertionHeaderVM } from '@shared/models/assertionHeaderVM';
import { take } from 'rxjs/operators';
// Note - this level downward is set up based on furture persistence model where
//        assertions etc. will be stored seperately in the db
@Component({
  selector: 'app-clr-assertions',
  templateUrl: './clr-assertions.component.html',
  styleUrls: ['./clr-assertions.component.scss']
})
export class ClrAssertionsComponent implements OnInit {
  @Input() clrId: number;
  @Input() clrIdentifier: string;
  @Input() assertionId: number;
  @Input() isChild: boolean;
  @Input() isShare: boolean;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  assertionHeaderVMs = new Array<AssertionHeaderVM>();
  message = 'loading assertions';
  showSpinner = false;


  private debug = false;

  constructor(private clrDetailService: ClrDetailService, public utils: UtilsService) { }

  ngOnChanges() {
    if (this.clrId > 0) {
      this.showSpinner = true;
      if (this.debug) console.log(`ClrAssertionsComponent ngOnChanges isChild: ${this.isChild} assertionId: ${this.assertionId}`);
      this.getData();
    }
  }

  ngOnInit(): void {
    if (this.debug) console.log('ClrAssertionsComponent ngOnInit');
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('ClrAssertionsComponent getData');
    if (this.isChild == false){
      this.clrDetailService.getParentAssertions(this.clrId, this.isShare)
        .pipe(take(1)).subscribe(data => {
          if (data.statusCode == 200) {
            this.assertionHeaderVMs = (<ApiOkResponse>data).result as Array<AssertionHeaderVM>;
            this.showSpinner = false;
          } else {
            this.assertionHeaderVMs = new Array<AssertionHeaderVM>();
            this.showSpinner = false;
          }
        });
    } else {
      this.clrDetailService.getChildAssertions(this.clrId, this.assertionId, this.isShare)
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.assertionHeaderVMs = (<ApiOkResponse>data).result as Array<AssertionHeaderVM>;
          this.showSpinner = false;
        } else {
          this.assertionHeaderVMs = new Array<AssertionHeaderVM>();
          this.showSpinner = false;
        }
      });
    }
  }
}
