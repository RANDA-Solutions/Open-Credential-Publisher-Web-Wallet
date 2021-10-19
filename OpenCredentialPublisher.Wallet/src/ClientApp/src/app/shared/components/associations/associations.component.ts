import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ClrService } from '@core/services/clr.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AssociationVM } from '@shared/models/clrSimplified/associationVM';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-associations]',
  templateUrl: './associations.component.html',
  styleUrls: ['./associations.component.scss']
})
export class AssociationsComponent implements OnInit {
  @Input() clrId: number;
  @Input() clrIdentifier: string;
  @Input() id: string;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  @Output() doAssociationsExist = new EventEmitter<boolean>();
  lineage: string;
  lineageKeys: string;

  associations: Array<AssociationVM>;
  showSpinner = false;
  showIt = false;
  private debug = false;
  constructor(private clrService: ClrService, public utils: UtilsService) { }

  ngOnChanges() {
    if (this.debug) console.log('AssociationsComponent ngOnChanges');
    if (this.id != null && this.clrId != null) {
      if (this.debug) console.log('AssociationsComponent ngOnChanges getting results & descriptions');
      this.getData();
    }

  }
  ngOnInit(): void {
    if (this.debug) console.log('AssociationsComponent ngOnInit');
  }
  getData():any {
    if (this.debug) console.log('AssociationsComponent getData');
    if (this.ancestors.split('.').pop() == 'achievement') {
      this.showSpinner = true;
      this.lineage = this.ancestors;
      this.lineageKeys = this.ancestorKeys;
      this.clrService.getAchievementAssociationVMList(this.clrId, encodeURIComponent(this.id))
        .pipe(take(1)).subscribe(data => {
          if (this.debug) console.log(data);
          if (data.statusCode == 200) {
            this.associations = (<ApiOkResponse>data).result as Array<AssociationVM>;
            this.doAssociationsExist.emit(this.associations.length > 0 );
            if (this.debug) console.log(`AssociationsComponent associations: ${this.associations.length}`);
          } else {
            this.associations = new Array<AssociationVM>();
          }
          this.showSpinner = false;
        });
    }
  }
}


