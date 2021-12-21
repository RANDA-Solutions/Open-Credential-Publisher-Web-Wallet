import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CredentialService } from '@core/services/credentials.service';
import { AppService } from '@core/services/app.service';
import { environment } from '@environment/environment';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { BackpackPackage } from '@shared/models/backpackPackage';
import { OpenBadge } from '@shared/models/openBadge';
import { PackageVM } from '@shared/models/packageVM';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-open-badges]',
  templateUrl: './open-badges.component.html',
  styleUrls: ['./open-badges.component.scss']
})
export class OpenBadgesComponent implements OnInit {
  @Input() package: PackageVM;
  @Input() canDelete: boolean;
  message = 'loading badges';
  showSpinner = false;
  backpackPackage = new BackpackPackage();
  modelErrors = new Array<string>();
  private debug = false;

  constructor(private route: ActivatedRoute, private router: Router, private credentialService: CredentialService, private appService: AppService) { }

  ngOnInit() {
    if (this.debug) console.log('OpenBadgesComponent ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }
  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('OpenBadgesComponent getData');
    this.credentialService.getBackpackPackage(this.package.id)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.backpackPackage = (<ApiOkResponse>data).result as BackpackPackage;
        } else {
          this.backpackPackage = new BackpackPackage();
        }
        this.showSpinner = false;
      });
  }

  embedClr(badge: OpenBadge):any {
    this.message = 'creating CLR';
    this.showSpinner = true;
    if (this.debug) console.log('DeleteCredentialComponent deletePackage');
    this.credentialService.clrEmbed(badge.id)
      .pipe(take(1)).subscribe(data => {
        this.message = 'loading badges';
        if (data.statusCode == 200) {
          window.location.href = this.appService.currentUrl;
        } else {
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
}
