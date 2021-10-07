
import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { LoginService } from '../auth/auth.service';

@Component({
	selector: 'app-unauthorized',
	templateUrl: 'unauthorized.component.html'
})
export class UnauthorizedComponent implements OnInit {

	constructor(private location: Location, private loginService: LoginService) {

	}

	ngOnInit() {
	}

	login() {
		this.loginService.doLogin();
	}

	goback() {
		this.location.back();
	}
}
