import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { environment } from '@environment/environment';
import { EndorsementProfileDType } from '@shared/models/clrLibrary/endorsementProfileDType';

@Component({
  selector: '[app-endorsement-profile]',
  templateUrl: './endorsement-profile.component.html'
})
export class EndorsementProfileComponent implements OnDestroy, OnInit {
  @Input() endorsementProfile: EndorsementProfileDType;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  lineage: string;
  lineageKeys: string;
  showSpinner = false;
   private debug = false;

  constructor() {
  }
  ngOnChanges() {
    this.lineage = `${this.ancestors}.profile`;
    this.lineageKeys = `${this.ancestorKeys}.${this.endorsementProfile.id}`;

  }
  ngOnInit() {
    if (this.debug) console.log('EndorsementProfileComponent ngOnInit');
  }

  ngOnDestroy(): void {
  }
}
