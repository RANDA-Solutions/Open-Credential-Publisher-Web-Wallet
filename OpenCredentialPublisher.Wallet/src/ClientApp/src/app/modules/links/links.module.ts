import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LogService } from '@core/error-handling/logerror.service';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { SharedModule } from '@shared/shared.module';
import { linkRouter } from './links.router';
import { LinksService } from './links.service';
import { CreateLinkComponent } from './pages/create/create-link.component';
import { LinkListComponent } from './pages/link-list/link-list.component';
import { NotAvailableComponent } from './pages/not-available/not-available.component';
import { ShareLinkComponent } from './pages/share-link/share-link.component';
import { RecipientListComponent } from './pages/recipient-list/recipient-list.component';
import { DeleteRecipientComponent } from './pages/delete-recipient/delete-recipient.component';
import { CreateRecipientComponent } from './pages/create-recipient/create-recipient.component';
import { EditRecipientComponent } from './pages/edit-recipient/edit-recipient.component';
@NgModule({
    imports: [
      SharedModule,
      FormsModule,
      ReactiveFormsModule,
      linkRouter
    ],
    declarations: [
      LinkListComponent,
      CreateLinkComponent,
      NotAvailableComponent,
      ShareLinkComponent,
      RecipientListComponent,
      DeleteRecipientComponent,
      CreateRecipientComponent,
      EditRecipientComponent,
      ],
      providers: [ LinksService, ClrDetailService, LogService ],
      entryComponents: [
      ]
  })
export class LinksModule {
  static forRoot(): ModuleWithProviders<LinksModule> {
      return {
        ngModule: LinksModule
      };
    }
  }
