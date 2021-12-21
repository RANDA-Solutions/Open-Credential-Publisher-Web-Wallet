import { Component, OnDestroy, OnInit } from '@angular/core';
import { CredentialFilterService } from '@modules/credentials/services/credentialFilter.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { PackageTypeEnum } from '@shared/models/enums/packageTypeEnum';
import { PackageList } from '@shared/models/packageList';
import { PackageVM } from '@shared/models/packageVM';
import { take } from 'rxjs/operators';
import { CredentialService } from '../../../../core/services/credentials.service';

@Component({
  selector: '[app-packages]',
  templateUrl: './credential-packages.component.html'
})
export class PackagesComponent implements OnDestroy, OnInit {
  packages: PackageVM[] = new Array<PackageVM>();
  packageList = new PackageList();
  private localStorageFilterKey = "credentialpackagefilter";
  modelErrors = new Array<string>();
  filtering = "all";
  showSpinner = false;
  private debug = false;
  constructor(private credentialService: CredentialService, private credentialFilterService: CredentialFilterService) {
  }

  ngOnInit() {
    if (this.debug) console.log('packages ngOnInit');
    this.showSpinner = true;
    let storedFilter = this.credentialFilterService.getFilter();
    if (storedFilter)
      this.filtering = storedFilter;
    this.getData();
  }

  filter(val) {
    if (this.filtering != val) {
      this.filtering = val;
      this.credentialFilterService.setFilter(val);
      this.filterPackages();
    }
  }

  refresh() {
    this.getData();
  }

  ngOnDestroy(): void {
  }
  // cardSectionHeaderClicked (event){
  //   if (event.target.tagName === 'A') return;
  //   $(".expand-button-icon", this).toggleClass("collapsed");
  //   $(this).next(".card-section-body").toggleClass("collapsed");
  //   var item = $($(this).children().children().children("span")[0].firstChild);
  //   item.toggleClass("fa-chevron-circle-right", "fa-chevron-circle-down");
  //   item.toggleClass("fa-chevron-circle-down", "fa-chevron-circle-right");
  // }
  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('packages getData');
    this.credentialService.getPackageList()
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.packageList = (<ApiOkResponse>data).result as PackageList;
          this.filterPackages();
        } else {
          this.packageList = new PackageList();
          this.packages = new Array<PackageVM>();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }

  filterPackages() {
    if (this.filtering == 'clrs') {
        this.packages = this.packageList.packages.filter((pkg) => { return pkg.typeId == PackageTypeEnum.Clr || pkg.typeId == PackageTypeEnum.ClrSet});
    }
    else if (this.filtering == 'badges') {
      this.packages = this.packageList.packages.filter((pkg) => { return pkg.typeId == PackageTypeEnum.OpenBadge || pkg.typeId == PackageTypeEnum.OpenBadgeConnect});
    }
    else if (this.filtering == 'collection') {
      this.packages = this.packageList.packages.filter((pkg) => { return pkg.typeId == PackageTypeEnum.Collection});
    }
    else if (this.filtering == 'vc') {
      this.packages = this.packageList.packages.filter((pkg) => { return pkg.typeId == PackageTypeEnum.VerifiableCredential});
    }
    else {
      this.packages = this.packageList.packages;
    }
  }
}
