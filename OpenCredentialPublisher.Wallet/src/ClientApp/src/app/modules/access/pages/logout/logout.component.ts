import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
	selector: 'app-logout',
	templateUrl: './logout.component.html'
})

export class LogoutComponent implements OnInit {

  message = 'logging out';
	constructor(
        private oidcSecurityService: OidcSecurityService,
		private router: Router
	) {
	}

	ngOnInit() {
		this.oidcSecurityService.logoffLocal();
		this.router.navigate(["/"]);
	}
}
