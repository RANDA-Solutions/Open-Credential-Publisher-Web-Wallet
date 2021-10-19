import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { SourceTypeEnum } from '@shared/models/enums/sourceTypeEnum';
import { SourceDetail } from '@shared/models/sourceDetail';
import { take } from 'rxjs/operators';
import { SourcesService } from '../../sources.service';

@UntilDestroy()
@Component({
  selector: 'app-source-details-component',
  templateUrl: './source-details.component.html'
})
export class SourceDetailComponent {
  id: string;
  redirectUrl: string;
  modelErrors = new Array<string>();
  public sourceDetail = new SourceDetail();
  message = 'loading details';
  showSpinner = false;
  private sub: any;
  private debug = false;
  constructor(private route: ActivatedRoute, private router: Router, private sourcesService: SourcesService) {
  }

  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('SourceDetailComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.id = params['id'];

      this.getData(this.id );
   });
  }
  public get SourceTypeEnum() {
    return SourceTypeEnum;
  }

  getData(id: string):any {
    this.showSpinner = true;
    if (this.debug) console.log('SourceDetailComponent getData');
    this.sourcesService.getAuthorizationDetail(id)
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.sourceDetail = (<ApiOkResponse>data).result as SourceDetail;
        } else {
          this.sourceDetail = new SourceDetail();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
  refreshClrs():any {
    this.message = 'refreshing clrs';
    this.showSpinner = true;
    if (this.debug) console.log('SourceDetailComponent refreshClrs');
    this.sourcesService.refreshClrs(this.id)
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          if (this.sourceDetail.sourceType == 'Open Badge') {
            this.router.navigate(['/sources/select-open-badges', this.id]);
          } else {
            this.router.navigate(['/credentials/']);
          }
        } else {
          this.sourceDetail = new SourceDetail();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.message = 'loading details';
        this.showSpinner = false;
      });
  }
}

