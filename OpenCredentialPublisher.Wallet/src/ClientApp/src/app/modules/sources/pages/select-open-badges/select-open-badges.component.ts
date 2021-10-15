import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { CredentialService } from '@core/services/credentials.service';
import { SourcesService } from '@modules/sources/sources.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { BackpackPackage } from '@shared/models/backpackPackage';
import { OpenBadge } from '@shared/models/openBadge';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: '[app-select-open-badges]',
  templateUrl: './select-open-badges.component.html',
  styleUrls: ['./select-open-badges.component.scss']
})
export class SelectOpenBadgesComponent implements OnInit {
  packageId: number;
  backpackPackage = new BackpackPackage();
  showSpinner = false;
  miniSpinner = false;
  modelErrors = new Array<string>();
  private debug = false;
  message = 'loading badges';
  private sub: any;

  constructor(private route: ActivatedRoute, private router: Router, private credentialService: CredentialService
    , private sourcesService: SourcesService, private appService: AppService) { }


  ngOnInit() {
    this.showSpinner = true;
    if (this.debug) console.log('SelectOpenBadgesComponent ngOnInit');
    this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
      this.packageId = params['id'];
      console.log(params['id']);
      this.getData();
   });
  }

  ngOnDestroy(): void {
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('OpenBadgesComponent getData');
    this.sourcesService.getBadgesForSelection
    (this.packageId)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.backpackPackage = (<ApiOkResponse>data).result as BackpackPackage;
        } else {
          this.backpackPackage = new BackpackPackage();
        }
        this.showSpinner = false;
      });
  }

  selectionChange(badge: OpenBadge){
    badge.isSelected = !badge.isSelected;
  }

  selectBadges() {
    this.message = 'saving selections';
    this.showSpinner = true;
    if (this.debug) console.log('SourceDetailComponent refreshClrs');
    var ids = new Array<number>();
    this.backpackPackage.badges.forEach(el =>
      {
        if (el.isSelected) {
          ids.push(el.id)
        }
      }
    );
    this.sourcesService.selectBadges(this.packageId, ids)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/credentials/']);
        }
        this.message = 'loading details';
        this.showSpinner = false;
      });
  }
}
