
import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { LoginService } from '../auth/login.service';

@Component({
	selector: 'app-unauthorized',
	styleUrls: ['./unauthorized.component.scss'],
	templateUrl: 'unauthorized.component.html'
})
export class UnauthorizedComponent implements OnInit {

	constructor(private location: Location, private loginService: LoginService) {

	}

	ngOnInit() {
	}

	login() {
		this.loginService.completeLogin();
	}

	goback() {
		this.location.back();
	}
}
