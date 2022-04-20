import { Component, NgZone, OnDestroy, OnInit } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { ActivatedRoute, Router } from "@angular/router";
import { AzLoginProofService } from "@modules/access/services/az-login-proof.service";
import { UntilDestroy } from "@ngneat/until-destroy";
import { ApiBadRequestResponse } from "@shared/models/apiBadRequestResponse";
import { ApiOkResponse } from "@shared/models/apiOkResponse";
import { AzLoginProofGetResponseModel } from "@shared/models/azLoginProofGetResponseModel";
import { EmailVerificationModelInput } from "@shared/models/EmailVerificationModelInput";
import { EmailVerificationResponse } from "@shared/models/EmailVerificationResponse";
import { MSProofStatus } from "@shared/models/msProofStatus";
import { DeviceDetectorService } from "ngx-device-detector";
import { take } from "rxjs/operators";

@UntilDestroy()
@Component({
	selector: 'app-az-email-credential',
	templateUrl: './az-email-credential.component.html',
	styleUrls: ['./az-email-credential.component.scss']
})
export class AzEmailCredentialComponent implements OnInit, OnDestroy {
  compName = 'AzAddEmailComponent';
  input = new EmailVerificationModelInput();
	showSpinner = false;
	modelErrors = new Array<string>();
	message: string = "loading";
	showUrl: boolean = false;
  public model: EmailVerificationResponse;
	getProofResponse = new AzLoginProofGetResponseModel();
  msProofStatus: MSProofStatus;
  submitted = false;
	complete = false;
  working = false;
  private key: string;
	private timeout: any;
	private debug: boolean = true;

	constructor(private deviceDetector: DeviceDetectorService, private loginProofService: AzLoginProofService
    , private route: ActivatedRoute,  private zone: NgZone, private router: Router
    , private sanitizer: DomSanitizer) {
      this.key = this.route.snapshot.params.key;
	}

	ngOnInit() {
    let self = this;
    this.showUrl = this.deviceDetector.isMobile() || this.deviceDetector.isTablet();
		this.loginProofService.verifyEmail(this.key)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          if (this.debug) console.log(`${this.compName} verifyEmail returned 200!`);
          this.working = true;
          this.getProofResponse = (<ApiOkResponse>data).result as AzLoginProofGetResponseModel;
          this.timeout = setInterval(() => {
            self.zone.run(() => {
              this.message = 'waiting for Authenticator response';
              this.loginProofService.issueStatus(this.getProofResponse.requestId).subscribe(response => {
                if (response.statusCode != 200) {
                  if (this.debug) console.log(`${this.compName} status call did not return 200`);
                  this.modelErrors = (<ApiBadRequestResponse>data).errors;
                  this.submitted = false;
                  this.working = false;
                  clearInterval(this.timeout);
                  return;
                }
                // stausCode 200 yay!
                this.msProofStatus = (<ApiOkResponse>response).result as MSProofStatus;
                if (this.msProofStatus.status == 'Accepted') {
                  if (this.debug) console.log(`${this.compName} status Accepted!`);
                  this.message = 'email added... redirecting';
                  clearInterval(this.timeout);
                  this.submitted = false;
                  this.working = false;
                  this.router.navigate(['/access/az-login-with-proof']);
                }
                else if (this.msProofStatus.status != 'Waiting' && this.msProofStatus.status != '') {
                  if (this.debug) console.log(`${this.compName} Unexpected status`);
                  this.modelErrors = new Array<string>();
                  this.modelErrors.push(this.msProofStatus.message);
                  this.submitted = false;
                  this.working = false;
                  clearInterval(this.timeout);
                } else {
                  if (this.debug) {
                    if (this.debug) console.log(`${this.compName} status: ${this.msProofStatus.status}`);
                  }
                }
              });
            }, self);
          }, 2500);
        } else {
          if (this.debug) console.log(`${this.compName} AddEmail call did not return 200`);
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
          this.working = false;
        }
      });
	}
	ngOnDestroy(){
  }
  imageTransform(){
    if (this.getProofResponse.image){
      var uri = `data:image/png;base64,${this.getProofResponse.image}`;
      return this.sanitizer.bypassSecurityTrustResourceUrl(uri);
    }
    return null;
  }

}
