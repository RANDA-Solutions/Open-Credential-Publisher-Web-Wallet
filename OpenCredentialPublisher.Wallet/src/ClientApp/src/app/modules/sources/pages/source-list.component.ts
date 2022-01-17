import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AuthorizationVM } from '@shared/models/authorization';
import { take } from 'rxjs/operators';
import { SourcesService } from '../sources.service';

@UntilDestroy()
@Component({
  selector: 'app-source-list-component',
  templateUrl: './source-list.component.html'
})
export class SourceListComponent {
  modelErrors = new Array<string>();
  public authorizations = new Array<AuthorizationVM>();
  message = 'loading source list';
  private sourceId: string;
  showSpinner = false;
  private debug = false;
  constructor(private route: ActivatedRoute, private router: Router, private sourcesService: SourcesService) {
  }

  ngOnInit() {
    if (this.debug) console.log('source-list ngOnInit');
    this.showSpinner = true;
    this.route.queryParams.pipe(untilDestroyed(this)).subscribe(
			(param: any) => {
				this.sourceId = param['sourceId'];
        this.getData();
			});
  }

  ngOnDestroy(): void {
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('source-list getData');
    this.sourcesService.getAuthorizationList()
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.authorizations = (<ApiOkResponse>data).result as Array<AuthorizationVM>;
        } else {
          this.authorizations = new Array<AuthorizationVM>();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.message = 'loading source list';
        this.showSpinner = false;
        if (this.sourceId) {
          this.refreshClrs(this.sourceId);
        }
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

