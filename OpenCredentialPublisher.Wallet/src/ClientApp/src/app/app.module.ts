import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { FaIconLibrary, FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { AppComponent } from './app.component';
import { ListCandidateComponent, PortfolioCandidateComponent, PortfolioComponent } from './components';
import { CodeflowComponent } from './components/codeflow/codeflow.component';
import { HomeComponent } from './components/home/home.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { CandidateProfileComponent } from './components/portfolio/candidate-profile.component';
import { PortfolioDeleteModalComponent } from './components/portfolio/portfolio-delete-modal.component';
import { PortfolioVerificationModalComponent } from './components/portfolio/portfolio-verification-modal.component';
import { VerificationComponent } from './components/verification/verification.component';
import { DirectivesModule } from './modules/directives/directives.module';
import { PayflowModule } from './modules/payflow/payflow.module';
import { SafeUrlPipe } from './pipes/safe-url.pipe';
import { VerifyResolver } from './resolvers/verify.resolver';
import {
  CandidateService,
  CodeFlowService,
  HomeService,
  PortfolioService,
  VerificationService
} from './services/';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ListCandidateComponent,
    PortfolioCandidateComponent,
    PortfolioComponent,
    CodeflowComponent,
    SafeUrlPipe,
    PortfolioVerificationModalComponent,
    PortfolioDeleteModalComponent,
    CandidateProfileComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: PortfolioComponent, pathMatch: 'full', canActivate: [AuthorizeGuard]  },
      { path: 'candidate', component: ListCandidateComponent, canActivate: [AuthorizeGuard] },
      { path: 'codeflow', component: CodeflowComponent, canActivate: [AuthorizeGuard] },
      { path: 'folio', canActivate: [AuthorizeGuard], loadChildren: () => import('./modules/folio/folio.module').then(m => m.FolioModule)},
      { path: 'transactions', canActivate: [AuthorizeGuard], loadChildren: () => import('./modules/transactions/transactions.module').then(m => m.TransactionsModule)},
      { path: 'portfolio', component: PortfolioComponent, canActivate: [AuthorizeGuard] },
      { path: 'verify', component: VerificationComponent, canActivate: [AuthorizeGuard], resolve: [VerifyResolver]}
    ]),
    FontAwesomeModule,
    DirectivesModule,
    PayflowModule    
  ],
  providers: [
    CandidateService,
    CodeFlowService,
    HomeService,
    PortfolioService,
    VerificationService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor(library: FaIconLibrary) {
    // Add an icon to the library for convenient access in other components
    library.addIconPacks(fas);
  }
}
