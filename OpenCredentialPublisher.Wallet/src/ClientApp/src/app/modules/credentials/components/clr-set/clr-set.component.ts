import { Component, Input, OnInit } from '@angular/core';
import { CredentialService } from '@core/services/credentials.service';
import { environment } from '@environment/environment';
import { ClrVM } from '@shared/models/clrVM';
import { PackageVM } from '@shared/models/packageVM';

@Component({
  selector: 'app-clr-set',
  templateUrl: './clr-set.component.html',
  styleUrls: ['./clr-set.component.scss']
})
export class ClrSetComponent implements OnInit {
  @Input() package: PackageVM;
  showSpinner = false;
  public clrs = new Array<ClrVM>();
  private debug = false;

  constructor(private credentialService: CredentialService) { }

  ngOnChanges() {
    if (this.debug) console.log('ClrSetComponent ngOnChanges');
  }

  ngOnInit() {
    if (this.debug) console.log('ClrSetComponent ngOnInit');
  }

  ngOnDestroy(): void {
  }
}
