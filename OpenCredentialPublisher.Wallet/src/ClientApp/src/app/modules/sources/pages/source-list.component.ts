import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AuthorizationVM } from '@shared/models/authorization';
import { SourceDetail } from '@shared/models/sourceDetail';
import { take } from 'rxjs/operators';
import { SourcesService } from '../sources.service';

@Component({
  selector: 'app-source-list-component',
  templateUrl: './source-list.component.html'
})
export class SourceListComponent {
  modelErrors = new Array<string>();
  public authorizations = new Array<AuthorizationVM>();
  message = 'loading source list';
  showSpinner = false;
  private debug = false;
  constructor(private route: ActivatedRoute, private router: Router, private sourcesService: SourcesService) {
  }

  ngOnInit() {
    if (this.debug) console.log('source-list ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('source-list getData');
    this.sourcesService.getAuthorizationList()
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.authorizations = (<ApiOkResponse>data).result as Array<AuthorizationVM>;
        } else {
          this.authorizations = new Array<AuthorizationVM>();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.message = 'loading source list';
        this.showSpinner = false;
      });
  }
  refreshClrs(id: string):any {
    this.message = 'refreshing source credential';
    this.showSpinner = true;
    if (this.debug) console.log('SourceDetailComponent refreshClrs');
    this.sourcesService.refreshClrs(id)
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
            this.router.navigate([(<ApiOkResponse>data).redirectUrl]);
        } else {
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.message = 'loading details';
        this.showSpinner = false;
      });
  }
}

