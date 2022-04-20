import { Component, NgZone, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { LoginProofService } from "@modules/access/services/login-proof.service";
import { UntilDestroy } from "@ngneat/until-destroy";
import { LoginService } from "@root/app/auth/login.service";
import { LoginProofModel } from "@shared/models/LoginProofModel";
import { DeviceDetectorService } from "ngx-device-detector";

@UntilDestroy()
@Component({
	selector: 'app-az-login-with-proof',
	templateUrl: './login-with-proof.component.html',
	styleUrls: ['./login-with-proof.component.scss']
})
export class LoginWithProofComponent implements OnInit, OnDestroy {
	showSpinner = true;
	modelErrors = new Array<string>();
	message: string = "loading";
	showUrl: boolean = false;
	public model: LoginProofModel;
	private timeout: any;
	private debug: boolean = false;

	constructor(private deviceDetector: DeviceDetectorService, private loginProofService: LoginProofService, private loginService: LoginService, private zone: NgZone, private router: Router) {
		this.showSpinner = true;
	}

	ngOnInit() {
		let self = this;
		this.showUrl = this.deviceDetector.isMobile() || this.deviceDetector.isTablet();
		this.loginProofService.get().subscribe(response => {
			this.model = response;
			this.showSpinner = false;
			if (response.errorMessage) {
				this.modelErrors.push(response.errorMessage);
			}
			else {
				this.timeout = setInterval(() => {
					self.zone.run(() => {
						this.message = 'waiting for proof';
						this.showSpinner = true;
						this.loginProofService.status(this.model.id).subscribe(response => {
							if (response.error) {
								this.modelErrors = new Array<string>();
								this.modelErrors.push(response.errorMessage);
								this.showSpinner = false;
								clearInterval(this.timeout);
								return;
							}

							if (response.status == 'Accepted') {
								this.message = 'proof received... redirecting';
								let returnUrl = this.loginService.returnUrl
								this.loginService.completeLogin().then(resp => {
									if (resp) {
										clearInterval(this.timeout);
										this.router.navigate([returnUrl ?? '/credentials']);

									}
								});

							}
							else if (response.status == 'Rejected') {
								this.modelErrors = new Array<string>();
								this.modelErrors.push('The email credential was rejected.  Please try again.');
								this.showSpinner = false;
								clearInterval(this.timeout);
							} else {
								if (this.debug) {
									console.log(response.status);
								}
							}

						});
					}, self);
				}, 2500);
			}
		});
	}
	ngOnDestroy(){}
}
