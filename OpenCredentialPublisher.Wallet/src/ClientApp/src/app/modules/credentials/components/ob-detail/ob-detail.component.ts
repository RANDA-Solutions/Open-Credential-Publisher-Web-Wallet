import { Component, Input, OnInit } from '@angular/core';
import { CredentialService } from '@core/services/credentials.service';
import { environment } from '@environment/environment';
import { BackpackPackage } from '@shared/models/backpackPackage';
import { PackageVM } from '@shared/models/packageVM';

@Component({
  selector: 'app-ob-detail',
  templateUrl: './ob-detail.component.html',
  styleUrls: ['./ob-detail.component.scss']
})
export class OBDetailComponent implements OnInit {
  @Input() package: PackageVM;
  showSpinner = false;
  backpackPackage = new BackpackPackage();
  private debug = false;
  canDelete = true;
  constructor(private credentialService: CredentialService) { }

  ngOnInit() {
    // if (this.debug) console.log('OpenBadgesComponent ngOnInit');
    // this.showSpinner = true;
    // this.getData();
  }

  ngOnDestroy(): void {
  }

  // getData():any {
  //   this.showSpinner = true;
  //   if (this.debug) console.log('OpenBadgesComponent getData');
  //   this.credentialService.getBackpackPackage(this.package.id)
  //     .pipe(take(1)).subscribe(data => {
  //       if (this.debug) console.log(data);
  //       if (data.statusCode == 200) {
  //         this.backpackPackage = (<ApiOkResponse>data).result as BackpackPackage;
  //       } else {
  //         this.backpackPackage = new BackpackPackage();
  //       }
  //       this.showSpinner = false;
  //     });
  // }
}
