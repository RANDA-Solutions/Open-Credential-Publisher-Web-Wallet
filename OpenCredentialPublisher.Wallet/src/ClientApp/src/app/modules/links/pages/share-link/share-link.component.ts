import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { LinksService } from '@modules/links/links.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { LinkShareVM } from '@shared/models/linkShareVM';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-share-link',
  templateUrl: './share-link.component.html',
  styleUrls: ['./share-link.component.scss']
})
export class ShareLinkComponent implements OnInit {
  id: string;
  redirectUrl: string;
  modelErrors = new Array<string>();
  public vm = new LinkShareVM();
  showSpinner = false;
  showIt = false;
  private sub: any;
  private debug = false;
  constructor(private route: ActivatedRoute, private router: Router, private linksService: LinksService) {
  }

  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('link-share ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.id = params['id'];
      this.getData(this.id );
   });
  }

  onSubmit():any {
    this.showSpinner = true;
    if (this.debug) console.log('link-share deleteSource');
    this.linksService.shareLink(this.vm)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/links'])
        } else {
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }

  getData(id: string):any {
    this.showSpinner = true;
    if (this.debug) console.log('link-share getData');
    this.linksService.getShareVM(id)
      .pipe(take(1)).subscribe(data => {
        this.showIt = true;
        if (data.statusCode == 200) {
          this.vm = (<ApiOkResponse>data).result as LinkShareVM;
          this.vm.recipientId = "";
          if (this.debug) console.log('link-share gotData');
        } else {
          this.vm = new LinkShareVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
}