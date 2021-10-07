import { APP_BASE_HREF, CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CoreModule } from '@core/core.module';
import { AppService } from '@core/services/app.service';
import { environment } from '@environment/environment';
import { AuthModule, EventTypes, LogLevel, OidcClientNotification, PublicEventsService } from 'angular-auth-oidc-client';
import { AuthStateResult } from 'angular-auth-oidc-client/lib/auth-state/auth-state';
import { LoggerModule, NgxLoggerLevel } from 'ngx-logger';
import { MessageService } from 'primeng/api';
import { filter } from 'rxjs/operators';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginService } from './auth/auth.service';
import { LoginCallbackComponent } from './auth/login-callback.component';
import { SourcesCallbackComponent } from './components/sources-callback/sources-callback.component';
import { SourcesErrorComponent } from './components/sources-error/sources-error.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

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
				configId: environment.configId,
				authority: `${environment.baseUrl}`,
				redirectUrl: `${window.location.origin}/callback`,
				postLogoutRedirectUri: `${window.location.origin}/credentials`,
				clientId: 'ocp-wallet-client',
				scope: 'openid profile roles offline_access', // 'openid profile offline_access ' + your scopes
				responseType: 'code',
				silentRenew: true,
				silentRenewUrl: `'${window.location.origin}/silent-renew.html`,
				triggerAuthorizationResultEvent: true,
				useRefreshToken: true,
				renewTimeBeforeTokenExpiresInSeconds: 30,
				logLevel: LogLevel.Error,
				postLoginRoute: `/credentials`,
				secureRoutes: environment.secureRoutes,
				historyCleanupOff: true
			},
		})
	],
	providers: [

		{ provide: APP_BASE_HREF, useValue: '/' }
		, AppService
		, MessageService
	],
	bootstrap: [AppComponent]
})
export class AppModule {
	private debug = false;
	constructor(private readonly eventService: PublicEventsService, private loginService: LoginService) {
		this.eventService
			.registerForEvents()
			.pipe(filter((notification) => notification.type === EventTypes.ConfigLoaded))
			.subscribe((config) => {
				// console.log('ConfigLoaded', config);
			});

		this.eventService
			.registerForEvents()
			.pipe(filter((notification) => notification.type === EventTypes.NewAuthenticationResult))
			.subscribe((result: OidcClientNotification<AuthStateResult>) => {
				console.log('isAuthenticated', result.value?.isAuthenticated);
				this.loginService.reportAuthState(result.value);
			});
	}

}
