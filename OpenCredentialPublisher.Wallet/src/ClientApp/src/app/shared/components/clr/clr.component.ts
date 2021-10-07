import { Component, Input, OnInit } from '@angular/core';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { CredentialService } from '@core/services/credentials.service';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { VerificationVM } from '@shared/models/clrSimplified/verificationVM';
import { ClrVM } from '@shared/models/clrVM';
import { forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-clr',
  templateUrl: './clr.component.html',
  styleUrls: ['./clr.component.scss']
})
export class ClrComponent implements OnInit {
  @Input() clrId: number;
  @Input() isShare: boolean;
  showSpinner = false;
  public clr = new ClrVM();
  public verification = new VerificationVM();
  message = 'loading clr';
  private debug = false;

  constructor(private credentialService: CredentialService, private clrDetailService: ClrDetailService) { }

  ngOnChanges() {
    this.showSpinner = true;
    if (this.debug) console.log('ClrComponent ngOnChanges');
    this.getData();
  }

  ngOnInit(): void {
    if (this.debug) console.log('ClrComponent ngOnInit');
  }
  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('ClrComponent getData');
    forkJoin({
      getClr: this.credentialService.getClrViewModelPlusAchievements(this.clrId).pipe(take(1)),
      getVerification: this.clrDetailService.getClrVerification(this.clrId).pipe(take(1))
    })
    .subscribe(({getClr, getVerification}) => {
      this.clr = getClr.statusCode == 200 ? (<ApiOkResponse>getClr).result as ClrVM : new ClrVM();
      this.verification = getVerification.statusCode == 200 ?
        (<ApiOkResponse>getVerification).result as VerificationVM : new VerificationVM();
        this.showSpinner = false;
    });
    // this.credentialService.getClrViewModelPlusAchievements(this.clrId)
    //   .pipe(take(1)).subscribe(data => {
    //     if (data.statusCode == 200) {
    //       this.clr = (<ApiOkResponse>data).result as ClrVM;
    //     } else {
    //       this.clr = new ClrVM();
    //     }
    //     this.showSpinner = false;
    //   });
  }

  isSelfPublished(): boolean {
    return this.clr.learner.id == this.clr.publisher.id;
  }

  publisherHeader() {
    return this.isSelfPublished() ? "Publisher (Self)" : "Publisher";
  }
}
