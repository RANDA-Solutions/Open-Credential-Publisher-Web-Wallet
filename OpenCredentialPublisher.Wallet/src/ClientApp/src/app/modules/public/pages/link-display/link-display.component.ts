import { DOCUMENT } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { PdfRequestTypeEnum } from '@shared/models/enums/pdfRequestTypeEnum';
import { LinkDisplayVM } from '@shared/models/linkDisplayVM';
import { EvidenceService } from '@shared/services/evidence.service';
import { take } from 'rxjs/operators';
import { LinksService } from '../../../links/links.service';

@UntilDestroy()
@Component({
  selector: 'app-link-display-component',
  templateUrl: './link-display.component.html',
  styleUrls: ['./link-display.component.scss']
})
export class LinkDisplayComponent {
  id: string;
  redirectUrl: string;
  modelErrors = new Array<string>();
  accessKey = '';
  canCreateProofRequest: boolean = false;

  public linkDisplay = new LinkDisplayVM();
  showSpinner = false;
  message: string = 'Loading';
  private debug = false;
  private sub: any;
  constructor(private route: ActivatedRoute,  private router: Router
    , private evidenceService: EvidenceService
    , private linksService: LinksService
    , public appService: AppService
    , @Inject(DOCUMENT) private document: any) {
  }

  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('LinkDisplayComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.id = params['id'];

      this.getData(this.id );
   });

  }

  onClickScroll(elementId: string): void {
    if (this.debug) console.log(`LinkDisplayComponent scrollto:${elementId}`);
    // this.vpScroller.scrollToAnchor(elementId);
    const elmnt = document.getElementById(elementId);
    elmnt.scrollIntoView({behavior: "smooth", block: "start", inline: "nearest"});
  }

  getData(id: string):any {
    this.showSpinner = true;
    if (this.debug) console.log('LinkDisplayComponent getData');
    this.linksService.getLinkDisplay(id, this.accessKey)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.linkDisplay = (<ApiOkResponse>data).result as LinkDisplayVM;
          if (!this.linkDisplay.requiresAccessKey)
            this.evidenceService.setRequestType(PdfRequestTypeEnum.OwnerViewPdf);
          if (this.debug) console.log(`link clrId: ${this.linkDisplay.clrId}`);
        } else {
          this.linkDisplay = new LinkDisplayVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
  access(){
    this.showSpinner = true;
    if (this.debug) console.log(`LinkDisplayComponent access key:${this.accessKey}`);
    this.linksService.linkAccess(this.id, this.accessKey)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.linkDisplay = (<ApiOkResponse>data).result as LinkDisplayVM;
          this.evidenceService.setAccessKey(this.accessKey);
          this.evidenceService.setRequestType(PdfRequestTypeEnum.LinkViewPdf);
          this.evidenceService.setLinkId(this.id);
          if (this.debug) console.log(`link clrId: ${this.linkDisplay.clrId}`);
        } else {
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
  refreshClrs():any {
    this.showSpinner = true;
    if (this.debug) console.log('LinkDisplayComponent refreshClrs');
    // this.sourcesService.refreshClrs(this.id)
    //   .pipe(take(1)).subscribe(data => {
    //     console.log(data);
    //     if (data.statusCode == 200) {
    //       this.router.navigateByUrl((<ApiOkResponse>data).redirectUrl);
    //     } else {
    //       this.sourceDetail = new SourceDetail();
    //       this.modelErrors = (<ApiBadRequestResponse>data).errors;
    //     }
    //     this.showSpinner = false;
    //   });
  }
  topFunction():void {
    if (this.debug) console.log('LinkDisplayComponent top');
    const element = document.getElementById ('theTop');
    element.scrollIntoView();
  }
}

