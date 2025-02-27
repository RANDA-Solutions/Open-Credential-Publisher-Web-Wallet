import { HttpClientModule } from '@angular/common/http';
import { ErrorHandler, NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { GlobalErrorHandler } from './error-handling/error.handler';
import { LogService } from './error-handling/logerror.service';
import { throwIfAlreadyLoaded } from './guards/module-import-guard';
import { AppService } from './services/app.service';
import { AuthorizationService } from './services/authorization.service';
import { ClrDetailService } from './services/clrdetail.service';
import { CredentialService } from './services/credentials.service';
import { UtilsService } from './services/utils.service';
@NgModule({
	imports: [
		HttpClientModule,
		BrowserAnimationsModule,
		ToastModule
	],
	providers: [
		AppService,
		AuthorizationService,
		ClrDetailService,
		CredentialService,
		LogService,
		MessageService,
		UtilsService,
		{
			provide: ErrorHandler,
			useClass: GlobalErrorHandler
		}
	],
	exports: [
		BrowserAnimationsModule,
		ToastModule
	],
	declarations: [
	]

})
export class CoreModule {
	constructor(@Optional() @SkipSelf()
		parentModule: CoreModule) {
		throwIfAlreadyLoaded(parentModule, 'CoreModule');
	}
}
