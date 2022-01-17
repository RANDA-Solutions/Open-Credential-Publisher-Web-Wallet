import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { WalletConnectionStatus } from '@shared/models/walletConnectionStatus';
import { WalletVM } from '@shared/models/walletVM';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { WalletService } from '../wallets.service';

@UntilDestroy()
@Component({
  selector: 'app-wallet-list-component',
  templateUrl: './wallet-list.component.html'
})
export class WalletListComponent  implements OnDestroy, OnInit {
  relationships = new Array<WalletVM>();
  modelErrors = new Array<string>();
  showSpinner = false;
  statuses: Array<WalletConnectionStatus>;
  currStatus = new WalletConnectionStatus(-1, '', null);
  statSubscription: Subscription;
  message = 'loading wallets';
  private debug = false;

  constructor(public appService: AppService, private walletService: WalletService, private router: Router
    , private modalService: NgbModal, private ngZone: NgZone ) {
  }
  ngOnInit() {
    if (this.debug) console.log('WalletListComponent ngOnInit');
    this.showSpinner = true;
    this.getData();
    this.statuses = [];
  }
  ngOnDestroy(): void {
    this.appService.killSignalR(this.appService.generateInvitationFlow);
    if (this.statSubscription) {
      if (this.debug) console.log("calling unsubscribe on statSubscription");
      this.statSubscription.unsubscribe();
    }
  }
  getData():any {
    this.message = 'refreshing wallets';
    this.showSpinner = true;
    if (this.debug) console.log('WalletListComponent getData');
    this.walletService.getWallets()
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.relationships = (<ApiOkResponse>data).result as Array<WalletVM>;
        } else {
          this.relationships = new Array<WalletVM>();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.message = 'loading wallets';
        this.showSpinner = false;
      });
  }
  open(content) {
    let ngbModalOptions: NgbModalOptions = {
      backdrop : 'static',
      keyboard : false,
      ariaLabelledBy: 'modal-basic-title'
    };
    this.modalService.open(content, ngbModalOptions);
    if (this.debug) console.log('WalletListComponent opened modal');
  }

  connect(content) {
    this.statuses = [];
    this.appService.connectSignalR(this.appService.generateInvitationFlow);
    this.statSubscription = this.appService.generateInvitationStatusChanged
      .subscribe(data => {
        this.statuses.push(data);
        this.currStatus = data;
        if (this.debug) console.log(`WalletListComponent currStatus: ${this.currStatus.status}`);
        if (this.currStatus.done) {
          this.statSubscription.unsubscribe();
          let redirectStatus = new WalletConnectionStatus(this.currStatus.relationshipId, 'Redirecting To Invitation', this.currStatus.done);
          this.currStatus = redirectStatus;
          this.statuses.push(redirectStatus);
          this.appService.killSignalR(this.appService.generateInvitationFlow);
          this.ngZone.run((relationshipId) => {
            setTimeout(() => {
              this.modalService.dismissAll();
              this.router.navigate(['wallets/invitation', relationshipId])
            }, 2000);
          }, this, [this.currStatus.relationshipId]);

        }
      });
    this.walletService.connect()
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          if (this.debug) console.log('WalletListComponent connect good statusCode');
          this.open(content);
        } else {
          if (this.debug) console.log('WalletListComponent connect bad statusCode');
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
    });
  }
  refresh() {

    this.showSpinner = true;
    this.getData();
  }
}
