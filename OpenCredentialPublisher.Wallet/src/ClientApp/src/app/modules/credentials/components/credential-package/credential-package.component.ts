import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { PackageTypeEnum } from '@shared/models/enums/packageTypeEnum';
import { PackageVM } from '@shared/models/packageVM';
import { CredentialService } from '../../../../core/services/credentials.service';

@Component({
  selector: '[app-package]',
  templateUrl: './credential-package.component.html'
})
export class PackageComponent implements OnChanges, OnDestroy, OnInit {
  @Input() package: PackageVM;
  showSpinner = false;
  private debug = false;
  constructor(private credentialService: CredentialService) {
  }
  ngOnChanges() {
    if (this.debug) console.log(this.package.id);
  }
  ngOnInit() {
    if (this.debug) console.log('package ngOnInit');
    if (this.debug) console.log(this.package.id);
  }

  header() {
    let count = this.package.assertionCount;
    let type = "Assertion";
    switch(this.package.typeId) {
      case PackageTypeEnum.OpenBadge:
      case PackageTypeEnum.OpenBadgeConnect:
        type = "Badge";
        break;
    }
    if (count != 1) {
      type += "s";
    }
    return `${count} ${type}`;
  }

  ngOnDestroy(): void {
  }

  public get PackageTypeEnum() {
    return PackageTypeEnum;
  }
}
