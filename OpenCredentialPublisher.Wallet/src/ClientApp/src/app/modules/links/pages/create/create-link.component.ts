import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CredentialService } from '@core/services/credentials.service';
import { LinksService } from '@modules/links/links.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ClrLinkVM } from '@shared/models/clrLinkVM';
import { ClrVM } from '@shared/models/clrVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-create-link-component',
  templateUrl: './create-link.component.html'
})
export class CreateLinkComponent  implements OnInit{
  redirectUrl = '';
  clrLinks = new Array<ClrLinkVM>();
  showSpinner = false;
  modelErrors = new Array<string>();

  constructor(private route: ActivatedRoute, private router: Router, private linksService: LinksService) {
  }
  ngOnInit() {
    this.showSpinner = true;
    this.getData();
  }
  create(clrLink: ClrLinkVM) {
    this.linksService.create(clrLink)
    .pipe(take(1)).subscribe(data => {
      console.log(data);
      if (data.statusCode == 200) {
        this.router.navigate(['/links']);
      } else {
        this.modelErrors = (<ApiBadRequestResponse>data).errors;
      }
      this.showSpinner = false;
    });
  }
  getData():any {
    this.showSpinner = true;
    this.linksService.getClrLinks()
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.clrLinks = (<ApiOkResponse>data).result as Array<ClrLinkVM>;
        } else {
          this.clrLinks = new Array<ClrLinkVM>();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
}
