import { Component, NgZone, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { EmailVerificationService } from "@modules/access/services/email-verification.service";
import { UntilDestroy } from "@ngneat/until-destroy";
import { EmailVerificationResponse } from "@shared/models/EmailVerificationResponse";
import { DeviceDetectorService } from "ngx-device-detector";
import { Subscription } from "rxjs";

@UntilDestroy()
@Component({
	selector: 'app-email-credential',
	templateUrl: './email-credential.component.html',
	styleUrls: ['./email-credential.component.scss']
})
export class EmailCredentialComponent implements OnInit, OnDestroy {
	private sub: Subscription;
	private timeout: any;
	private key: string;
	public showUrl: boolean = false;
	public model: EmailVerificationResponse;
	public showSpinner: boolean = true;
	public message: string = 'loading';
	public modelErrors: Array<string> = new Array<string>();


	constructor(private deviceDetector: DeviceDetectorService, private route: ActivatedRoute, private router: Router, private emailVerificationService: EmailVerificationService, private zone: NgZone) {
		this.key = this.route.snapshot.params.key;
	}

	ngOnInit() {
		let self = this;
		this.showUrl = this.deviceDetector.isMobile() || this.deviceDetector.isTablet();
		this.emailVerificationService.verify(this.key).subscribe(response => {
			this.model = response;
			this.showSpinner = false;
			if (response.errorMessage) {
				this.modelErrors.push(response.errorMessage);
			}
			else {
				this.timeout = setInterval(() => {
					self.zone.run(() => {
						this.message = 'checking email credential status';
						this.showSpinner = true;
						this.emailVerificationService.status(this.key).subscribe(response => {
							if (response.errorMessage) {
								this.modelErrors = new Array<string>();
								this.modelErrors.push(response.errorMessage);
								this.showSpinner = false;
								clearInterval(this.timeout);
								return;
							}
							if (response.status == 'Accepted') {
								this.message = 'email credential accepted... redirecting';
								this.router.navigate(['/access/login-with-proof']).then(() => {
									clearInterval(this.timeout);
								});
							}
							else if (response.status == 'Rejected') {
								this.modelErrors = new Array<string>();
								this.modelErrors.push('The email credential was rejected.  Please try again.');
								this.showSpinner = false;
								clearInterval(this.timeout);
							}
						});
					}, self);
				}, 2500);
			}
		});

	}
	ngOnDestroy(){}
}
