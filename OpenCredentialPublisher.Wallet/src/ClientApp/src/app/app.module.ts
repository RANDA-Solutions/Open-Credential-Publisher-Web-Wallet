import { APP_BASE_HREF, CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CoreModule } from '@core/core.module';
import { AppService } from '@core/services/app.service';
import { environment } from '@environment/environment';
import { NgIdleModule } from '@ng-idle/core';
import { LoggerModule, NgxLoggerLevel } from 'ngx-logger';
import { WebStorageStateStore } from 'oidc-client';
import { MessageService } from 'primeng/api';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './auth/auth-client.service';
import { SourcesCallbackComponent } from './components/sources-callback/sources-callback.component';
import { SourcesErrorComponent } from './components/sources-error/sources-error.component';
import { WalletsFeatureGuard } from './guards/wallets-feature.guard';
import { LoginLayoutComponent } from './layout/login-layout/login-layout.component';
import { PlainLayoutComponent } from './layout/plain-layout/plain-layout.component';
import { SecureLayoutComponent } from './layout/secure-layout/secure-layout.component';
import { SiteFooterComponent } from './layout/site-footer/site-footer.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SecureRoutesService } from './services/secureRoutes.service';
import { TimeoutService } from './services/timeout.service';
import { TokenInterceptorService } from './services/token-interceptor.service';

@NgModule({
	declarations: [
		AppComponent,
		SiteFooterComponent,
		PlainLayoutComponent,
		SecureLayoutComponent,
		LoginLayoutComponent,
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
		NgIdleModule.forRoot(),
		AppRoutingModule,
		AuthModule.forRoot({
				authority: `${environment.baseUrl}`,
				client_id: 'ocp-wallet-client',
				loadUserInfo: true,
				lockSkew: 0,
				post_logout_redirect_uri: `${window.location.origin}/access/login`,
				redirect_uri: `${window.location.origin}/callback`,
				response_type: 'code',
				scope: 'openid profile roles offline_access', // 'openid profile offline_access ' + your scopes
				automaticSilentRenew: true,
				silent_redirect_uri: `${window.location.origin}/silent-renew.html`,
				accessTokenExpiringNotificationTime: 30,
				userStore: new WebStorageStateStore({
					store: window.localStorage
				}),
				filterProtocolClaims: true,
		})
	],
	providers: [

		{ provide: APP_BASE_HREF, useValue: '/' }
    , WalletsFeatureGuard
		, AppService
		, MessageService
		, SecureRoutesService
		, TimeoutService
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
