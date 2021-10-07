import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LogService } from '@core/error-handling/logerror.service';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { SafeIdPipe } from '@shared/pipes/safeId.pipe';
import { SharedModule } from '@shared/shared.module';
import { CredentialService } from '../../core/services/credentials.service';
import { VerifiableCredentialComponent } from '../../shared/components/verifiable-credential/verifiable-credential.component';
import { AssertionPopupComponent } from './components/assertion-popup/assertion-popup';
import { ClrSectionAssertionComponent } from './components/clr-section-assertion/clr-section-assertion.component';
import { ClrSectionAssertionsComponent } from './components/clr-section-assertions/clr-section-assertions.component';
import { ClrSectionComponent } from './components/clr-section/clr-section.component';
import { ClrSetComponent } from './components/clr-set/clr-set.component';
import { PackageComponent } from './components/credential-package/credential-package.component';
import { PackagesComponent } from './components/credential-packages/credential-packages.component';
import { OBDetailComponent } from './components/ob-detail/ob-detail.component';
import { OpenBadgesComponent } from './components/open-badges/open-badges.component';
import { credentialRouter } from './credentials.router';
import { ConnectCredentialComponent } from './pages/connect/connect-credential.component';
import { CreateCollectionComponent } from './pages/create/create-collection.component';
import { CredentialListComponent } from './pages/credential-list.component';
import { DeleteCredentialComponent } from './pages/delete/delete-credential.component';
import { DisplayCredentialComponent } from './pages/display/display-credential.component';
import { ConnectCredentialResolver } from './services/connect-credential-resolver.service';
import { ConnectCredentialService } from './services/connect-credential.service';
import { CredentialFilterService } from './services/credentialFilter.service';
@NgModule({
    imports: [
      SharedModule,
      FormsModule,
      ReactiveFormsModule,
      credentialRouter
    ],
    declarations: [
      AssertionPopupComponent,
      ConnectCredentialComponent,
      CreateCollectionComponent,
      CredentialListComponent,
      DisplayCredentialComponent,
      DeleteCredentialComponent,
      SafeIdPipe,
      PackagesComponent,
      PackageComponent,
      OpenBadgesComponent,
      OBDetailComponent,
      ClrSectionComponent,
      VerifiableCredentialComponent,
      ClrSetComponent,
      ClrSectionAssertionsComponent,
      ClrSectionAssertionComponent
      ],
      providers: [ ConnectCredentialResolver, ConnectCredentialService, CredentialService, CredentialFilterService, ClrDetailService, LogService ],
      entryComponents: [
      ]
  })
export class CredentialsModule {
  static forRoot(): ModuleWithProviders<CredentialsModule> {
      return {
        ngModule: CredentialsModule
      };
    }
  }
