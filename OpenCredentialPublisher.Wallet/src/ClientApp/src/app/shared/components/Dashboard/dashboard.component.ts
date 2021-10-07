import { Component, OnDestroy, OnInit } from '@angular/core';
import { DownloadService } from '@core/services/download.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { PdfRequestTypeEnum } from '@shared/models/enums/pdfRequestTypeEnum';
import { PdfRequest } from '@shared/models/pdfRequest';
import { take } from 'rxjs/operators';
import { Dashboard } from '../../models/Dashboard';
import { DashboardService } from './dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnDestroy, OnInit {
  dashboard = new Dashboard();
  message = 'loading highlights';
  showSpinner = false;
  private debug = false;

  constructor(private dashboardService: DashboardService, private downloads: DownloadService) {
  }

  ngOnInit() {
    if (this.debug) console.log('dashboard ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('dashboard getData');
    this.dashboardService.getDashboard()
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.dashboard = (<ApiOkResponse>data).result as Dashboard;
          if (this.debug) console.log(JSON.stringify(this.dashboard));

        } else {
          this.dashboard = new Dashboard();
        }
        this.showSpinner = false;
      });
  }

  showPdf(id: string, clrId: number, assertionId: string, evidenceName: string, artifactId: number, artifactName: string){
    const dr : PdfRequest = {
      requestType: PdfRequestTypeEnum.OwnerDownloadPdf,
      linkId: id,
      clrId: clrId,
      assertionId: assertionId,
      evidenceName: evidenceName,
      artifactId: artifactId,
      artifactName:artifactName,
      createLink: true
    }
    this.message = 'downloading transcript';
    this.showSpinner = true;
      if (this.debug) console.log('LinkListComponent showPdf');
      this.downloads.pdf(dr)
        .pipe(take(1)).subscribe(resp => {
          this.downloads.saveAs(resp.body, artifactName, 'Pdf');
          this.showSpinner = false;
          this.message = 'loading highlights';
        });

  }
  openPdf() {
    window.open('assets/documents/WalletInstructions.pdf', "_blank", "width=700,height=600");
  }
}
