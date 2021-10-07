import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { LoginService } from '@root/app/auth/auth.service';
import { NGXLogger } from 'ngx-logger';
@Injectable()
export class LoginGuard implements CanActivate {
	constructor(private loginService: LoginService, private router: Router, private logger: NGXLogger) {

	}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
		const returnUrl = route.queryParams['returnUrl'] ?? route.queryParams['ReturnUrl'];
		if (returnUrl)
		{
			this.logger.info('ReturnUrl: ', returnUrl);
			return true;
		}
		else {
			this.logger.info('Missing return url.');
			this.loginService.doLogin();
			return false;
		}
	}
}
