import { Component, Input, OnInit } from '@angular/core';
import { environment } from '@environment/environment';
import { ProofInvitation } from '@shared/models/proofInvitation';
import { DeviceDetectorService } from 'ngx-device-detector';

@Component({
  selector: 'app-proof-invitation',
  templateUrl: './invitation.component.html',
  styleUrls: ['./invitation.component.scss']
})
export class ProofInvitationComponent implements OnInit {
  @Input() invitation: ProofInvitation;
  showUrl: boolean = false;
  showSpinner = false;
  private debug = false;

  constructor(private deviceDetector: DeviceDetectorService) { }

  ngOnChanges() {
    if (this.debug) console.log('ProofInvitationComponent ngOnChanges');
  }

  ngOnInit() {
    this.showUrl = this.deviceDetector.isMobile() || this.deviceDetector.isTablet();
    if (this.debug) console.log('ProofInvitationComponent ngOnInit');
  }

  ngOnDestroy(): void {
  }
}
