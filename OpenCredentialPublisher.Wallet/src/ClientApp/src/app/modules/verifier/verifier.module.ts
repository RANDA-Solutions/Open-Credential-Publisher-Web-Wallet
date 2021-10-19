import { ModuleWithProviders, NgModule } from "@angular/core";
import { ProofService } from "@core/services/proof.services";
import { SharedModule } from "@shared/shared.module";
import { ProofInvitationComponent } from "./components/invitation/invitation.component";
import { ProofResponseComponent } from "./components/proof-response/proof-response.component";
import { CreateRequestComponent } from "./pages/create-request/create-request.component";
import { VerifierComponent } from "./pages/verifier.component";
import { ViewComponent } from "./pages/view/view.component";
import { VerifierService } from "./services/verifier.service";
import { verifierRouter } from "./verifier.router";

@NgModule({
    imports: [
      verifierRouter,
      SharedModule
    ],
    declarations: [
      CreateRequestComponent,
      ViewComponent,
	  VerifierComponent,
	  ProofInvitationComponent,
	  ProofResponseComponent	
      ],
      providers: [ ProofService, VerifierService],
      entryComponents: [
      ]
  })
export class VerifierModule {
  static forRoot(): ModuleWithProviders<VerifierModule> {
      return {
        ngModule: VerifierModule
      };
    }
  }