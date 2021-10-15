import { APP_BASE_HREF, CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CoreModule } from '@core/core.module';
import { AppService } from '@core/services/app.service';
import { environment } from '@environment/environment';
import { AuthModule, LogLevel } from 'angular-auth-oidc-client';
import { LoggerModule, NgxLoggerLevel } from 'ngx-logger';
import { MessageService } from 'primeng/api';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginCallbackComponent } from './auth/login-callback.component';
import { SourcesCallbackComponent } from './components/sources-callback/sources-callback.component';
import { SourcesErrorComponent } from './components/sources-error/sources-error.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SecureRoutesService } from './services/secureRoutes.service';
import { TokenInterceptorService } from './services/token-interceptor.service';

@NgModule({
	declarations: [
		AppComponent,
		LoginCallbackComponent,
		SourcesCallbackComponent,
		SourcesErrorComponent,
		NavMenuComponent
	],
	imports: [
		CommonModule,
		CoreModule,
		BrowserModule,
		FormsModule,
		ReactiveFormsModule,
		HttpClientModule,
		LoggerModule.forRoot({
			serverLoggingUrl: '/api/clientlog/ngxlogger',
			level: NgxLoggerLevel.DEBUG,
			serverLogLevel: NgxLoggerLevel.ERROR
		}),
		AppRoutingModule,
		AuthModule.forRoot({
			config: {
				authority: `${environment.baseUrl}`,
				clientId: 'ocp-wallet-client',
				configId: environment.configId,
				historyCleanupOff: true,
				logLevel: LogLevel.Error,
				postLoginRoute: `/credentials`,
				postLogoutRedirectUri: `${window.location.origin}/credentials`,
				renewTimeBeforeTokenExpiresInSeconds: 30,
				redirectUrl: `${window.location.origin}/callback`,
				responseType: 'code',
				scope: 'openid profile roles offline_access', // 'openid profile offline_access ' + your scopes
				silentRenew: true,
				startCheckSession: true,
				triggerAuthorizationResultEvent: true,
				unauthorizedRoute: '/unauthorized',
				useRefreshToken: true,
			},
		})
	],
	providers: [

		{ provide: APP_BASE_HREF, useValue: '/' }
		, AppService
		, MessageService
		, SecureRoutesService
		, {
			provide: HTTP_INTERCEPTORS,
			useClass: TokenInterceptorService,
			multi: true,
		}
	],
	bootstrap: [AppComponent]
})
export class AppModule {
	private debug = environment.debug;
	
	constructor() {
		
	}

}
