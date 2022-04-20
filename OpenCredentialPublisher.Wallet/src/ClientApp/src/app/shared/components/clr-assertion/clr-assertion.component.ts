import { ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { Component, Input, OnInit } from '@angular/core';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { EndorsementDType } from '@shared/models/clrLibrary/endorsementDType';
import { AssertionWithAchievementVM } from '@shared/models/clrSimplified/assertionWithAchievementVM';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-clr-assertion]',
  templateUrl: './clr-assertion.component.html',
  styleUrls: ['./clr-assertion.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush //NB: this affects ALL CHILDREN components
})
export class ClrAssertionComponent implements OnInit {
  @Input() clrId = -1;
  @Input() clrIdentifier = '';
  @Input() id = '';
  @Input() displayName = '';
  @Input() childAssertion = new AssertionWithAchievementVM();
  @Input() isChild: boolean;
  @Input() ancestors = '';
  @Input() ancestorKeys = '';
  isCollapsed = true;
  isLoaded = false;
  lineage: string;
  lineageKeys: string;
  assertion = new AssertionWithAchievementVM();
  showIt = false;
  endorsements = new Array<EndorsementDType>();
  message = 'loading assertion';

  //child component display flags
  alignmentsExist = true;
  associationsExist = true;
  evidenceExists = true;
  endorsementsExist = true;
  resultsExist = true;

  private debug = false;

  constructor(private clrDetailService: ClrDetailService, private utilsService: UtilsService, private ref: ChangeDetectorRef) { }

  ngOnChanges() {
    if (this.debug) console.log(`ClrAssertionComponent ngOnChanges  isChild: ${this.isChild} clrId: ${this.clrId} childAssertion: ${this.childAssertion.id}`);
    if (this.isChild != null && this.clrId != null){
      if (this.childAssertion != null) {
        this.assertion = this.childAssertion;
        this.showIt = true;
      } else {
        if (this.debug) console.log('ClrAssertionComponent ngOnChanges not enough input yet');
      }
    } else {
      if (this.debug) console.log('ClrAssertionComponent ngOnChanges no input yet');
    }
  }

  ngOnInit(): void {
    if (this.debug) console.log('ClrAssertionComponent ngOnInit');
  }
  get safeAssertionId():string {
    if (this.debug) {
      if (this.id == null || this.id == undefined) {
        console.log(`ClrAssertionComponent safeAssertionId id:${this.id}`);
      }
    }
    return this.utilsService.safeId(this.id);
  }
  exportToView(image: string){
    const dataWindow = window.open("");
        dataWindow.document.write(
            `<iframe width='100%' height='100%' src='${image}'></iframe>`);
  }
  getData():any {
    if (this.debug) console.log(`clrId - ${this.clrId}`);
    if (this.debug) console.log(`clrIdfr - ${this.clrIdentifier}`);
    if (this.debug) console.log('ClrAssertionComponent getData');
    this.clrDetailService.getClrAssertion(this.clrId, encodeURIComponent(this.id))
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.assertion = (<ApiOkResponse>data).result as AssertionWithAchievementVM;
          this.lineage = `${this.ancestors}.assertion`;
          this.lineageKeys = `${this.ancestorKeys}.${this.assertion.assertionId}`;
          if (this.debug) console.log(`ClrAssertionComponent this.isLoaded = true;`);
          this.isLoaded = true;
          this.ref.markForCheck();
        } else {
          this.assertion = new AssertionWithAchievementVM();
          this.lineage = `${this.ancestors}.assertion`;
          this.lineageKeys = `${this.ancestorKeys}.-1`;
          if (this.debug) console.log(`ClrAssertionComponent this.isLoaded = true;`);
          this.isLoaded = true;
          this.ref.markForCheck();
        }
      });
  }
  onAlignmentsExist(alignmentsExist: boolean) {
    this.alignmentsExist = alignmentsExist;
  }
  onAssociationsExist(associationsExist: boolean) {
    this.associationsExist = associationsExist;
  }
  onEvidenceExists(evidenceExists: boolean) {
    this.evidenceExists = evidenceExists;
  }
  onEndorsementsExist(endorsementsExist: boolean) {
    this.endorsementsExist = endorsementsExist;
  }
  onResultsExist(resultsExist: boolean) {
    this.resultsExist = resultsExist;
  }
  toggleCollapse() {
    this.isCollapsed = !this.isCollapsed;
    if (!this.isCollapsed && !this.isLoaded) {
      this.getData();
    }
  }
}
