import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { AuthorizationService } from '@core/services/authorization.service';
import { environment } from '@environment/environment';
import { AccessService } from '@modules/access/services/access.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AuthService } from '@root/app/auth/auth.service';
import { LoginService } from '@root/app/auth/login.service';
import { Credentials } from '@shared/interfaces/credentials.interface';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { TwoFactorAuthenticationResultEnum } from '@shared/models/enums/twoFactorAuthenticationResultEnum';
import { TwoFactorAuthenticationModel } from '@shared/models/twoFactorAuthenticationModel';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
	selector: 'app-login-form',
	templateUrl: './login-form.component.html',
	styleUrls: ['./login-form.component.scss']
})

export class LoginFormComponent implements OnInit {
	externalApiUrl = `${environment.publicEndPoint}account/login/external/`;
	resultModel = new TwoFactorAuthenticationModel();
	modelErrors = new Array<string>();
	brandNew: boolean;
	errors: string;
	showSpinner = false;
	submitted = false;
	credentials: Credentials = { email: '', password: '' };
	infoMessage?: string;
	//externalProviders: AuthenticationSchemeModel[];

	private sub: Subscription;
	private returnUrl: string | null;
	private debug = false;

	constructor(private authorizationService: AuthorizationService,
		private loginService: LoginService,
		private appService: AppService,
		private accessService: AccessService,
		private authService: AuthService,
		private router: Router,
		private route: ActivatedRoute) {
	}

	ngOnInit() {
		if (this.debug) console.log(`LoginFormComponent ngOnInit`);
		// subscribe to router event
		this.sub = this.route.queryParams.pipe(untilDestroyed(this)).subscribe(
			(param: any) => {
				this.infoMessage = param['infoMessage'];
				this.brandNew = param['brandNew'];
				this.credentials.email = param['email'];
				if (this.returnUrl?.startsWith("/connect/authorize/callback") == false)
				{
					this.loginService.storeReturnUrl(this.returnUrl);
				}
				else 
				{
					this.returnUrl = '';
					history.pushState({ }, null, `${window.location.origin}/access/login`);
				}
			});
			
		this.authService.checkLogin().then((loggedIn) =>
		{
			if (environment.debug) {
				console.log("LoginForm: ", loggedIn);
			}
			if (loggedIn) {
				this.router.navigate(["/credentials"]);
			} 
		});
	}

	login({ value, valid }: { value: Credentials; valid: boolean }) {
		if (this.debug) console.log(`LoginFormComponent login`);
		this.submitted = true;
		this.errors = '';
		if (valid) {
			this.showSpinner = true;
			this.authorizationService.login(value.email, value.password, this.returnUrl)
				.pipe(take(1)).subscribe(data => {
					if (this.debug) console.log(`LoginFormComponent returned from api/Login result: ${this.resultModel.result}`);
					if (data.statusCode == 200) {
						this.resultModel = (<ApiOkResponse>data).result as TwoFactorAuthenticationModel;
					} else {
						this.modelErrors = (<ApiBadRequestResponse>data).errors;
					}
					if (this.resultModel.result == TwoFactorAuthenticationResultEnum.Success) {
						this.loginService.completeLogin().then(result => {
							if (result) {
								let returnUrl = this.loginService.returnUrl;
								if (returnUrl) {
									if (returnUrl.includes(environment.baseUrl)) {
										returnUrl = returnUrl.replace(environment.baseUrl, '');
									}
									this.loginService.clearReturnUrl();
									this.router.navigateByUrl(returnUrl);
								} 
								else {
									this.router.navigate(["/credentials"]);
								}
							}
							if (this.debug) console.log(`LoginFormComponent returned from OAuthService.doLogin()`);
						});
					} else if (this.resultModel.result == TwoFactorAuthenticationResultEnum.Required) {
						this.router.navigate(['/access/login-with2fa']);
					} else if (this.resultModel.result == TwoFactorAuthenticationResultEnum.Lockout) {
						this.router.navigate(['/public/lockout']);
					} else {
						this.modelErrors.push(this.resultModel.errorMessage);
					}
					this.showSpinner = false;
				}, (error) => {
					this.modelErrors.push("The provided credentials are not valid.  Please try again.");
					this.showSpinner = false;
				});
		}
	}

	external(name) {
		if (!!this.returnUrl)
			return `${this.externalApiUrl}${name}?returnUrl=${this.returnUrl}`;
		return `${this.externalApiUrl}${name}`;
	}
}
