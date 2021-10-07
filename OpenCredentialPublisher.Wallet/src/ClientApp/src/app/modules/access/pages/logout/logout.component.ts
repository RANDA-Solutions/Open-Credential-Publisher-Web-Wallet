import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '@core/services/authorization.service';
import { LoginService } from '@root/app/auth/auth.service';

@Component({
	selector: 'app-logout',
	templateUrl: './logout.component.html'
})

export class LogoutComponent implements OnInit {

  message = 'logging out';
	constructor(
        private loginService: LoginService,
		private authorizationService: AuthorizationService
	) {
	}

	ngOnInit() {
    setTimeout(() => {
      this.loginService.signOut();
		}, 2000);
	}
}
