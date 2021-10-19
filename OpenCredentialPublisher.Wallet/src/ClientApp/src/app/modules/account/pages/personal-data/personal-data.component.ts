import { Component, OnInit } from '@angular/core';
import { DownloadService } from '@core/services/download.service';
import { environment } from '@environment/environment';
import { AccountService } from '@modules/account/account.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-personal-data',
  templateUrl: './personal-data.component.html',
  styleUrls: ['./personal-data.component.scss']
})
export class PersonalDataComponent implements OnInit {
  message = "";
  showSpinner = false;
  private debug = false;

  constructor(private acctService: AccountService, private downloads: DownloadService ) { }

  ngOnInit(): void {
  }
  downloadUserJson(){
    this.showSpinner = true;
    this.message = 'retrieving user data';
    this.acctService.getProfileData()
      .pipe(take(1))
      .subscribe(resp => {
        var contentDispositionHeader = resp.headers.get('Content-Disposition');
        var filename = contentDispositionHeader.split(';')[1].trim().split('=')[1].replace(/"/g, '');
        if (this.debug) console.log(`DisplayCredentialComponent VCJson:${JSON.stringify(resp.body)}`);
        this.downloads.saveAs(JSON.stringify(resp.body), filename, 'Pdf');
        this.showSpinner = false;
      });
  }
}
