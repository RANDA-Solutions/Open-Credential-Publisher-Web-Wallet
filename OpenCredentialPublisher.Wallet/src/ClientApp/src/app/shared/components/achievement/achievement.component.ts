import { Component, Input, OnInit } from '@angular/core';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AchievementVM } from '@shared/models/clrSimplified/achievementVM';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-achievement]',
  templateUrl: './achievement.component.html',
  styleUrls: ['./achievement.component.scss']
})
export class AchievementComponent implements OnInit {
  @Input() clrId: number;
  @Input() clrIdentifier: string;
  @Input() id: string;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  lineage: string;
  lineageKeys: string;
  achievement: AchievementVM;
  // endorsements = new Array<EndorsementVM>();
  showSpinner = false;
  private debug = false;
  constructor(private clrDetailService: ClrDetailService, private utilsService: UtilsService) { }

  ngOnChanges() {
    if (this.id != null) {
      this.showSpinner = true;
      if (this.debug) console.log('AchievementComponent ngOnChanges');
      this.getData();
    }
  }

  ngOnInit(): void {
    if (this.debug) console.log('AchievementComponent ngOnInit');
  }
  get safeAchievementId():string {
    return this.utilsService.safeId(this.achievement.id);
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('AchievementComponent getData');
    this.clrDetailService.getClrAchievement(this.clrId, this.id)
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.achievement = (<ApiOkResponse>data).result as AchievementVM;
          if (this.debug) console.log('AchievementComponent gotData');
          this.lineage = `${this.ancestors}.achievement`;
          this.lineageKeys = `${this.ancestorKeys}.${this.achievement.achievementId}`;
        } else {
          this.achievement = new AchievementVM();
          this.lineage = `${this.ancestors}.achievement`;
          this.lineageKeys = `${this.ancestorKeys}.-1`;
        }
        this.showSpinner = false;
      });
  }
}
