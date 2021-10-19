import { Component, OnDestroy, OnInit } from "@angular/core";
import { environment } from "@environment/environment";
import { EmailVerificationService } from "@modules/access/services/email-verification.service";
import { UntilDestroy } from "@ngneat/until-destroy";
import { EmailVerificationModelInput } from "@shared/models/EmailVerificationModelInput";

@UntilDestroy()
@Component({
	selector: 'app-email-verification',
	templateUrl: './email-verification.component.html',
	styleUrls: ['./email-verification.component.scss']
})
export class EmailVerificationComponent implements OnInit, OnDestroy {
	input = new EmailVerificationModelInput();
	showSpinner = false;
	modelErrors = new Array<string>();
	submitted = false;
	complete = false;
	message: string;
	private debug = false;

	constructor(private emailVerificationService: EmailVerificationService) {

	}

	verify({value, valid} : { value: EmailVerificationModelInput; valid: boolean}) {
		this.submitted = true;
		this.modelErrors = [];
		if (valid) {
			this.showSpinner = true;
			this.emailVerificationService.beginVerification(value.emailAddress).subscribe(() => {
				this.complete = true;
				this.showSpinner = false;
			});
		}
	  }

	ngOnInit() {}
	ngOnDestroy(){}
}
