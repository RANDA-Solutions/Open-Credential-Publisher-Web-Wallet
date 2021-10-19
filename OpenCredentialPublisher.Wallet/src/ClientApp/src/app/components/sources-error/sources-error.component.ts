import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-sources-error-component',
  templateUrl: './sources-error.component.html'
})
export class SourcesErrorComponent {
  modelErrors = new Array<string>();
  private debug = false;
  constructor(private route: ActivatedRoute, private router: Router) {
	  this.modelErrors = router.getCurrentNavigation()?.extras?.state.errors;
  }

  ngOnInit() {
    if (this.debug) console.log('SourcesErrorComponent ngOnInit');
  }
}

