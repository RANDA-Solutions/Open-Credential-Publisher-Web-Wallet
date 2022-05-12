import { ModuleWithProviders, NgModule } from '@angular/core';
import { LogService } from '@core/error-handling/logerror.service';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { LinksService } from '@modules/links/links.service';
import { LinkDeleteComponent } from '@modules/links/pages/delete/link-delete.component';
import { SharedModule } from '@shared/shared.module';
import { LinkSummaryComponent } from './components/link-summary/link-summarycomponent';
import { ContactusComponent } from './pages/contactus/contactus.component';
import { LinkDisplayComponent } from './pages/link-display/link-display.component';
import { LockoutComponent } from './pages/lockout/lockout.component';
import { PrivacyComponent } from './pages/privacy/privacy.component';
import { TermsComponent } from './pages/terms/terms.component';
import { publicRouter } from './public.router';
@NgModule({
    imports: [
      publicRouter,
      SharedModule
    ],
    declarations: [
      LinkDisplayComponent,
      LinkSummaryComponent,
      LinkDeleteComponent,
      ContactusComponent,
      TermsComponent,
      PrivacyComponent,
      LockoutComponent
      ],
      providers: [ LinksService, ClrDetailService, LogService ],
      entryComponents: [
      ]
  })
export class PublicModule {
  static forRoot(): ModuleWithProviders<PublicModule> {
      return {
        ngModule: PublicModule
      };
    }
  }
