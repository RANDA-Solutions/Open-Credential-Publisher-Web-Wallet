import { Component, OnDestroy, OnInit } from '@angular/core';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { Profile } from '@shared/models/profile';
import { take } from 'rxjs/operators';
import { ProfileService } from './profile.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnDestroy, OnInit {
  profile = new Profile();
  showSpinner = false;
   private debug = false;

  constructor(private profileService: ProfileService) {
  }

  ngOnInit() {
    if (this.debug) console.log('profile ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }

  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('profile getData');
    this.profileService.getProfile()
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.profile = (<ApiOkResponse>data).result as Profile;
        } else {
          this.profile = new Profile();
        }
        this.showSpinner = false;
      });
  }
}
