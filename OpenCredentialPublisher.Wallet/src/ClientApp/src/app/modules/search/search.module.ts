import { ModuleWithProviders, NgModule } from "@angular/core";
import { SharedModule } from "@shared/shared.module";
import { ProofInvitationComponent } from "./components/invitation/invitation.component";
import { SearchResponseComponent } from "./components/search-response/search-response.component";
import { SearchRequestComponent } from "./pages/search-request/search-request.component";
import { SearchComponent } from "./pages/search.component";
import { searchRouter } from "./search.router";
import { SearchService } from "./services/search.service";

@NgModule({
    imports: [
      searchRouter,
      SharedModule
    ],
    declarations: [
      SearchRequestComponent,
      SearchComponent,
	  ProofInvitationComponent,
	  SearchResponseComponent	
      ],
      providers: [SearchService],
      entryComponents: [
      ]
  })
export class SearchModule {
  static forRoot(): ModuleWithProviders<SearchModule> {
      return {
        ngModule: SearchModule
      };
    }
  }