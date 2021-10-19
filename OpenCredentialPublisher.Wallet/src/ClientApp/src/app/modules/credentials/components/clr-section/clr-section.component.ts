import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { CredentialService } from '@core/services/credentials.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ClrVM } from '@shared/models/clrVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-clr-section',
  templateUrl: './clr-section.component.html',
  styleUrls: ['./clr-section.component.scss']
})
export class ClrSectionComponent implements OnChanges, OnInit {
  @Input() packageId: number;
  @Input() canDelete: boolean;
  showSpinner = false;
  public clrs = new Array<ClrVM>();
  private debug = false;

  constructor(private credentialService: CredentialService) { }

  ngOnChanges() {
    if (this.debug) console.log('ClrSectionComponent ngOnChanges');
    this.showSpinner = true;
    this.getData();
    if (this.debug) console.log(`ClrSectionComponent ngOnChanges canDelete: ${this.canDelete}`);
  }

  ngOnInit() {
    if (this.debug) console.log('ClrSectionComponent ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }

  collapseToggle(event: any, clr: ClrVM){
    if (event.target.tagName == 'A') return;
    clr.isCollapsed = !clr.isCollapsed;
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('ClrSectionComponent getData');
    this.credentialService.getClrViewModels(this.packageId)
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.clrs = (<ApiOkResponse>data).result as Array<ClrVM>;
        } else {
          this.clrs = new Array<ClrVM>();
        }
        this.showSpinner = false;
      });
  }

  getName(clr: ClrVM) {
    var name = new Array<string>();
    if (clr.name) {
      name.push(clr.name);
    }
    if (clr.publisherName) {
      name.push(clr.publisherName);
    }
    return name.join(' - ');
  }
}
