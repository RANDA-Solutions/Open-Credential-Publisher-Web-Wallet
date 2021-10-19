import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ClrService } from '@core/services/clr.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { EndorsementVM } from '@shared/models/clrSimplified/endorsementVM';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-endorsements]',
  templateUrl: './endorsements.component.html',
  styleUrls: ['./endorsements.component.scss']
})
export class EndorsementsComponent implements OnInit {
  @Input() clrId: number;
  @Input() clrIdentifier: string;
  @Input() id: string;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  @Output() doEndorsementsExist = new EventEmitter<boolean>();
  lineage: string;
  lineageKeys: string;

  endorsements: Array<EndorsementVM>;
  showSpinner = false;
  showIt = false;
  private debug = false;
  constructor(private clrService: ClrService, public utils: UtilsService) { }

  ngOnChanges() {
    if (this.debug) console.log('EndorsementsComponent ngOnChanges');
    if (this.id != null && this.clrId != null) {
      if (this.debug) console.log('EndorsementsComponent ngOnChanges getting results & descriptions');
      this.getData();
    }

  }
  ngOnInit(): void {
    if (this.debug) console.log('EndorsementsComponent ngOnInit');
  }
  getData():any {
    if (this.debug) console.log('EndorsementsComponent getData');
    if (this.ancestors.split('.').pop() == 'assertion') {
      this.showSpinner = true;
      this.clrService.getAssertionEndorsementVMList(this.clrId, encodeURIComponent(this.id))
        .pipe(take(1)).subscribe(data => {
          if (this.debug) console.log(data);
          if (data.statusCode == 200) {
            this.endorsements = (<ApiOkResponse>data).result as Array<EndorsementVM>;
            this.doEndorsementsExist.emit(this.endorsements.length > 0 );
            this.lineage = `${this.ancestors}.endorsement`;
            this.lineageKeys = `${this.ancestorKeys}.${this.id}`;
            if (this.debug) console.log(`EndorsementsComponent endorsements: ${this.endorsements.length}`);
          } else {
            this.endorsements = new Array<EndorsementVM>();
            this.lineage = `${this.ancestors}.endorsement`;
            this.lineageKeys = `${this.ancestorKeys}.${this.id}`;
          }
          this.showSpinner = false;
        });
    }
    if (this.ancestors.split('.').pop() == 'achievement') {
      this.showSpinner = true;
      this.clrService.getAchievementEndorsementVMList(this.clrId, encodeURIComponent(this.id))
        .pipe(take(1)).subscribe(data => {
          if (this.debug) console.log(data);
          if (data.statusCode == 200) {
            this.endorsements = (<ApiOkResponse>data).result as Array<EndorsementVM>;
            this.lineage = `${this.ancestors}.endorsement`;
            if (this.debug) console.log(`EndorsementsComponent endorsements: ${this.endorsements.length}`);
          } else {
            this.endorsements = new Array<EndorsementVM>();
            this.lineage = `${this.ancestors}.endorsement`;
          }
          this.showSpinner = false;
        });
    }
  }
}
