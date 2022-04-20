import { Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { environment } from "@environment/environment";
import { EmailVerificationService } from "@modules/access/services/email-verification.service";
import { UntilDestroy } from "@ngneat/until-destroy";
import { EmailVerificationModelInput } from "@shared/models/EmailVerificationModelInput";
import { take } from "rxjs/operators";

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
  type: number;
	message: string;
	private debug = false;

	constructor(private route: ActivatedRoute, private emailVerificationService: EmailVerificationService) {
    this.route.params
      .pipe(take(1)).subscribe(params => {
        this.type = params['type'] as number;
      });
	}

	verify({value, valid} : { value: EmailVerificationModelInput; valid: boolean}) {
		this.submitted = true;
		this.modelErrors = [];
		if (valid) {
      value.type = +this.type; // No idea why this NUMBER insists on passing as a string without the +...
			this.showSpinner = true;
			this.emailVerificationService.beginVerification(value)
      .subscribe(() => {
				this.complete = true;
				this.showSpinner = false;
			});
		}
	  }

	ngOnInit() {}
	ngOnDestroy(){}
}
