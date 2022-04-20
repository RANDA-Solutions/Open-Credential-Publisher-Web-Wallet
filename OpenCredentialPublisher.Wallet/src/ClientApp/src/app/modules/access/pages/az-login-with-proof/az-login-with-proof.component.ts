import { Component, NgZone, OnDestroy, OnInit } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { AzLoginProofService } from "@modules/access/services/az-login-proof.service";
import { LoginProofService } from "@modules/access/services/login-proof.service";
import { UntilDestroy } from "@ngneat/until-destroy";
import { LoginService } from "@root/app/auth/login.service";
import { ApiBadRequestResponse } from "@shared/models/apiBadRequestResponse";
import { ApiOkResponse } from "@shared/models/apiOkResponse";
import { AzLoginProofGetResponseModel } from "@shared/models/azLoginProofGetResponseModel";
import { LoginProofModel } from "@shared/models/LoginProofModel";
import { MSProofStatus } from "@shared/models/msProofStatus";
import { PackageList } from "@shared/models/packageList";
import { PackageVM } from "@shared/models/packageVM";
import { DeviceDetectorService } from "ngx-device-detector";
import { take } from "rxjs/operators";

@UntilDestroy()
@Component({
	selector: 'app-az-login-with-proof',
	templateUrl: './az-login-with-proof.component.html',
	styleUrls: ['./az-login-with-proof.component.scss']
})
export class AzLoginWithProofComponent implements OnInit, OnDestroy {
	showSpinner = false;
	modelErrors = new Array<string>();
	message: string = "loading";
	showUrl: boolean = false;
	getProofResponse: AzLoginProofGetResponseModel;
  msProofStatus: MSProofStatus;
  compName = 'AzLoginWithProofComponent';
	private timeout: any;
	private debug: boolean = true;

	constructor(private deviceDetector: DeviceDetectorService, private loginProofService: AzLoginProofService
    , private loginService: LoginService, private zone: NgZone, private router: Router
    , private sanitizer: DomSanitizer) {
	}

	ngOnInit() {

	}
  login() {
    this.showSpinner = true;
    let self = this;
		this.showUrl = this.deviceDetector.isMobile() || this.deviceDetector.isTablet();
    this.loginProofService.get()
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(`${this.compName} data: ${data}`);
        if (data.statusCode == 200) {
          this.getProofResponse = (<ApiOkResponse>data).result as AzLoginProofGetResponseModel;
          this.timeout = setInterval(() => {
            self.zone.run(() => {
              this.message = 'waiting for proof';
              this.showSpinner = true;
              this.loginProofService.status(this.getProofResponse.requestId).subscribe(response => {
                if (response.statusCode != 200) {
                  this.modelErrors = (<ApiBadRequestResponse>data).errors;
                  this.showSpinner = false;
                  clearInterval(this.timeout);
                  return;
                }
                // stausCode 200 yay!
                this.msProofStatus = (<ApiOkResponse>response).result as MSProofStatus;
                if (this.msProofStatus.status == 'Accepted') {
                  this.message = 'proof received... redirecting';
                  let returnUrl = this.loginService.returnUrl
                  this.loginService.completeLogin().then(resp => {
                    if (resp) {
                      clearInterval(this.timeout);
                      this.router.navigate([returnUrl ?? '/credentials']);
                    }
                  });
                }
                else if (this.msProofStatus.status != 'Waiting' && this.msProofStatus.status != '') {
                  this.modelErrors = new Array<string>();
                  this.modelErrors.push(this.msProofStatus.message);
                  this.showSpinner = false;
                  clearInterval(this.timeout);
                } else {
                  if (this.debug) {
                    console.log(this.msProofStatus.status);
                  }
                }
              });
            }, self);
          }, 2500);
        } else {
          if (this.debug) console.log(`${this.compName} getProof call did not return 200`);
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
          this.showSpinner = false;
        }
      });
  }
	ngOnDestroy(){
    clearInterval(this.timeout);
  }
  imageTransform(){
    if (this.getProofResponse && this.getProofResponse.image){
      var uri = `data:image/png;base64,${this.getProofResponse.image}`;
      return this.sanitizer.bypassSecurityTrustResourceUrl(uri);
    }
    return null;
  }
}
