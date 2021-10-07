import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ClrService } from '@core/services/clr.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AlignmentVM } from '@shared/models/clrSimplified/alignmentVM';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-alignments]',
  templateUrl: './alignments.component.html',
  styleUrls: ['./alignments.component.scss']
})
export class AlignmentsComponent implements OnInit {
  @Input() clrId: number;
  @Input() clrIdentifier: string;
  @Input() id: string;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  @Output() doAlignmentsExist = new EventEmitter<boolean>();

  alignments: Array<AlignmentVM>;
  showSpinner = false;
  private debug = false;
  constructor(private clrService: ClrService, public utils: UtilsService) { }

  ngOnChanges() {
    if (this.debug) console.log('AlignmentsComponent ngOnChanges');
    if (this.id != null && this.clrId != null) {
      if (this.debug) console.log('AlignmentsComponent ngOnChanges getting results & descriptions');
      this.getData();
    }

  }
  ngOnInit(): void {
    if (this.debug) console.log('AlignmentsComponent ngOnInit');
  }
  getData():any {
    if (this.debug) console.log(`AlignmentsComponent getData ${this.ancestors} ${this.ancestorKeys}`);
    if (this.ancestors.split('.').pop() == 'achievement') {
      this.showSpinner = true;
      if (this.ancestors.split('.')[this.ancestors.split('.').length-2] == 'assertion') {
        this.clrService.getAssertionAchievementAlignmentVMList(this.clrId, Number(this.ancestorKeys.split('.')[this.ancestorKeys.split('.').length-2]), encodeURIComponent(this.id))
          .pipe(take(1)).subscribe(data => {
            if (this.debug) console.log(data);
            if (data.statusCode == 200) {
              this.alignments = (<ApiOkResponse>data).result as Array<AlignmentVM>;
              this.doAlignmentsExist.emit(this.alignments.length > 0 );
              if (this.debug) console.log(`AlignmentsComponent alignments: ${this.alignments.length}`);
            } else {
              this.alignments = new Array<AlignmentVM>();
            }
            this.showSpinner = false;
          });
      } else if (this.ancestors.split('.')[this.ancestors.split('.').length-2] == 'clr') {
        this.clrService.getAchievementAlignmentVMList(this.clrId, encodeURIComponent(this.id))
        .pipe(take(1)).subscribe(data => {
          if (this.debug) console.log(data);
          if (data.statusCode == 200) {
            this.alignments = (<ApiOkResponse>data).result as Array<AlignmentVM>;
            if (this.debug) console.log(`AlignmentsComponent alignments: ${this.alignments.length}`);
          } else {
            this.alignments = new Array<AlignmentVM>();
          }
          this.showSpinner = false;
        });
      }
    }
  }
}

