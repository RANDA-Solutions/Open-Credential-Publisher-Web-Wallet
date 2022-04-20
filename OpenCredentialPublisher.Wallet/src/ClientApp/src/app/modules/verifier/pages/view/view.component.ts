import { Component, NgZone, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AppService } from "@core/services/app.service";
import { ProofService } from "@core/services/proof.services";
import { environment } from "@environment/environment";
import { VerifierService } from "@modules/verifier/services/verifier.service";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ProofInvitation, ProofRequest } from "@shared/models/proofInvitation";
import { ProofResponse } from "@shared/models/proofResponse";

@UntilDestroy()
@Component(
	{
		selector: 'app-verifier-view-component',
  		templateUrl: './view.component.html',
  		styleUrls: ['./view.component.scss']
	}
)
export class ViewComponent implements OnInit, OnDestroy {
	constructor(private route: ActivatedRoute, private router: Router, private proofService: ProofService, private verifierService: VerifierService, private appService: AppService, private ngZone: NgZone) {}

	id: string;
	showSpinner = false;
	proof: ProofResponse;
	proofRequest: ProofRequest;
	invitation: ProofInvitation;
	isCreator: boolean;
	private _url: string;
	get url() { return  this._url }
	copied: boolean = false;
	private debug = false;
	private sub: any;

	public status: string = '';
	displayProof: boolean = false;
	displayInvitation: boolean = false;
	displayStatus: boolean = true;


	ngOnInit() {
		let self = this;
		this.appService.proofStatusChanged.pipe(untilDestroyed(this)).subscribe(status => {
			if (this.debug) console.log(status);
			this.ngZone.run((latestStatus) => {
				if (this.status !== latestStatus) {
					this.status = latestStatus;

					if (latestStatus == "InvitationLinkReceived") {
						this.proofService.getInvitation(this.id).subscribe(invitation => {
							this.showSpinner = false;
							this.invitation = invitation;
							this.displayInvitation = true;
						})
					}

					if (this.isCreator) {
						if (latestStatus == "ReceivingProofResponse") {
							this.showSpinner = true;
						}

						if (latestStatus == "ProofReceived") {
							this.appService.proofFinished.emit(true);
							this.displayInvitation = false;
						}
					}
				}
			}, self, [status]);
		 });

		 this.appService.proofFinished.pipe(untilDestroyed(this)).subscribe(finished => {
			 if (this.isCreator) {
				if (finished) {
					this.appService.killSignalR(this.appService.proofFlow);
					this.proofService.getProof(this.id).subscribe(proof => {
						this.ngZone.run((response) => {
							this.proof = response;
							this.displayInvitation = false;
							this.displayProof = true;
							this.displayStatus = false;
							this.showSpinner = false;
						}, self, [proof]);
					});
				}
			}
		 });
		 this._url = `${environment.baseUrl}${this.router.url}`;

		this.sub = this.route.params.pipe(untilDestroyed(this)).subscribe(params => {
			this.showSpinner = true;
			this.id = params['id'];
			this.isCreator = this.verifierService.isCreator(this.id);
			if (this.isCreator) {
				self.ngZone.run(() => {
					this.appService.connectSignalR(this.appService.proofFlow, this.id);
					this.proofService.getProofStatus(this.id).subscribe(status => {
						this.appService.proofStatusChanged.emit(status);
					});

					this.proofService.getProofInformation(this.id).subscribe(proofRequest => {
						this.proofRequest = proofRequest;
					});
				}, self);
			}
      else {
        this.proofService.getInvitation(this.id).subscribe(invitation => {
          this.showSpinner = false;
          this.invitation = invitation;
          this.displayInvitation = true;
        })
      }
		 });
	}

	copyUrlInput(input) {
		input.select();
		document.execCommand('copy');
		input.setSelectionRange(0, 0);
		this.copied = true;
		setTimeout(() => {
			this.copied = false;
		}, 2000);
	}

	getStatus(status: string) {
		return status.replace(/([A-Z])/g, ' $1').trim();
	}

	ngOnDestroy() {

	}
}
