import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { WalletService } from '@modules/wallets/wallets.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ConnectionViewModel } from '@shared/models/connectionViewModel';
import { InvitationVM } from '@shared/models/invitationVM';
import { WalletConnectionStatus } from '@shared/models/walletConnectionStatus';
import { DeviceDetectorService } from 'ngx-device-detector';
import { take } from 'rxjs/operators';


@UntilDestroy()
@Component({
  selector: 'app-invitation',
  templateUrl: './invitation.component.html',
  styleUrls: ['./invitation.component.scss']
})
export class InvitationComponent implements OnInit,  OnDestroy {
  id: number;
  redirectUrl: string;
  modelErrors = new Array<string>();
  statuses: Array<WalletConnectionStatus>;
  currStatus = new WalletConnectionStatus(-1, '', null);
  showNameInput: boolean = false;
  showConnecting: boolean = false;
  showUrl: boolean = false;
  public vm = new InvitationVM();
  showSpinner = false;
  private sub: any;
  private debug = false;
  constructor(private walletService: WalletService
    , private route: ActivatedRoute
    , private router: Router
    , private detectorService: DeviceDetectorService
    , private modalService: NgbModal
    , private appService: AppService
    , private zone: NgZone
    , private sanitizer:DomSanitizer ) {
  }

  ngOnInit() {
    this.showSpinner = true;
    this.showUrl = this.detectorService.isMobile() || this.detectorService.isTablet();
    if (this.debug) console.log('InvitationComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.id = params['id'];

      this.getData(this.id );
    });
    this.statuses = [];
    this.appService.completeInvitationStatusChanged.pipe(untilDestroyed(this))
      .subscribe(data => {
        this.currStatus = data;
        if (this.currStatus.status == "Invitation Accepted") {
          this.showConnecting = true;
        }
        if (this.debug) console.log(`InvitationComponent currStatus: ${this.currStatus.status}`);
        if (this.currStatus.done) {
          this.showConnecting = false;
          this.showNameInput = true;
          this.vm.hideQRCode = true;
          this.appService.killSignalR(this.appService.completeInvitationFlow);
        }
      });
    this.appService.connectSignalR(this.appService.completeInvitationFlow);

  }



  ngOnDestroy() {
    this.appService.killSignalR(this.appService.completeInvitationFlow);
  }

  save():any {
    this.showSpinner = true;
    if (this.debug) console.log('InvitationComponent deleteConnection');
    let connection = new ConnectionViewModel();
    connection.name = this.vm.nickname;
    connection.id = this.vm.id;
    this.walletService.saveConnection(this.id, connection)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/wallets/wallet-list'])
        } else {
          this.vm = new InvitationVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
  getData(id: number, ctx: any = null):any {
    let self = ctx ?? this;
    self.showSpinner = true;
    if (self.debug) console.log('InvitationComponent getData');
    self.walletService.getInvitationVM(id)
      .pipe(take(1)).subscribe(data => {
        if (self.debug) console.log(data);
        if (data.statusCode == 200) {
          self.vm = (<ApiOkResponse>data).result as InvitationVM;
          if (self.vm.hideQRCode)
          self.showNameInput = true;
        } else {
          self.vm = new InvitationVM();
          self.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        self.showSpinner = false;
      });
  }
  //Call this method in the image source, it will sanitize it.
  imageTransform(){
    var uri = `data:image/png;base64,${this.vm.qrCodeString}`;
    if (this.debug) console.log(`InvitationComponent imageTransform: ${uri}`);
    return this.sanitizer.bypassSecurityTrustResourceUrl(uri);
  }
  refresh() {
    this.zone.run(() => {
      let self = this;
      self.showSpinner = true;
      setTimeout(() => {
        this.getData(self.id, self);
      }, 1000);
    }, this);
  }
}
