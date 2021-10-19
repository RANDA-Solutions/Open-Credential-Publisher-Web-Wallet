import { Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ProofService } from "@core/services/proof.services";
import { VerifierService } from "@modules/verifier/services/verifier.service";
import { UntilDestroy } from "@ngneat/until-destroy";
import { CreateProofRequest } from "@shared/interfaces/create-proof-request.interface";
import { ApiOkResponse } from "@shared/models/apiOkResponse";
import { CreateProofRequestPage } from "@shared/models/createProofRequestPage";
import { CreateProofResponse } from "@shared/models/createProofResponse";
import { CredentialSchemaModel } from "@shared/models/credentialSchemaModel";

@UntilDestroy()
@Component({
	selector: 'app-verifier-create-request-component',
	templateUrl: './create-request.component.html',
	styleUrls: ['./create-request.component.scss']
})
export class CreateRequestComponent implements OnInit, OnDestroy {
	constructor(private proofService: ProofService, private router: Router, private route: ActivatedRoute, private verifierService: VerifierService) {}
	model: CreateProofRequestPage;
	form: CreateProofRequest = { name: '', credentialSchemaId: null, notificationAddress: '' };
	modelErrors = new Array<string>();
	submitted: boolean = false;
	loading: boolean = true;
	message: string = "Loading Form...";

	ngOnInit() {
		this.loading = true;
		this.proofService.getPageModel().subscribe(response => {
			if (response.statusCode == 200) {
				let pageModel = (<ApiOkResponse>response).result as CreateProofRequestPage;
				let address = this.verifierService.getAddress();
				if (address) {
					this.form.notificationAddress = address;
				}
				this.model = pageModel;
				this.loading = false;
			}
		});
	}

	schemaName(schema: CredentialSchemaModel) {
		return schema.name.replace("_", " ");
	}

	submitRequest({value, valid} : { value: CreateProofRequest; valid: boolean}) {
		this.submitted = true;
		this.proofService.createProofRequest(value.credentialSchemaId, value.notificationAddress, value.name).subscribe(response => {
			if (response.statusCode == 200) {
				let createProofResponse = (<ApiOkResponse>response).result as CreateProofResponse;
				if (!createProofResponse.hasError) {
					this.verifierService.setCreator(createProofResponse.id);
					this.verifierService.setAddress(value.notificationAddress);
					this.router.navigate([createProofResponse.id], { relativeTo: this.route});
				}
			}
			else {
				this.submitted = false;
			}
		})
	}

	ngOnDestroy() {
		
	}
}