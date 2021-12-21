import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LinksService } from '@modules/links/links.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ClrLinkVM } from '@shared/models/clrLinkVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-create-link-component',
  styleUrls: [ './create-link.component.scss'],
  templateUrl: './create-link.component.html'
})
export class CreateLinkComponent  implements OnInit{
  redirectUrl = '';
  clrLinks = new Array<ClrLinkVM>();
  showSpinner = false;
  modelErrors = new Array<string>();
  
  form = new FormGroup({
    clrId: new FormControl(),
    nickname: new FormControl()
  });
  
  constructor(private route: ActivatedRoute, private router: Router, private linksService: LinksService) {
  }
  ngOnInit() {
    this.showSpinner = true;
    this.getData();
  }
  create() {
    var link = { 
      clrId: this.form.get("clrId").value,
      nickname: this.form.get("nickname").value
    } as ClrLinkVM;

    this.linksService.create(link)
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
