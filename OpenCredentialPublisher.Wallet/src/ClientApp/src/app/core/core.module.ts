import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorHandler, NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthInterceptor } from 'angular-auth-oidc-client';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { GlobalErrorHandler } from './error-handling/error.handler';
import { LogService } from './error-handling/logerror.service';
import { AuthGuard } from './guards/auth.guard';
import { LoginGuard } from './guards/login.guard';
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
		// { provide: HTTP_INTERCEPTORS, useClass: HttpRequestInterceptor, multi: true },
		AppService,
		AuthorizationService,
		AuthGuard,
		ClrDetailService,
		CredentialService,
    LoginGuard,
		LogService,
		MessageService,
		UtilsService,
		{
			provide: ErrorHandler,
			useClass: GlobalErrorHandler
		},
		{
			provide: HTTP_INTERCEPTORS,
			useClass: AuthInterceptor,
			multi: true,
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
