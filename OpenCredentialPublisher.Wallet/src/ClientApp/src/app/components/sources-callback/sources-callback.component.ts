import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { SourceCallback } from '@shared/models/sourceCallback';
import { SourcesCallbackResponse } from '@shared/models/sourcesCallbackResponse';
import { Subscription } from 'rxjs';

@UntilDestroy()
@Component({
  selector: 'app-sources-callback',
  templateUrl: './sources-callback.component.html'
})
export class SourcesCallbackComponent implements OnInit {
  modelErrors = new Array<string>();
  private sub: Subscription;
  sourceCallback = new SourceCallback();
  private debug = false;
  constructor(
    private router: Router, private route: ActivatedRoute, ) {
  }

  ngOnInit() {
    this.route.data.subscribe((data: { response: SourcesCallbackResponse }) => {
      if (data.response.error) {
        if (this.debug) {
          console.log("Error: ", data.response.errorMessages);
        }
		    this.router.navigate(['/sources-error'],  { state: { errors: data.response.errorMessages }});
      }
      else {
        this.router.navigate(['/sources/source-list'], { queryParams: { sourceId: data.response.sourceId }});
      }
    });
  }
}
