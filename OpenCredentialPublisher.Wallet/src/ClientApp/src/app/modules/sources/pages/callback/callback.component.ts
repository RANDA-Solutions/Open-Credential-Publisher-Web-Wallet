import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SourcesService } from '@modules/sources/sources.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { SourceCallback } from '@shared/models/sourceCallback';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-register-callback',
  templateUrl: './callback.component.html'
})
export class RegisterCallbackComponent implements OnInit {
  modelErrors = new Array<string>();
  private sub: Subscription;
  sourceCallback = new SourceCallback();
  private debug = false;
  constructor(
    private router: Router, private route: ActivatedRoute, private sourcesService: SourcesService ) {
  }

  ngOnInit() {
    if (this.debug) console.log('RegisterCallbackComponent ngOnInit');
    this.sub = this.route.queryParams.pipe(untilDestroyed(this)).subscribe(
      (param: any) => {
         this.sourceCallback.code = param['code'];
         this.sourceCallback.scope = param['scope'];
         this.sourceCallback.state = param['state'];
         this.sourceCallback.error = param['error'];
         this.postData();
      });
  }
  postData():any {
    if (this.debug) console.log('RegisterCallbackComponent getData');
    this.sourcesService.postCallback(this.sourceCallback)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/sources/source-list', (<ApiOkResponse>data).result as number]);
        } else {
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
      });
  }
}
