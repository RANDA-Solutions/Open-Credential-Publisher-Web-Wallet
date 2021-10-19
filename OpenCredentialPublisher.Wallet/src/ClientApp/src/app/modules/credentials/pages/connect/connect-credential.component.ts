import { Component } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ConnectCredentialService } from "@modules/credentials/services/connect-credential.service";
import { ConnectCredentialResponseModel } from "@shared/interfaces/connect-credential-response.interface";
import { ConnectCredentialModel } from "@shared/interfaces/connect-credential.interface";

@Component({
	selector: "app-connect-credential",
	templateUrl: "./connect-credential.component.html",
	styleUrls: ["./connect-credential.component.scss"]
})
export class ConnectCredentialComponent {
	model: ConnectCredentialModel;
	modelErrors: any[] = [];
  message = 'adding data'
  showSpinner = false;
	constructor(private connectCredentialService: ConnectCredentialService, private activatedRoute: ActivatedRoute, private router: Router) {
		this.activatedRoute.data.subscribe(data => {
		  this.model = data.model;
		});
	  }

	  connect() {
      this.showSpinner = true;
		  this.connectCredentialService.connectPost(this.model).subscribe((result: ConnectCredentialResponseModel) => {
			  if (result.hasError) {
				  this.modelErrors = result.errorMessages;
          this.showSpinner = false;
			  }
			 else  {
        this.showSpinner = false;
				if (!!result.id)
					this.router.navigate(['/credentials/display/', result.id]);
			}
		  });
	  }
}
