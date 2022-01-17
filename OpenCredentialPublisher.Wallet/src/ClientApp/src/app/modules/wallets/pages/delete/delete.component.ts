import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WalletService } from '@modules/wallets/wallets.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { RelationshipVM } from '@shared/models/relationshipVM';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-delete-relationship',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.scss']
})
export class DeleteWalletComponent implements OnInit {
  id: number;
  redirectUrl: string;
  modelErrors = new Array<string>();
  public relationshipVM = new RelationshipVM();
  showSpinner = false;
  private sub: any;
  private debug = false;
  constructor(private walletService: WalletService, private route: ActivatedRoute, private router: Router
    , private modalService: NgbModal ) {
  }

  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('RelationshipDeleteComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.id = params['id'];

      this.getData(this.id );
   });
  }

  delete():any {
    this.showSpinner = true;
    if (this.debug) console.log('RelationshipDeleteComponent deleteConnection');
    this.walletService.delete(this.id)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/wallets/wallet-list'])
        } else {
          this.relationshipVM = new RelationshipVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }

  getData(id: number):any {
    this.showSpinner = true;
    if (this.debug) console.log('RelationshipDeleteComponent getData');
    this.walletService.getRelationshipVM(id)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.relationshipVM = (<ApiOkResponse>data).result as RelationshipVM;
        } else {
          this.relationshipVM = new RelationshipVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }

}
