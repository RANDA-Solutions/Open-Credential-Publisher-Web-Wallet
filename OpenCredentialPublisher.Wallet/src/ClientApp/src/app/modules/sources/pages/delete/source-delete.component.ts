import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { SourceDetail } from '@shared/models/sourceDetail';
import { take } from 'rxjs/operators';
import { SourcesService } from '../../sources.service';

@UntilDestroy()
@Component({
  selector: 'app-source-delete-component',
  templateUrl: './source-delete.component.html'
})
export class SourceDeleteComponent {
  id: string;
  redirectUrl: string;
  modelErrors = new Array<string>();
  public sourceDetail = new SourceDetail();
  showSpinner = false;
  private sub: any;
  private debug = false;
  constructor(private route: ActivatedRoute, private router: Router, private sourcesService: SourcesService) {
  }

  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('SourceDeleteComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.id = params['id'];

      this.getData(this.id );
   });
  }

  deleteConnection():any {
    this.showSpinner = true;
    if (this.debug) console.log('SourceDeleteComponent deleteConnection');
    this.sourcesService.deleteConnection(this.id)
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/sources/source-list'])
        } else {
          this.sourceDetail = new SourceDetail();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
  deleteSource():any {
    this.showSpinner = true;
    if (this.debug) console.log('SourceDeleteComponent deleteSource');
    this.sourcesService.deleteSource(this.id)
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/sources/source-list'])
        } else {
          this.sourceDetail = new SourceDetail();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }

  getData(id: string):any {
    this.showSpinner = true;
    if (this.debug) console.log('source-details getData');
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
}

