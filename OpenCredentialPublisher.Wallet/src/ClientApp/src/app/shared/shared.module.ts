import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { LimitTextComponent } from '@shared/components/LimitText/limit-text.component';
import { ProfileComponent } from '@shared/components/Profile/profile.component';
import { SpinnerComponent } from '@shared/components/spinner/spinner.component';
import { QRCodeModule } from 'angularx-qrcode';
import { BootstrapModule } from './bootstrap.module';
import { AchievementComponent } from './components/achievement/achievement.component';
import { AddlPropertiesComponent } from './components/addl-properties/addl-properties.component';
import { AlignmentsComponent } from './components/alignments/alignments.component';
import { AssociationComponent } from './components/association/association.component';
import { AssociationsComponent } from './components/associations/associations.component';
import { ClrAssertionComponent } from './components/clr-assertion/clr-assertion.component';
import { ClrAssertionsComponent } from './components/clr-assertions/clr-assertions.component';
import { ClrProfileComponent } from './components/clr-profile/clr-profile.component';
import { ClrComponent } from './components/clr/clr.component';
import { DashboardComponent } from './components/Dashboard/dashboard.component';
import { EndorsementProfileComponent } from './components/endorsement-profile/endorsement-profile.component';
import { EndorsementsComponent } from './components/endorsements/endorsements.component';
import { EvidenceComponent } from './components/evidence/evidence.component';
import { MobileWalletsComponent } from './components/mobile-wallets/mobile-wallets.component';
import { PaginationComponent } from './components/Pagination/pagination.component';
import { PaginationService } from './components/Pagination/pagination.service';
import { ProofComponent } from './components/proof/proof.component';
import { PublicKeyComponent } from './components/public-key/public-key.component';
import { ResultComponent } from './components/result/result.component';
import { ResultsComponent } from './components/results/results.component';
import { SmartResumeComponent } from './components/smart-resume/smart-resume.component';
import { UploadComponent } from './components/upload/upload.component';
import { VCVerificationComponent } from './components/vc-verification/vc-verification.component';
import { VerificationComponent } from './components/verification/verification.component';
import { DisableControlDirective } from './directives/disable-control.directive';

@NgModule({
  imports: [
    BootstrapModule,
    CommonModule,
    RouterModule,
    QRCodeModule,
  ],
  declarations: [
    AchievementComponent,
    AddlPropertiesComponent,
    AlignmentsComponent,
    AssociationComponent,
    AssociationsComponent,
    ClrAssertionsComponent,
    ClrAssertionComponent,
    ClrComponent,
    ClrProfileComponent,
    DashboardComponent,
    DisableControlDirective,
    EndorsementsComponent,
    EndorsementProfileComponent,
    EvidenceComponent,
    LimitTextComponent,
    MobileWalletsComponent,
    PaginationComponent,
    ProfileComponent,
    PublicKeyComponent,
    ResultsComponent,
    ResultsComponent,
    ResultComponent,
    SmartResumeComponent,
    SpinnerComponent,
    UploadComponent,
    VCVerificationComponent,
    VerificationComponent,
    ProofComponent
  ],
  exports: [
    AchievementComponent,
    AddlPropertiesComponent,
    AlignmentsComponent,
    AssociationComponent,
    AssociationsComponent,
    BootstrapModule,
    ClrAssertionsComponent,
    ClrAssertionComponent,
    ClrComponent,
    ClrProfileComponent,
    CommonModule,
    DashboardComponent,
    DisableControlDirective,
    EndorsementsComponent,
    EndorsementProfileComponent,
    EvidenceComponent,
    /* https://angular.io/guide/sharing-ngmodules
       Even though the components declared by SharedModule might not bind with [(ngModel)] and there may be no need for
       SharedModule to import FormsModule, SharedModule can still export FormsModule without listing it among its imports.
       This way, you can give other modules access to FormsModule without having to import it directly into the @NgModule decorator. */
    FormsModule,
    LimitTextComponent,
    MobileWalletsComponent,
    PaginationComponent,
    ProfileComponent,
    QRCodeModule,
    ReactiveFormsModule,
    ResultsComponent,
    ResultComponent,
    SmartResumeComponent,
    SpinnerComponent,
    UploadComponent,
    VCVerificationComponent,
    VerificationComponent,
    ProofComponent
  ],
  providers: [PaginationService]

})
export class SharedModule { }
