import { CommonModule } from "@angular/common";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { InjectionToken, ModuleWithProviders, NgModule, Optional, SkipSelf } from "@angular/core";
import { Router, RouterModule } from "@angular/router";
import { AuthGuard } from "./auth.guard";
import { AuthService } from "./auth.service";
import { AuthSettings } from "./auth.settings";
import { LoginService } from "./login.service";

export const AUTH_CONFIG = new InjectionToken<AuthSettings>('AUTH_CONFIG');
		
@NgModule({
	imports: [CommonModule, HttpClientModule, RouterModule],
	//providers: [AuthService, LoginService]
  })
export class AuthModule {
	constructor(@Optional() @SkipSelf() parentModule?: AuthModule) {
		if (parentModule) {
			throw new Error('AuthModule is already loaded. Import it in the AppModule only.');
		}
	}

	static forRoot(config: AuthSettings): ModuleWithProviders<AuthModule> {
		return {
			ngModule: AuthModule,
			providers: [
				{ provide: AUTH_CONFIG, useValue: config,  },
				{ provide: AuthService, deps: [AUTH_CONFIG]},
				{ provide: LoginService, deps: [AuthService, HttpClient, Router] },
				{ provide: AuthGuard, deps: [AuthService, LoginService, Router] }
			],
			
			
		}
	}
}