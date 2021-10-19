import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DownloadService } from '@core/services/download.service';
import { environment } from '@environment/environment';
import { LinksService } from '@modules/links/links.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { PdfRequestTypeEnum } from '@shared/models/enums/pdfRequestTypeEnum';
import { LinkVM } from '@shared/models/linkVM';
import { PdfRequest } from '@shared/models/pdfRequest';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-link-list',
  templateUrl: './link-list.component.html',
  styleUrls: ['./link-list.component.scss']
})
export class LinkListComponent implements OnInit {
  links = new Array<LinkVM>();
  redirectUrl = '';
  showSpinner = false;
  private debug = false;
  modelErrors = new Array<string>();
  message = 'loading links, this can take a while';
  constructor(private route: ActivatedRoute, private router: Router, private linksService: LinksService
    , private downloads: DownloadService ) {
  }
  ngOnInit() {
    this.showSpinner = true;
    this.getData();
  }
  showPdf(id: string, clrId: number, assertionId: string, evidenceName: string, artifactId: number, artifactName: string){
    const dr : PdfRequest = {
      requestType: PdfRequestTypeEnum.OwnerViewPdf,
      linkId: id,
      clrId: clrId,
      assertionId: assertionId,
      evidenceName: evidenceName,
      artifactId: artifactId,
      artifactName: artifactName,
      createLink: false
    }
    this.message = 'retrieving pdf';
    this.showSpinner = true;
      if (this.debug) console.log('LinkListComponent showPdf');
      this.downloads.pdf(dr)
      .pipe(take(1))
      .subscribe(resp => {
        let blob =  new Blob([resp.body], { type: 'application/pdf' })
        var fileURL = URL.createObjectURL(blob);
        if (fileURL) {
          if (this.debug) console.log(`LinkListComponent fileURL:${fileURL}`);
        }
        window.open(fileURL);
        this.showSpinner = false;
        this.message = 'loading links, this can take a while';
      });

  }
  getData():any {
    this.showSpinner = true;
    this.linksService.getLinks()
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.links = (<ApiOkResponse>data).result as Array<LinkVM>;
        } else {
          this.links = new Array<LinkVM>();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }

  getLink(url: string) {
    return url.replace(environment.baseUrl, '');
  }

}
