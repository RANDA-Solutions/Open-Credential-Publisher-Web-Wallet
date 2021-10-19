import { Component, Input, OnInit } from '@angular/core';
import { CredentialService } from '@core/services/credentials.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { PackageVM } from '@shared/models/packageVM';
import { VerifiableCredentialVM } from '@shared/models/verifiableCredentialVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-verifiable-credential',
  templateUrl: './verifiable-credential.component.html',
  styleUrls: ['./verifiable-credential.component.scss']
})
export class VerifiableCredentialComponent implements OnInit {
  @Input() package: PackageVM;
  showSpinner = false;
  public vm = new VerifiableCredentialVM();
  private debug = false;

  constructor(private credentialService: CredentialService) { }
  ngOnChanges() {
  }

  ngOnInit(): void {
    if (this.debug) console.log('VerifiableCredentialComponent ngOnInit');
    this.getData();
  }
  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('VerifiableCredentialComponent getData');
    this.credentialService.getPackageVCVM(this.package.id)
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as VerifiableCredentialVM;
          if (this.debug) console.log(`VerifiableCredentialComponent gotData ${JSON.stringify(this.vm)}`);
        } else {
          this.vm = new VerifiableCredentialVM();
          if (this.debug) console.log(`VerifiableCredentialComponent gotData ${data.statusCode}`);
        }
        this.showSpinner = false;
      });
  }
}
