import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizationService } from '@core/services/authorization.service';
import { environment } from '@environment/environment';
import { WalletService } from '@modules/wallets/wallets.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ConnectionViewModel } from '@shared/models/connectionViewModel';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-edit-wallet',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditWalletComponent implements OnInit {
  id: number;
  redirectUrl: string;
  modelErrors = new Array<string>();
  public vm = new ConnectionViewModel();
  showSpinner = false;
  private sub: any;
  private debug = false;
  constructor(private walletService: WalletService, private route: ActivatedRoute, private router: Router
    , private modalService: NgbModal, private authService: AuthorizationService
    , private sanitizer:DomSanitizer ) {
  }

  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('EditWalletComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.id = params['id'];
      this.getData(this.id );
   });
  }

  save():any {
    this.showSpinner = true;
    this.walletService.saveConnection(this.id, this.vm)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/wallets/wallet-list'])
        } else {
          this.vm = new ConnectionViewModel();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
  getData(id: number):any {
    this.showSpinner = true;
    this.walletService.getConnectionVM(id)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as ConnectionViewModel;
        } else {
          this.vm = new ConnectionViewModel();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
}
