import { ModuleWithProviders, NgModule } from '@angular/core';
import { LogService } from '@core/error-handling/logerror.service';
import { SharedModule } from '@shared/shared.module';
import { SourceListComponent } from './pages/source-list.component';
import { sourceRouter } from './sources.router';
import { SourcesService } from './sources.service';
import { RegisterComponent } from './pages/register/register.component';
// import { SourceDetailComponent } from './pages/details/source-details.component';
import { SourceDeleteComponent } from './pages/delete/source-delete.component';
import { RegisterCallbackComponent } from './pages/callback/callback.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelectOpenBadgesComponent } from './pages/select-open-badges/select-open-badges.component';
@NgModule({
    imports: [
      sourceRouter,
      SharedModule,
      FormsModule,
      ReactiveFormsModule
    ],
    declarations: [
      SelectOpenBadgesComponent,
      // SourceDetailComponent,
      SourceDeleteComponent,
      SourceListComponent,
      RegisterComponent,
      RegisterCallbackComponent
      ],
      providers: [ SourcesService, LogService ],
      entryComponents: [
      ]
  })
export class SourcesModule {
  static forRoot(): ModuleWithProviders<SourcesModule> {
      return {
        ngModule: SourcesModule
      };
    }
  }
