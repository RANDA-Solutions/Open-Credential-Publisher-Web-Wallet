import { Component, Input, OnInit } from '@angular/core';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { environment } from '@environment/environment';
import { ProfileVM } from '@shared/models/clrSimplified/profileVM';

@Component({
  selector: '[app-clr-profile]',
  templateUrl: './clr-profile.component.html',
  styleUrls: ['./clr-profile.component.scss']
})
export class ClrProfileComponent implements OnInit {
  @Input() profile: ProfileVM;
  @Input() profileType: string;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  lineage: string;
  lineageKeys: string;
  showSpinner = false;
  private debug = false;

  // TODO when LearnerId == PublisherId (self published)

  constructor(private clrDetailService: ClrDetailService) { }

  ngOnChanges() {
    this.lineage = `${this.ancestors}.profile`;
    this.lineageKeys = `${this.ancestorKeys}.${this.profile.profileId}`;
  }

  ngOnInit(): void {
    if (this.debug) console.log('ClrProfileComponent ngOnInit');
  }
}
