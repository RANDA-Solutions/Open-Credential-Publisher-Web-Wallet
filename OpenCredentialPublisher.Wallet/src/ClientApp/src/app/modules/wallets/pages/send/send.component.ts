import { Component, NgZone, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { WalletService } from '@modules/wallets/wallets.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { CredentialSendStatus } from '@shared/models/credentialSendStatus';
import { SendWalletVM } from '@shared/models/sendWalletVM';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-send-wallet',
  templateUrl: './send.component.html',
  styleUrls: ['./send.component.scss']
})
export class SendWalletComponent implements OnDestroy, OnInit {
  @ViewChild('errorModal', { static: false }) private errorModal;
  @ViewChild('revokedModal', { static: false }) private revokedModal;

  id: number;
  redirectUrl: string;
  modelErrors = new Array<string>();
  public vm = new SendWalletVM();
  showSpinner = false;
  private qsSub: any;
  statuses: Array<CredentialSendStatus>;
  currStatus = new CredentialSendStatus( -1, -1, '', false, false, false);
  statSubscription: Subscription;
  private debug = false;
  constructor(public appService: AppService, private walletService: WalletService, private route: ActivatedRoute
    , private router: Router, private modalService: NgbModal
    , private sanitizer:DomSanitizer, private ngZone: NgZone ) {
  }

  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('SendWalletComponent ngOnInit');
    this.qsSub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.id = params['id'];

      this.getData(this.id );
    });
  }

  ngOnDestroy(): void {
    this.appService.killSignalR(this.appService.credentialStatusFlow);
    if (this.statSubscription) {
      this.statSubscription.unsubscribe();
    }
    this.ngZone.run(() => {
      this.modalService.dismissAll();
    },  this);
  }

  getData(id: number):any {
    this.showSpinner = true;
    if (this.debug) console.log('SendWalletComponent getData');
    this.walletService.getSendWalletVM(id)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as SendWalletVM;
        } else {
          this.vm = new SendWalletVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
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
    if (this.debug) console.log('SendWalletComponent opened modal');
  }
  send(modal, id: number) {
    this.statuses = [];
    this.appService.connectSignalR(this.appService.credentialStatusFlow);
    this.statSubscription = this.appService.credentialStatusChanged
      .subscribe(newStatus => {

        let exists = false;
        this.statuses.forEach((currentValue, index) => {
          var found = newStatus.status === currentValue.status;
          if (found) {
            exists = true;
          }
        });
        if (!exists) {
          this.statuses.push(newStatus);
          this.currStatus = newStatus;
          if (this.currStatus.done == true) {
            this.statSubscription.unsubscribe();
            this.appService.killSignalR(this.appService.credentialStatusFlow);
            if (this.currStatus.error == false && this.currStatus.revoked == false) {
              let lastStatus = new CredentialSendStatus(
                this.currStatus.relationshipId, 999, 'Credential sent successfully', this.currStatus.done, false, false
              );
              this.currStatus = lastStatus;
              this.statuses.push(lastStatus);
            }
            this.ngZone.run(() => {
              setTimeout(() => {
                this.modalService.dismissAll();
              }, 2000);
            },  this);
          }
        }

        if (this.debug) console.log(`SendWalletComponent currStatus: ${this.currStatus.status} done: ${this.currStatus.done}`);

        //NB currstatus is NOT the last one we sent above
        if (this.currStatus.error) {
          this.open(this.errorModal);
        }
        if (this.currStatus.revoked) {
          this.open(this.revokedModal);
        }
      });
    this.walletService.send(this.id, id)
    .pipe(take(1)).subscribe(data => {
      if (this.debug) console.log(data);
      if (data.statusCode == 200) {
        if (this.debug) console.log('SendWalletComponent connect good statusCode');
        this.open(modal);
      } else {
        if (this.debug) console.log('SendWalletComponent connect bad statusCode');
        this.modelErrors = (<ApiBadRequestResponse>data).errors;
      }
      this.showSpinner = false;
    });
  }
  refresh() {
    this.showSpinner = true;
    this.walletService.getSendWalletVM(this.id)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          var vm = (<ApiOkResponse>data).result as SendWalletVM;
          this.vm.credentials = vm.credentials;
        } else {
          this.vm = new SendWalletVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
}
